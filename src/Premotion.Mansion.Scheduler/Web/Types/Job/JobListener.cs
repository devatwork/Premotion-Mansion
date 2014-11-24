using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;

namespace Premotion.Mansion.Scheduler.Web.Types.Job
{
	internal class JobListener : NodeListener
	{
		/// <summary>
		/// Add the job to the scheduler
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		/// <param name="properties"></param>
		protected override void DoAfterCreate(IMansionContext context, Record record, IPropertyBag properties)
		{
			if (!properties.Contains("_scheduleStatusUpdate"))
				ScheduleJob(context, record as Node);
			base.DoAfterCreate(context, record, properties);
		}



		/// <summary>
		/// Remove the job from the scheduler
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		protected override void DoBeforeDelete(IMansionContext context, Record record)
		{
			DeleteJob(context, record as Node);
			base.DoBeforeDelete(context, record);
		}



		/// <summary>
		/// Remove the job from the scheduler
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		/// <param name="properties"></param>
		protected override void DoBeforeUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			if (!properties.Contains("_scheduleStatusUpdate"))
				DeleteJob(context, record as Node);
			base.DoBeforeUpdate(context, record, properties);
		}



		/// <summary>
		/// Add the job to the scheduler
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		/// <param name="properties"></param>
		protected override void DoAfterUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			if (!properties.Contains("_scheduleStatusUpdate"))
				ScheduleJob(context, record as Node);
			base.DoAfterUpdate(context, record, properties);
		}



		/// <summary>
		/// Add the job tasks to the scheduler
		/// </summary>
		/// <param name="context"></param>
		/// <param name="node"></param>
		private static void ScheduleJob(IMansionContext context, Node node)
		{
			var schedulerService = context.Nucleus.ResolveSingle<ISchedulerService>();
			schedulerService.ScheduleJob(context, node);
		}



		/// <summary>
		/// Remove the job tasks from the scheduler
		/// </summary>
		/// <param name="context"></param>
		/// <param name="node"></param>
		private static void DeleteJob(IMansionContext context, Node node)
		{
			var schedulerService = context.Nucleus.ResolveSingle<ISchedulerService>();
			schedulerService.DeleteJob(context, node);
		}
	}
}
