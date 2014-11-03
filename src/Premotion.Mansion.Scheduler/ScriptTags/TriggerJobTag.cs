using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Scheduler.ScriptTags
{
	/// <summary>
	/// Triggers all tasks for the given job node immediately, disregarding the trigger interval.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "triggerJob")]
	public class TriggerJobTag : ScriptTag
	{
		protected override void DoExecute(IMansionContext context)
		{
			var jobNode = GetRequiredAttribute<Node>(context, "source");
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();
			var type = typeService.Load(context, jobNode.Type);
			var descriptors = type.GetDescriptors<RegisterTaskDescriptor>().Select(descriptor => descriptor);

			foreach (var descriptor in descriptors)
			{
				var schedulerService = context.Nucleus.ResolveSingle<QuartzSchedulerService>();
				schedulerService.TriggerTask(context, descriptor.TaskType, jobNode);
			}
		}
	}
}