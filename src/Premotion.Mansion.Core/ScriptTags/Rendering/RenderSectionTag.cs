using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Core.ScriptTags.Rendering
{
	/// <summary>
	/// Renders a template section.
	/// </summary>
	[Named(Constants.NamespaceUri, "renderSection")]
	public class RenderSectionTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the attribute values
			var sectionName = GetRequiredAttribute<string>(context, "name");
			var targetField = GetAttribute<string>(context, "targetField");

			// get the services
			var templateService = context.Nucleus.Get<ITemplateService>(context);

			// render the section
			if (string.IsNullOrWhiteSpace(targetField))
			{
				using (templateService.Render(context, sectionName))
					ExecuteChildTags(context);
			}
			else
			{
				using (templateService.Render(context, sectionName, targetField))
					ExecuteChildTags(context);
			}
		}
	}
}