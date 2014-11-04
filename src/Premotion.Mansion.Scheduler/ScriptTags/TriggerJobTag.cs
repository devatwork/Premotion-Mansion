using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Scheduler.ScriptTags
{
	/// <summary>
	/// Triggers the given job node immediately, disregarding the trigger interval.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "triggerJob")]
	public class TriggerJobTag : ScriptTag
	{
		protected override void DoExecute(IMansionContext context)
		{
			var jobNode = GetRequiredAttribute<Node>(context, "source");
			var schedulerService = context.Nucleus.ResolveSingle<QuartzSchedulerService>();
			schedulerService.TriggerJob(context, jobNode);
		}
	}
}