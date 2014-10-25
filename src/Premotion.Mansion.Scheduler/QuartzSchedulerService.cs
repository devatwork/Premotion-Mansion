using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Quartz;
using Quartz.Impl;

namespace Premotion.Mansion.Scheduler
{
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
		public void ScheduleTask(IMansionContext context, Task task, Node jobNode)
		{
			var jobKey = new JobKey("jobName", "jobGroup");
			var job = JobBuilder.Create(task.GetType())
				.WithIdentity(jobKey)
				.StoreDurably()
				.Build();
			job.JobDataMap.Put("context", context);
			job.JobDataMap.Put("record", jobNode);


			JobBuilder.Create();
			var trigger = TriggerBuilder.Create()
				.WithIdentity("triggerName", "triggerGroup")
				.StartNow()
				.WithSimpleSchedule(x => x
					.WithIntervalInSeconds(10)
					.RepeatForever())
				.Build();



			_sched.ScheduleJob(job, trigger);
		}

		#endregion
		#region Private Fields
		private readonly ISchedulerFactory _schedFact;
		private readonly IScheduler _sched;
		#endregion
	}
}