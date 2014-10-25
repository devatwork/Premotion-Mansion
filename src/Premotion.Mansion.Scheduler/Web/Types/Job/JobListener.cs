using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;
using Premotion.Mansion.Core.Types;

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
				ScheduleTask(context, record);
			base.DoAfterCreate(context, record, properties);
		}



		/// <summary>
		/// Remove the job from the scheduler
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		protected override void DoBeforeDelete(IMansionContext context, Record record)
		{
			DeleteTask(context, record);
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
				DeleteTask(context, record);
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
				ScheduleTask(context, record);
			base.DoAfterUpdate(context, record, properties);
		}



		/// <summary>
		/// Add the job tasks to the scheduler
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		private static void ScheduleTask(IMansionContext context, Record record)
		{
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();
			var schedulerService = context.Nucleus.ResolveSingle<QuartzSchedulerService>();

			var type = typeService.Load(context, record.Type);
			var tasks =
				type.GetDescriptors<RegisterTaskDescriptor>()
					.Select(descriptor => descriptor.TaskType);

			foreach (var task in tasks)
				schedulerService.ScheduleTask(context, task, record as Node);
		}



		/// <summary>
		/// Remove the job tasks from the scheduler
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		private static void DeleteTask(IMansionContext context, Record record)
		{
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();
			var schedulerService = context.Nucleus.ResolveSingle<QuartzSchedulerService>();

			var type = typeService.Load(context, record.Type);
			var tasks =
				type.GetDescriptors<RegisterTaskDescriptor>()
					.Select(descriptor => descriptor.TaskType);

			foreach (var task in tasks)
				schedulerService.DeleteTask(context, task, record as Node);
		}
	}
}
