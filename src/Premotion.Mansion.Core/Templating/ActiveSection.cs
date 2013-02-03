using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Represents an active section.
	/// </summary>
	public class ActiveSection
	{
		#region Constructors
		/// <summary>
		/// Constructs an active section.
		/// </summary>
		/// <param name="templateService">The <see cref="ITemplateServiceInternal"/>.</param>
		/// <param name="section">The section which is being rendered.</param>
		/// <param name="targetField">The target field.</param>
		public ActiveSection(ITemplateServiceInternal templateService, ISection section, IField targetField)
		{
			// validate arguments
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (section == null)
				throw new ArgumentNullException("section");
			if (targetField == null)
				throw new ArgumentNullException("targetField");

			// set values
			TemplateService = templateService;
			Section = section;
			TargetField = targetField;

			// initialize
			foreach (var placeholder in (from candidate in Section.Expression.Expressions
			                             where candidate is PlaceholderExpression && !((PlaceholderExpression) candidate).Name.StartsWith("@", StringComparison.OrdinalIgnoreCase)
			                             select (PlaceholderExpression) candidate).Where(placeholder => !fields.ContainsKey(placeholder.Name)))
			{
				// create a field
				fields.Add(placeholder.Name, new StringBufferField());
			}
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Finializes the rendering of the active section.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public void FinalizeRendering(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if all the requirements of this section are satisfied
			if (!Section.AreRequirementsSatified(context))
				return;

			// check if this section should be renderend  only once
			if (Section.ShouldBeRenderedOnce)
			{
				// check if the section was already rendered
				var sectionId = Section.Id;
				if (TargetField.RenderedSections.Contains(sectionId))
					return;
				TargetField.RenderedSections.Add(sectionId);
			}

			// create the bag of section properties
			foreach (var placeholder in
				from candidate in Section.Expression.Expressions
				where candidate is PlaceholderExpression
				select (PlaceholderExpression) candidate)
			{
				// check if the placeholder is an auto parse section
				placeholder.Content = placeholder.Name.StartsWith("@", StringComparison.OrdinalIgnoreCase) ? TemplateService.RenderToString(context, placeholder.Name.Substring(1)) : fields[placeholder.Name].Content;
			}

			// pus the field bag to the stack and execute
			TargetField.Append(Section.Expression.Execute<string>(context));
		}
		#endregion
		#region Field Methods
		/// <summary>
		/// Checks whether this section contains the field.
		/// </summary>
		/// <param name="targetField">The name of th field.</param>
		/// <returns>Returns true when this section contains the field, otherwise false.</returns>
		public bool ContainsField(string targetField)
		{
			// validate arguments
			if (string.IsNullOrEmpty(targetField))
				throw new ArgumentNullException("targetField");

			// check if the field exists
			return fields.ContainsKey(targetField);
		}
		/// <summary>
		/// Gets a field by it's name.
		/// </summary>
		/// <param name="targetField">The field name.</param>
		/// <returns>Returns the field.</returns>
		public IField GetField(string targetField)
		{
			// validate arguments
			if (string.IsNullOrEmpty(targetField))
				throw new ArgumentNullException("targetField");

			// check if the field exists
			return fields[targetField];
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the section being rendered.
		/// </summary>
		public ISection Section { get; private set; }
		/// <summary>
		/// Gets the target to which the field is rendered.
		/// </summary>
		public IField TargetField { get; private set; }
		/// <summary>
		/// Gets/Sets the template service.
		/// </summary>
		private ITemplateServiceInternal TemplateService { get; set; }
		#endregion
		#region Private Fields
		private readonly IDictionary<string, IField> fields = new Dictionary<string, IField>(StringComparer.OrdinalIgnoreCase);
		#endregion
	}
}