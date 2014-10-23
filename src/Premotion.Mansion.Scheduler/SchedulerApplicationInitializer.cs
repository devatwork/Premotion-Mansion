using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Scheduler
{
	/// <summary>
	/// This initializer makes sure the scheduler will be started.
	/// </summary>
	public class SchedulerApplicationInitializer : ApplicationInitializer
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SchedulerApplicationInitializer()
			: base(100)
		{
		}
		#endregion
		#region Overrides of ApplicationInitializer
		/// <summary>
		/// Initializes the application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoInitialize(IMansionContext context)
		{
			var scheduler = context.Nucleus.ResolveSingle<QuartzSchedulerService>();
			scheduler.ScheduleJob<ExampleJob>("jobName", "jobGroup", "triggerName", "triggerGroup");
		}
		#endregion
	}
}