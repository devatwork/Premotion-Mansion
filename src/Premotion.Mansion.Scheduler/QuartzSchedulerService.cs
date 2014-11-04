using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Quartz;
using Quartz.Impl;

namespace Premotion.Mansion.Scheduler
{
	public class QuartzSchedulerService
	{
		/// <summary>
		/// 
		/// </summary>
		public QuartzSchedulerService()
		{
			_schedFact = new StdSchedulerFactory();
			_sched = _schedFact.GetScheduler();
			_sched.Start();
		}



		public void ScheduleJob(IMansionContext context, Node jobNode)
		{
			var jobKey = GetJobKey(context, jobNode);
			var job = JobBuilder.Create<Job>()
				.WithIdentity(jobKey)
				.StoreDurably()
				.Build();
			job.JobDataMap.Put("applicationContext", context);
			job.JobDataMap.Put("jobNode", jobNode);

			var trigger = GetJobTrigger(context, jobNode);
			_sched.ScheduleJob(job, trigger);
		}



		/// <summary>
		/// Schedules the task depending on the last run datetime.
		/// If the last run is known, it schedules the next run accordingly, else it will start immediately.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobRecord"></param>
		/// <returns>Quartz ITrigger</returns>
		private static ITrigger GetJobTrigger(IMansionContext context, Record jobRecord)
		{
			var triggerTimeSpan = new TimeSpan();
			int triggerInterval;
			if (jobRecord.TryGet(context, "triggerIntervalSeconds", out triggerInterval))
				triggerTimeSpan = triggerTimeSpan.Add(TimeSpan.FromSeconds(triggerInterval));

			if (jobRecord.TryGet(context, "triggerIntervalMinutes", out triggerInterval))
				triggerTimeSpan = triggerTimeSpan.Add(TimeSpan.FromMinutes(triggerInterval));

			if (jobRecord.TryGet(context, "triggerIntervalHours", out triggerInterval))
				triggerTimeSpan = triggerTimeSpan.Add(TimeSpan.FromHours(triggerInterval));
			
			var simpleSchedule = SimpleScheduleBuilder.Create()
				.WithInterval(triggerTimeSpan)
				.RepeatForever();

			var dateTimeOffset = new DateTimeOffset();
			var lastRun = jobRecord.Get(context, "lastRun", DateTime.MinValue);
			if (lastRun != DateTime.MinValue)
				dateTimeOffset = new DateTimeOffset(lastRun).Add(triggerTimeSpan);
			
			if (lastRun != DateTime.MinValue && dateTimeOffset > DateTime.Now)
			{
				return TriggerBuilder.Create()
					.WithIdentity(GetTriggerKey(context, jobRecord))
					.StartAt(dateTimeOffset)
					.WithSchedule(simpleSchedule)
					.Build();
			}
			else
			{
				return TriggerBuilder.Create()
					.WithIdentity(GetTriggerKey(context, jobRecord))
					.StartNow()
					.WithSchedule(simpleSchedule)
					.Build();
			}
		}



		/// <summary>
		/// Remove the given task from the scheduler.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobNode"></param>
		public void DeleteJob(IMansionContext context, Record jobNode)
		{
			_sched.DeleteJob(GetJobKey(context, jobNode));
		}



		/// <summary>
		/// Remove the given task from the scheduler.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobNode"></param>
		public void TriggerJob(IMansionContext context, Record jobNode)
		{
			_sched.TriggerJob(GetJobKey(context, jobNode));
		}



		/// <summary>
		/// Get the job key based on the task and job.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobNode"></param>
		/// <returns></returns>
		private static JobKey GetJobKey(IMansionContext context, Record jobNode)
		{
			var jobName = jobNode.Get<string>(context, "name");
			var taskName = String.Format("{0} {1}", jobNode.Id, jobName);
			return new JobKey(taskName, jobName);
		}



		/// <summary>
		/// Get the trigger key based on the task and job.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobNode"></param>
		/// <returns></returns>
		private static TriggerKey GetTriggerKey(IMansionContext context, Record jobNode)
		{
			var jobName = jobNode.Get<string>(context, "name");
			var taskName = String.Format("{0} {1}", jobNode.Id, jobName);
			return new TriggerKey(taskName, jobName);
		}
		#region Private Fields
		private readonly ISchedulerFactory _schedFact;
		private readonly IScheduler _sched;
		#endregion
	}
}