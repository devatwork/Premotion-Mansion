using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Scheduler
{
	interface ISchedulerService
	{
		void ScheduleJob(IMansionContext context, Node jobNode);
		void DeleteJob(IMansionContext context, Node jobNode);
		void TriggerJob(IMansionContext context, Node jobNode);
	}
}