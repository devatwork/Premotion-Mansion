using System;
using Premotion.Mansion.Core;
using Quartz;
using Quartz.Impl;

namespace Premotion.Mansion.Scheduler
{
	public class ExampleJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			var dataMap = context.MergedJobDataMap;
			var welcome = (string)dataMap["welcome"];
		}
	}

	public class QuartzSchedulerService
	{
		#region Constructors
		public QuartzSchedulerService()
		{
			_schedFact = new StdSchedulerFactory();
			_sched = _schedFact.GetScheduler();
			_sched.Start();
		}
		#endregion
		#region Implementation of ISchedulerService
		public void ScheduleJobs(IMansionContext context)
		{
			var jobKey = new JobKey("jobName", "jobGroup");
			var job = JobBuilder.Create<ExampleJob>()
				.WithIdentity(jobKey)
				.StoreDurably()
				.Build();
			job.JobDataMap.Put("welcome", "Hello, world!");


			JobBuilder.Create();
			var trigger = TriggerBuilder.Create()
				.WithIdentity("triggerName", "triggerGroup")
				.StartNow()
				.WithSimpleSchedule(x => x
					.WithIntervalInSeconds(10)
					.RepeatForever())
				.Build();



			_sched.ScheduleJob(job, trigger);
			_sched.Start();
		}

		#endregion
		#region Private Fields
		private readonly ISchedulerFactory _schedFact;
		private readonly IScheduler _sched;
		#endregion
	}
}