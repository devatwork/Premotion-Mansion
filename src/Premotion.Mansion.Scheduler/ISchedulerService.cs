using Quartz;

namespace Premotion.Mansion.Scheduler
{
	public interface ISchedulerService
	{
		void ScheduleJob<T>(string jobName, string jobGroup, string triggerName, string triggerGroup)
			where T : IJob;
	}
}