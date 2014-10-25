using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Types;

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
		/// Initializes the scheduler.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoInitialize(IMansionContext context)
		{
			using (RepositoryUtil.Open(context))
			{
				var repository = context.Repository;
				var jobNodeset = repository.RetrieveNodeset(context, new PropertyBag
				{
					{"parentSource", repository.RetrieveRootNode(context)},
					{"baseType", "Job"},
					{"depth", "any"},
					{"bypassAuthorization", true}
				});

				var typeService = context.Nucleus.ResolveSingle<ITypeService>();
				var schedulerService = context.Nucleus.ResolveSingle<QuartzSchedulerService>();

				foreach (var jobNode in jobNodeset.Nodes)
				{
					var type = typeService.Load(context, jobNode.Type);
					var tasks =
						type.GetDescriptors<RegisterTaskDescriptor>()
							.Select(descriptor => descriptor.TaskType);

					foreach (var task in tasks)
					{
						schedulerService.ScheduleTask(context, task, jobNode);
					}
				}
			}
		}
		#endregion
	}
}