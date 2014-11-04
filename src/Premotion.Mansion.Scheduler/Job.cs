using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
		private readonly object _jobLock = new object();

		/// <summary>
		/// This method is called when the job is triggered to execute.
		/// </summary>
		/// <param name="jobContext"></param>
		public void Execute(IJobExecutionContext jobContext)
		{
			// Initialize and set context
			var dataMap = jobContext.MergedJobDataMap;
			var applicationContext = (MansionContext) dataMap["applicationContext"];
			var context = new MansionContext(applicationContext.Nucleus);
			var jobNode = (Node) dataMap["jobNode"];
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();
			var type = typeService.Load(context, jobNode.Type);
			var taskDescriptors = type.GetDescriptors<RegisterTaskDescriptor>().Select(descriptor => descriptor.TaskType);

			var mailReportWhenFailed = jobNode.Get(context, "mailReportWhenFailed", false);

			foreach (var taskDescriptor in taskDescriptors)
			{
				var editProperties = new PropertyBag();
				var taskOutput = new StringBuilder();
				var task = (ITask) Activator.CreateInstance(taskDescriptor);

				using (RepositoryUtil.Open(context))
				{
					try
					{
						var ranSuccessfully = task.DoExecute(context, jobNode, ref taskOutput);
						editProperties.Add(taskDescriptor.Name + ".lastRunSuccessfull", ranSuccessfully);
						editProperties.Add(taskDescriptor.Name + ".exceptionThrown", false);

						if (!ranSuccessfully)
						{
							if (mailReportWhenFailed)
								SendReportEmail(applicationContext, jobNode, taskDescriptor, taskOutput);
						}
					}
					catch (Exception e)
					{
						editProperties.Add(taskDescriptor.Name + ".lastRunSuccessfull", false);
						editProperties.Add(taskDescriptor.Name + ".exceptionThrown", true);
						editProperties.Add(taskDescriptor.Name + ".exceptionMessage", e.Message);

						if (mailReportWhenFailed)
							SendReportEmail(applicationContext, jobNode, taskDescriptor, taskOutput, e);
					}
					finally
					{
						editProperties.Add(taskDescriptor.Name + ".lastRun", DateTime.Now);
						editProperties.Add(taskDescriptor.Name + ".taskOutput", taskOutput);
						editProperties.Add("_scheduleStatusUpdate", taskOutput);

						lock (_jobLock)
						{
							var repository = context.Repository;
							repository.UpdateNode(context, jobNode, editProperties);
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