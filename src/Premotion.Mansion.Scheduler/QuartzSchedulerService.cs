using Quartz;
using Quartz.Impl;

namespace Premotion.Mansion.Scheduler
{
	public class QuartzSchedulerService : ISchedulerService
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
		public void ScheduleJob<T>(string jobName, string jobGroup, string triggerName, string triggerGroup)
			where T : IJob
		{
			var jobKey = new JobKey(jobName, jobGroup);
			var job = JobBuilder.Create<T>()
				.WithIdentity(jobKey)
				.StoreDurably()
				.Build();

			var trigger = TriggerBuilder.Create()
				.WithIdentity(triggerName, triggerGroup)
				.StartNow()
				.WithSimpleSchedule(x => x
					.WithIntervalInSeconds(40)
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



	public class ExampleJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			
		}
	}
}