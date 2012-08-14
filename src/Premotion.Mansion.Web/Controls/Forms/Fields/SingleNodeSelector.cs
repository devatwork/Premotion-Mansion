using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Allows the user to select a single <see cref="Node"/>.
	/// </summary>
	public class SingleNodeSelector : NodeSelectorBase<SingleNodeSelector, string>
	{
		#region Nested type: SingleNodeSelectorFactoryTag
		/// <summary>
		/// Constructs <see cref="SingleNodeSelector"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "singleNodeSelector")]
		public class SingleNodeSelectorFactoryTag : NodeSelectorBaseFactoryTag
		{
			#region Overrides of NodeSelectorBaseFactoryTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override SingleNodeSelector DoCreate(IMansionWebContext context, ControlDefinition definition)
			{
				return new SingleNodeSelector(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructors this control.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public SingleNodeSelector(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of Field{string}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			//  first allow base to initialize
			base.DoInitialize(context, form);

			// if there is no value do not display it
			if (!HasValue(context))
				return;
			var currentValue = GetValue(context);
			if (string.IsNullOrEmpty(currentValue))
				return;

			// retrieve the corresponding node
			var repository = context.Repository;
			var node = repository.RetrieveSingleNode(context, new PropertyBag
			                                                  {
			                                                  	{ValueProperty, currentValue}
			                                                  });

			// if no node was found clear the value of this field
			if (node == null)
			{
				ClearValue();
				return;
			}

			// set the display value
			Definition.Properties.Set("displayValue", node.Get(context, LabelProperty, node.Get<string>(context, ValueProperty)));
		}
		#endregion
	}
}