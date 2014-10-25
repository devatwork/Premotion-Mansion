using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
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
			// open the repository
			using (RepositoryUtil.Open(context))
			{
				var repository = context.Repository;
				var taskNodeset = repository.RetrieveNodeset(context, new PropertyBag
				{
					{"parentSource", repository.RetrieveRootNode(context)},
					{"baseType", "Task"},
					{"depth", "any"},
					{"bypassAuthorization", true}
				});

				var scheduler = context.Nucleus.ResolveSingle<QuartzSchedulerService>();

				scheduler.ScheduleJobs(context);
			}
		}
		#endregion
	}
}