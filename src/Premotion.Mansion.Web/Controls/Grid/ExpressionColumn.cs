using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Grid
{
	/// <summary>
	/// Represents a column which is bound to an expression.
	/// </summary>
	public class ExpressionColumn : Column
	{
		#region Nested type: ExpressionColumnFactoryTag
		/// <summary>
		/// Creates <see cref="PropertyColumn"/>s.
		/// </summary>
		[ScriptTag(Constants.ControlTagNamespaceUri, "expressionColumn")]
		public class ExpressionColumnFactoryTag : ColumnFactoryTag
		{
			#region Constructors
			/// <summary>
			/// 
			/// </summary>
			/// <param name="expressionScriptService"></param>
			/// <exception cref="ArgumentNullException"></exception>
			public ExpressionColumnFactoryTag(IExpressionScriptService expressionScriptService)
			{
				// validate arguments
				if (expressionScriptService == null)
					throw new ArgumentNullException("expressionScriptService");

				//set values
				this.expressionScriptService = expressionScriptService;
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
				return new ExpressionColumn(GetAttributes(context))
				       {
				       	epxression = expressionScriptService.Parse(context, new LiteralResource(GetRequiredAttribute<string>(context, "expression")))
				       };
			}
			#endregion
			#region Private Fields
			private readonly IExpressionScriptService expressionScriptService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="properties"></param>
		private ExpressionColumn(IPropertyBag properties) : base(properties)
		{
		}
		#endregion
		#region Overrides of Column
		/// <summary>
		/// Renders a cell of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="dataset">The <see cref="Dataset"/> rendered in this column.</param>
		/// <param name="row">The being rendered.</param>
		protected override void DoRenderCell(IMansionWebContext context, ITemplateService templateService, Dataset dataset, IPropertyBag row)
		{
			// create the cell properties
			var cellProperties = new PropertyBag
			                     {
			                     	{"value", epxression.Execute<object>(context)}
			                     };

			// render the cell
			using (context.Stack.Push("CellProperties", cellProperties))
				templateService.Render(context, "GridControlExpressionColumnContent").Dispose();
		}
		#endregion
		#region Private Fields
		private IScript epxression;
		#endregion
	}
}