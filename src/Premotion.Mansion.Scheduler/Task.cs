using System;
using System.Collections.Concurrent;
using System.Net.Mail;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Web.Mail;
using Quartz;
using Quartz.Util;

namespace Premotion.Mansion.Scheduler
{
	public abstract class Task : IJob
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobProperties"></param>
		/// <param name="taskOutput"></param>
		/// <returns>Boolean indicating if the job ran successfully</returns>
		public abstract bool DoExecute(IMansionContext context, IPropertyBag jobProperties, ref StringBuilder taskOutput);

		private static readonly ConcurrentDictionary<int, object> JobLocks = new ConcurrentDictionary<int, object>();

		/// <summary>
		/// This method is called when the task is triggered to execute.
		/// </summary>
		/// <param name="jobContext"></param>
		public void Execute(IJobExecutionContext jobContext)
		{
			// Initialize and set context
			var dataMap = jobContext.MergedJobDataMap;
			var applicationContext = (MansionContext)dataMap["applicationContext"];
			var taskContext = new MansionContext(applicationContext.Nucleus);

			var jobNode = (Node)dataMap["jobNode"];
			var jobRecord = jobNode as Record;
			var task = GetType();
			var taskName = task.Name;
			JobLocks.TryAdd(jobRecord.Id, new object());

			var editProperties = new PropertyBag();
			var taskOutput = new StringBuilder();
			var mailReportWhenFailed = jobNode.Get(taskContext, "mailReportWhenFailed", false);

			using (RepositoryUtil.Open(taskContext))
			{
				try
				{
					var ranSuccessfully = DoExecute(taskContext, jobRecord, ref taskOutput);
					editProperties.Add(taskName + ".lastRunSuccessfull", ranSuccessfully);
					editProperties.Add(taskName + ".exceptionThrown", false);

					if (!ranSuccessfully)
					{
						if (mailReportWhenFailed)
							SendReportEmail(applicationContext, jobNode, task, taskOutput);
					}
				}
				catch (Exception e)
				{
					editProperties.Add(taskName + ".lastRunSuccessfull", false);
					editProperties.Add(taskName + ".exceptionThrown", true);
					editProperties.Add(taskName + ".exceptionMessage", e.Message);

					if (mailReportWhenFailed)
						SendReportEmail(applicationContext, jobNode, task, taskOutput, e);
				}
				finally
				{
					editProperties.Add(taskName + ".lastRun", DateTime.Now);
					editProperties.Add(taskName + ".taskOutput", taskOutput);
					editProperties.Add("_scheduleStatusUpdate", taskOutput);

					// Make sure no other task is editing the same JobNode at the same time
					var jobLock = JobLocks.GetOrAdd(jobRecord.Id, (key) => new object());
					lock (jobLock)
					{
						var repository = taskContext.Repository;
						repository.UpdateNode(taskContext, jobNode, editProperties);
					}
				}
			}
		}



		/// <summary>
		/// Send a report email
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobNode"></param>
		/// <param name="task"></param>
		/// <param name="taskOutput"></param>
		/// <param name="exception"></param>
		private static void SendReportEmail(IMansionContext context, IPropertyBag jobNode, Type task, StringBuilder taskOutput, Exception exception = null)
		{
			var reportEmailFrom =jobNode.Get<string>(context, "reportEmailFrom", null);
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
				message.Subject = String.Format("Job '{0}' failed while executing task '{1}'", jobNode.Get<string>(context, "name"), task.Name);

				message.Body = (exception != null) 
					? String.Format( "The following exception occurred while executing task '{0}': {1}", task.Name, exception.Message)
					: String.Format("The task '{0}' failed and gave the following output: {1}", task.Name, taskOutput);
	
				mailService.Send(context, message);
			}
		}
	}
}