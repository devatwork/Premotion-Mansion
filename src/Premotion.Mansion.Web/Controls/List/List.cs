using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.List
{
	/// <summary>
	/// Implements the list control.
	/// </summary>
	public abstract class List : DataboundControl<Dataset>
	{
		#region Nested type: EmbeddedList
		/// <summary>
		/// Represents an embedded list.
		/// </summary>
		private class EmbeddedList : List
		{
			#region Constructors
			/// <summary>
			/// Constructs a control with the specified ID.
			/// </summary>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			/// <param name="rowContentExpression"></param>
			public EmbeddedList(ControlDefinition definition, IScript rowContentExpression) : base(definition, rowContentExpression)
			{
			}
			#endregion
		}
		#endregion
		#region Nested type: FormList
		/// <summary>
		/// Represents a list with a form.
		/// </summary>
		private class FormList : List, IFormControl
		{
			#region Constructors
			/// <summary>
			/// Constructs a control with the specified ID.
			/// </summary>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			/// <param name="rowContentExpression"></param>
			public FormList(ControlDefinition definition, IScript rowContentExpression) : base(definition, rowContentExpression)
			{
			}
			#endregion
		}
		#endregion
		#region Nested type: ListFactoryTag
		/// <summary>
		/// Constructs <see cref="List"/>s.
		/// </summary>
		[ScriptTag(Constants.ControlTagNamespaceUri, "list")]
		public class ListFactoryTag : ControlFactoryTag<List>
		{
			#region Constructors
			/// <summary>
			/// 
			/// </summary>
			/// <param name="expressionScriptService"></param>
			/// <exception cref="ArgumentNullException"></exception>
			public ListFactoryTag(IExpressionScriptService expressionScriptService)
			{
				// validate arguments
				if (expressionScriptService == null)
					throw new ArgumentNullException("expressionScriptService");

				// set values
				this.expressionScriptService = expressionScriptService;
			}
			#endregion
			#region Overrides of ControlFactoryTag<List>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override List Create(IMansionWebContext context, ControlDefinition definition)
			{
				// get the expressions
				var rowContentExpression = expressionScriptService.Parse(context, new LiteralResource(GetRequiredAttribute<string>(context, "rowContentExpression")));

				// create the proper control
				IFormControl control;
				return context.TryFindControl(out control) ? new EmbeddedList(definition, rowContentExpression) : (List) new FormList(definition, rowContentExpression);
			}
			#endregion
			#region Private Fields
			private readonly IExpressionScriptService expressionScriptService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		/// <param name="rowContentExpression"></param>
		protected List(ControlDefinition definition, IScript rowContentExpression) : base(definition)
		{
			// validate arguments
			if (rowContentExpression == null)
				throw new ArgumentNullException("rowContentExpression");

			// set values
			this.rowContentExpression = rowContentExpression;
		}
		#endregion
		#region Overrides of Control
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			// retrieve the data
			var data = Retrieve(context);

			// render the control
			using (context.Stack.Push("ListProperties", Definition.Properties))
			using (context.Stack.Push("Dataset", data))
			using (templateService.Render(context, GetType().Name + "Control"))
			{
				if (data.RowCount > 0)
				{
					// create the loop
					var loop = new Loop(data);

					// loop through all the rows
					using (context.Stack.Push("Loop", loop))
					{
						foreach (var row in loop.Rows)
						{
							// push the row to the stack
							using (context.Stack.Push("Row", row))
							using (context.Stack.Push("RowProperties", new PropertyBag
							                                           {
							                                           	{"content", rowContentExpression.Execute<string>(context)}
							                                           }))
								templateService.Render(context, "ListControlRow").Dispose();
						}
					}
				}
				else
				{
					// render the no result message
					templateService.Render(context, "ListControlRowNoResults").Dispose();
				}

				// check for paging
				if (data.IsPaged)
					templateService.Render(context, "ListControlPaging").Dispose();
			}
		}
		#endregion
		#region Private Fields
		private readonly IScript rowContentExpression;
		#endregion
	}
}