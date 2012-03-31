using System;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Core.ScriptTags.Rendering
{
	/// <summary>
	/// Renders a template section.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderSection")]
	public class RenderSectionTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="templateService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RenderSectionTag(ITemplateService templateService)
		{
			// validate arguments
			if (templateService == null)
				throw new ArgumentNullException("templateService");

			// set values
			this.templateService = templateService;
		}
		#endregion
		#region Override of ScriptTag
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the attribute values
			var sectionName = GetRequiredAttribute<string>(context, "name");
			var targetField = GetAttribute<string>(context, "targetField");

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
		#endregion
		#region Private Fields
		private readonly ITemplateService templateService;
		#endregion
	}
}