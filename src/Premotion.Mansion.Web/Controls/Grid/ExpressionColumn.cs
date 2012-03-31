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
	/// Represents a grid column displaying a property.
	/// </summary>
	public class ExpressionColumn : Column
	{
		#region Nested type: ExpressionColumnFactoryTag
		/// <summary>
		/// Constructs <see cref="ExpressionColumn"/>s/
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
				// get the column properties
				var properties = GetAttributes(context);

				// parse into an expression
				var expression = expressionScriptService.Parse(context, new LiteralResource(GetRequiredAttribute<string>(context, "expression")));

				// create the column))
				return new ExpressionColumn(properties, expression);
			}
			#endregion
			#region Private Fields
			private readonly IExpressionScriptService expressionScriptService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="properties">The properties of this column.</param>
		/// <param name="expression">The <see cref="IScript"/> expression which to evaluate.</param>
		private ExpressionColumn(IPropertyBag properties, IScript expression) : base(properties)
		{
			// validate arguments
			if (expression == null)
				throw new ArgumentNullException("expression");

			// set values
			this.expression = expression;
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
			// create the cell properties
			var cellProperties = new PropertyBag
			                     {
			                     	{"value", expression.Execute<object>(context)}
			                     };

			// render the cell
			using (context.Stack.Push("CellProperties", cellProperties, false))
				templateService.Render(context, "GridControlPropertyColumnContent").Dispose();
		}
		#endregion
		#region Private Fields
		private readonly IScript expression;
		#endregion
	}
}