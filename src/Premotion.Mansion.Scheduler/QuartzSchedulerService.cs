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
		/// 
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
			job.JobDataMap.Put("context", context);
			job.JobDataMap.Put("record", jobNode);


			var triggerTimeSpan = new TimeSpan();

			
			int triggerInterval;
			if (jobNode.TryGet(context, "triggerIntervalSeconds", out triggerInterval))
				triggerTimeSpan = triggerTimeSpan.Add(TimeSpan.FromSeconds(triggerInterval));

			if (jobNode.TryGet(context, "triggerIntervalMinutes", out triggerInterval))
				triggerTimeSpan = triggerTimeSpan.Add(TimeSpan.FromMinutes(triggerInterval));

			if (jobNode.TryGet(context, "triggerIntervalHours", out triggerInterval))
				triggerTimeSpan = triggerTimeSpan.Add(TimeSpan.FromHours(triggerInterval));

			var simpleSchedule = SimpleScheduleBuilder.Create().WithInterval(triggerTimeSpan).RepeatForever();

			JobBuilder.Create();
			var trigger = TriggerBuilder.Create()
				.WithIdentity(GetTriggerKey(context, task, jobNode))
				.StartNow()
				.WithSchedule(simpleSchedule)
				.Build();

			_sched.ScheduleJob(job, trigger);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="task"></param>
		/// <param name="jobNode"></param>
		public void DeleteTask(IMansionContext context, Type task, Node jobNode)
		{
			_sched.DeleteJob(GetJobKey(context, task, jobNode));
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobNode"></param>
		/// <param name="task"></param>
		/// <returns></returns>
		private JobKey GetJobKey(IMansionContext context, Type task, Node jobNode)
		{
			var jobName = jobNode.Get<string>(context, "name");
			var taskName = String.Format("{0} {1}, {2}", jobNode.Id, jobName, task);
			return new JobKey(taskName, jobName);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="task"></param>
		/// <param name="jobNode"></param>
		/// <returns></returns>
		private TriggerKey GetTriggerKey(IMansionContext context, Type task, Node jobNode)
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