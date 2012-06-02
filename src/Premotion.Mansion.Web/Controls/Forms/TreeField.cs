using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Web.Controls.Providers;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Base class for all tree fields.
	/// </summary>
	/// <typeparam name="TValue">The type of value contained by this field.s</typeparam>
	public abstract class TreeField<TValue> : Field<TValue>, IDataConsumerControl<TreeProvider, Leaf>
	{
		#region Nested type: TreeFieldFactoryTag
		/// <summary>
		/// Base tag for <see cref="ScriptTag"/>s creating <see cref="Field{TValue}"/>s.
		/// </summary>
		/// <typeparam name="TField">The type of list field created by this factory tag.</typeparam>
		public abstract class TreeFieldFactoryTag<TField> : FormControlFactoryTag<TField> where TField : TreeField<TValue>
		{
			#region Overrides of ControlFactoryTag<NodeSelect>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override TField Create(IMansionWebContext context, ControlDefinition definition)
			{
				// create the controls
				var control = DoCreate(context, definition);

				// return the control
				return control;
			}
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected abstract TField DoCreate(IMansionWebContext context, ControlDefinition definition);
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected TreeField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Implementation of IDataConsumerControl<in TreeProvider,Leaf>
		/// <summary>
		/// Sets the <paramref name="dataProvider"/> for this control.
		/// </summary>
		/// <param name="dataProvider">The <see cref="DataProvider{TDataType}"/> which to set.</param>
		public void SetDataProvider(TreeProvider dataProvider)
		{
			// validate arguments
			if (dataProvider == null)
				throw new ArgumentNullException("dataProvider");
			if (provider != null)
				throw new InvalidOperationException("The data provider has already been set, cant override it.");

			// set value
			provider = dataProvider;
		}
		#endregion
		#region Overrides of Container
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			// check if no provider was set
			if (provider == null)
				throw new InvalidOperationException("No data provider was set.");

			// set the field value
			Definition.Properties.Set("Value", GetValue(context));

			using (context.Stack.Push("FieldControl", PropertyBagAdapterFactory.Adapt<Field>(context, this)))
			using (templateService.Render(context, "FieldContainer"))
			using (templateService.Render(context, GetType().Name + "Control"))
			{
				// get the root leaf
				var root = provider.Retrieve(context);
				if (root == null)
					throw new InvalidOperationException("Must have a root node to render this tree field");

				// render the leaf
				RenderRoot(context, templateService, root);
			}
		}
		/// <summary>
		/// Renders the root leaf.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="root">The <see cref="Leaf"/> which to render.</param>
		private void RenderRoot(IMansionWebContext context, ITemplateService templateService, Leaf root)
		{
			RenderLeaf(context, templateService, root);
		}
		/// <summary>
		/// Renders a leaf.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		/// <param name="root">The <see cref="Leaf"/> which to render.</param>
		private void RenderLeaf(IMansionWebContext context, ITemplateService templateService, Leaf root)
		{
			// push leaf properties to the stack
			using (context.Stack.Push("LeafProperties", root.Properties))
			using (templateService.Render(context, GetType().Name + "ControlLeaf"))
			{
				// if the leaf does not have any children we are done
				if (!root.HasChildren)
					return;

				// loop through all the children and render them
				using (templateService.Render(context, GetType().Name + "ControlWithChildren"))
				{
					foreach (var leaf in root.Children)
						RenderLeaf(context, templateService, leaf);
				}
			}
		}
		#endregion
		#region Private Fields
		private TreeProvider provider;
		#endregion
	}
}