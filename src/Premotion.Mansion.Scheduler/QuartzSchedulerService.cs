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



		/// <summary>
		/// Add the given task to the scheduler.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="task"></param>
		/// <param name="jobNode"></param>
		public void ScheduleTask(IMansionContext context, Type task, Node jobNode)
		{
			var jobKey = GetJobKey(context, task, jobNode);
			var job = JobBuilder.Create(task)
				.WithIdentity(jobKey)
				.StoreDurably()
				.Build();
			job.JobDataMap.Put("applicationContext", context);
			job.JobDataMap.Put("jobNode", jobNode);

			var trigger = GetTaskTrigger(context, task, jobNode);
			_sched.ScheduleJob(job, trigger);
		}



		/// <summary>
		/// Schedules the task depending on the last run datetime.
		/// If the last run is known, it schedules the next run accordingly, else it will start immediately.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="task"></param>
		/// <param name="jobNode"></param>
		/// <returns>Quartz ITrigger</returns>
		private static ITrigger GetTaskTrigger(IMansionContext context, Type task, Node jobNode)
		{
			var triggerTimeSpan = new TimeSpan();
			int triggerInterval;
			if (jobNode.TryGet(context, "triggerIntervalSeconds", out triggerInterval))
				triggerTimeSpan = triggerTimeSpan.Add(TimeSpan.FromSeconds(triggerInterval));

			if (jobNode.TryGet(context, "triggerIntervalMinutes", out triggerInterval))
				triggerTimeSpan = triggerTimeSpan.Add(TimeSpan.FromMinutes(triggerInterval));

			if (jobNode.TryGet(context, "triggerIntervalHours", out triggerInterval))
				triggerTimeSpan = triggerTimeSpan.Add(TimeSpan.FromHours(triggerInterval));
			
			var simpleSchedule = SimpleScheduleBuilder.Create()
				.WithInterval(triggerTimeSpan)
				.RepeatForever();

			var dateTimeOffset = new DateTimeOffset();
			var lastRun = jobNode.Get(context, task.Name + ".lastRun", DateTime.MinValue);
			if (lastRun != DateTime.MinValue)
				dateTimeOffset = new DateTimeOffset(lastRun).Add(triggerTimeSpan);
			
			if (lastRun != DateTime.MinValue && dateTimeOffset > DateTime.Now)
			{
				return TriggerBuilder.Create()
					.WithIdentity(GetTriggerKey(context, task, jobNode))
					.StartAt(dateTimeOffset)
					.WithSchedule(simpleSchedule)
					.Build();
			}
			else
			{
				return TriggerBuilder.Create()
					.WithIdentity(GetTriggerKey(context, task, jobNode))
					.StartNow()
					.WithSchedule(simpleSchedule)
					.Build();
			}
		}



		/// <summary>
		/// Remove the given task from the scheduler.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="task"></param>
		/// <param name="jobNode"></param>
		public void DeleteTask(IMansionContext context, Type task, Node jobNode)
		{
			_sched.DeleteJob(GetJobKey(context, task, jobNode));
		}



		/// <summary>
		/// Remove the given task from the scheduler.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="task"></param>
		/// <param name="jobNode"></param>
		public void TriggerTask(IMansionContext context, Type task, Node jobNode)
		{
			_sched.TriggerJob(GetJobKey(context, task, jobNode));
		}



		/// <summary>
		/// Get the job key based on the task and job.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobNode"></param>
		/// <param name="task"></param>
		/// <returns></returns>
		private static JobKey GetJobKey(IMansionContext context, Type task, Node jobNode)
		{
			var jobName = jobNode.Get<string>(context, "name");
			var taskName = String.Format("{0} {1}, {2}", jobNode.Id, jobName, task);
			return new JobKey(taskName, jobName);
		}



		/// <summary>
		/// Get the trigger key based on the task and job.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="task"></param>
		/// <param name="jobNode"></param>
		/// <returns></returns>
		private static TriggerKey GetTriggerKey(IMansionContext context, Type task, Node jobNode)
		{
			var jobName = jobNode.Get<string>(context, "name");
			var taskName = String.Format("{0} {1}, {2}", jobNode.Id, jobName, task);
			return new TriggerKey(taskName, jobName);
		}
		#region Private Fields
		private readonly ISchedulerFactory _schedFact;
		private readonly IScheduler _sched;
		#endregion
	}
}