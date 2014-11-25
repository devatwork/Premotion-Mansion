using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Mail;
using Quartz;
using Quartz.Util;

namespace Premotion.Mansion.Scheduler
{
	public class Job : IJob
	{
		private List<RegisterTaskDescriptor> _queuedTaskDescriptors;
		private readonly List<RegisterTaskDescriptor> _finishedTaskDescriptors = new List<RegisterTaskDescriptor>();
		private readonly object _jobLock = new object();
		private readonly object _queueLock = new object();

		/// <summary>
		/// This method is called when the job is triggered to execute.
		/// </summary>
		/// <param name="jobContext"></param>
		public void Execute(IJobExecutionContext jobContext)
		{
			// Initialize and set context
			var dataMap = jobContext.MergedJobDataMap;
			var applicationContext = (MansionContext) dataMap["applicationContext"];
			var jobNode = (Node) dataMap["jobNode"];


			var typeService = applicationContext.Nucleus.ResolveSingle<ITypeService>();
			var type = typeService.Load(applicationContext, jobNode.Type);

			// get all tasks
			_queuedTaskDescriptors = type.GetDescriptors<RegisterTaskDescriptor>()
				.Select(descriptor => descriptor).ToList();

			// Save the current time as the last run of the current Job
			var context = new MansionContext(applicationContext.Nucleus);
			using (RepositoryUtil.Open(context))
			{
				var repository = context.Repository;
				repository.UpdateNode(context, jobNode, new PropertyBag
				{
					{"lastRun", DateTime.Now}
				});
			}

			// Start tasks that have no dependency on other tasks
			foreach (var taskDescriptor in _queuedTaskDescriptors
				.Select(descriptor => descriptor)
				.Where(descriptor => descriptor.TaskWaitsFor.IsNullOrWhiteSpace()))
			{
				var descriptor = taskDescriptor;
				Task.Factory.StartNew(() => ExecuteTask(applicationContext, descriptor, jobNode));
			}
		}



		/// <summary>
		/// Start the given task
		/// </summary>
		/// <param name="applicationContext"></param>
		/// <param name="taskDescriptor"></param>
		/// <param name="jobNode"></param>
		private void ExecuteTask(IMansionContext applicationContext, RegisterTaskDescriptor taskDescriptor, Node jobNode)
		{
			// Initialize and set context
			var mailReportWhenFailed = jobNode.Get(applicationContext, "mailReportWhenFailed", false);
			var context = new MansionContext(applicationContext.Nucleus);
			var editProperties = new PropertyBag();
			var taskOutput = new StringBuilder();
			var taskType = taskDescriptor.TaskType;
			var task = (ITask) Activator.CreateInstance(taskType);
			bool ranSuccessfully = false;

			using (RepositoryUtil.Open(context))
			{
				var taskStopwatch = new Stopwatch();
				try
				{
					// Execute the asynchronous task
					taskStopwatch.Start();
					var theActualTask = Task<bool>.Factory.StartNew(() => task.DoExecute(context, jobNode, ref taskOutput));
					Task.Factory.ContinueWhenAll(new[] {theActualTask}, tasks => taskStopwatch.Stop());
					ranSuccessfully = theActualTask.Result;

					editProperties.Add(taskType.Name + ".exceptionThrown", false);

					if (!ranSuccessfully)
					{
						if (mailReportWhenFailed)
							SendReportEmail(applicationContext, jobNode, taskType, taskOutput);
					}
				}
				catch (Exception e)
				{
					// Catch any exception that is thrown by the task and save it on the job node
					ranSuccessfully = false;
					editProperties.Add(taskType.Name + ".exceptionThrown", true);
					editProperties.Add(taskType.Name + ".exceptionMessage", e.InnerException.Message);

					if (mailReportWhenFailed)
						SendReportEmail(applicationContext, jobNode, taskType, taskOutput, e.InnerException);
				}
				finally
				{
					// Update job node with task results
					editProperties.Add(taskType.Name + ".lastRunSuccessfull", ranSuccessfully);
					editProperties.Add(taskType.Name + ".lastRun", DateTime.Now);
					editProperties.Add(taskType.Name + ".lastDuration", taskStopwatch.Elapsed);
					editProperties.Add(taskType.Name + ".taskOutput", taskOutput);
					editProperties.Add("_scheduleStatusUpdate", taskOutput);

					lock (_jobLock)
					{
						var repository = context.Repository;
						repository.UpdateNode(context, jobNode, editProperties);
					}

					
					if (ranSuccessfully)
					{
						// Find out what tasks can be started next
						lock (_queueLock)
						{
							_queuedTaskDescriptors.Remove(taskDescriptor);
							_finishedTaskDescriptors.Add(taskDescriptor);

							// Only loop those tasks that have a dependency on the current task
							foreach (var descriptor in _queuedTaskDescriptors
								.Where(descriptor => !descriptor.TaskWaitsFor.IsNullOrWhiteSpace())
								.Where(descriptor => descriptor.TaskWaitsFor.Split(',')
									.Contains(taskDescriptor.TaskId.ToString(CultureInfo.InvariantCulture))))
							{
								var waitsFor = descriptor.TaskWaitsFor.Split(',');


								// Check if all other dependencies of the queued task are finished
								var allDependenciesFinished = waitsFor.All(waitFor => _finishedTaskDescriptors
									.Select(finishedDescriptor => finishedDescriptor)
									.Count(finishedDescriptor => finishedDescriptor.TaskId.ToString(CultureInfo.InvariantCulture).Equals(waitFor)) != 0);

								if (allDependenciesFinished)
									Task.Factory.StartNew(() => ExecuteTask(applicationContext, descriptor, jobNode));
							}
						}
					}
				}
			}
			
		}


		/// <summary>
		/// Send a report email
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobNode"></param>
		/// <param name="taskDescriptor"></param>
		/// <param name="taskOutput"></param>
		/// <param name="exception"></param>
		private static void SendReportEmail(IMansionContext context, IPropertyBag jobNode, Type taskDescriptor,
			StringBuilder taskOutput, Exception exception = null)
		{
			var reportEmailFrom = jobNode.Get<string>(context, "reportEmailFrom", null);
			if (reportEmailFrom.IsNullOrWhiteSpace())
				return;

			var reportEmailTo = jobNode.Get<string>(context, "reportEmailTo", null);
			if (reportEmailTo.IsNullOrWhiteSpace())
				return;

			var mailService = context.Nucleus.ResolveSingle<IMailService>();
			using (var message = mailService.CreateMessage())
			{
				message.To.Add(reportEmailTo);
				message.From = new MailAddress(reportEmailFrom);
				message.Subject = String.Format("Job '{0}' failed while executing task '{1}'", jobNode.Get<string>(context, "name"),
					taskDescriptor.Name);

				message.Body = (exception != null)
					? String.Format("The following exception occurred while executing task '{0}': {1}", taskDescriptor.Name,
						exception.Message)
					: String.Format("The task '{0}' failed and gave the following output: {1}", taskDescriptor.Name, taskOutput);

				mailService.Send(context, message);
			}
		}
	}
}