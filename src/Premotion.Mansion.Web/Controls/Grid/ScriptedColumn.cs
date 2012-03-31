using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents a grid column which displays content by rendering the inner content of the tag.
	/// </summary>
	public class ScriptedColumn : Column
	{
		#region Nested type: ScriptedColumnFactoryTag
		/// <summary>
		/// Constructs <see cref="ScriptedColumn"/>s.
		/// </summary>
		[ScriptTag(Constants.ControlTagNamespaceUri, "scriptedColumn")]
		public class ScriptedColumnFactoryTag : ColumnFactoryTag
		{
			#region Nested type: ScriptedColumnContentTag
			/// <summary>
			/// Defines the content of the scripted column.
			/// </summary>
			[ScriptTag(Constants.ControlTagNamespaceUri, "scriptedColumnContent")]
			public class ScriptedColumnContentTag : AlternativeScriptTag
			{
			}
			#endregion
			#region Overrides of ColumnFactoryTag
			/// <summary>
			/// Create a <see cref="Column"/> instance.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created <see cref="Column"/>.</returns>
			protected override Column Create(IMansionWebContext context)
			{
				// get the tag
				var tag = GetAlternativeChildTag<ScriptedColumnContentTag>();

				// create the column))
				return new ScriptedColumn(GetAttributes(context), tag);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="properties">The properties of this column.</param>
		/// <param name="script">The <see cref="IScript"/> expression which to evaluate.</param>
		private ScriptedColumn(IPropertyBag properties, IScript script) : base(properties)
		{
			// validate arguments
			if (script == null)
				throw new ArgumentNullException("script");

			// set values
			this.script = script;
		}
		#endregion
		#region Overrides of Column
		/// <summary>
		/// Renders a cell of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="data">The <see cref="Dataset"/> rendered in this column.</param>
		/// <param name="row">The being rendered.</param>
		protected override void DoRenderCell(IMansionWebContext context, ITemplateService templateService, Dataset data, IPropertyBag row)
		{
			// render the cell
			using (context.Stack.Push("CellProperties", Properties, false))
				script.Execute(context);
		}
		#endregion
		#region Private Fields
		private readonly IScript script;
		#endregion
	}
}