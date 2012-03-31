using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Web.Controls.Providers;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Base class for all list fields.
	/// </summary>
	/// <typeparam name="TValue">The type of value contained by this field.s</typeparam>
	public abstract class ListField<TValue> : Field<TValue>, IDataConsumerControl<DataProvider<Dataset>, Dataset>
	{
		#region Field Factory Tag
		/// <summary>
		/// Base tag for <see cref="ScriptTag"/>s creating <see cref="Field{TValue}"/>s.
		/// </summary>
		/// <typeparam name="TField">The type of list field created by this factory tag.</typeparam>
		public abstract class ListFieldFactoryTag<TField> : FormControlFactoryTag<TField> where TField : ListField<TValue>
		{
			#region Overrides of FormControlFactoryTag{TControl}
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override TField Create(IMansionWebContext context, ControlDefinition definition)
			{
				// determine the mapping strategy
				var mappingStrategy = GetMappingStrategy(context);

				// create the list control
				return Create(context, definition, mappingStrategy);
			}
			/// <summary>
			/// Determines the <see cref="RowMappingStrategy"/> used by this list field.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the mapping strategy.</returns>
			private RowMappingStrategy GetMappingStrategy(IMansionWebContext context)
			{
				var valueProperty = GetAttribute(context, "valueProperty", string.Empty);
				var labelProperty = GetAttribute(context, "labelProperty", string.Empty);
				if (!string.IsNullOrEmpty(valueProperty) && ! string.IsNullOrEmpty(labelProperty))
					return new PropertyRowMappingStrategy(valueProperty, labelProperty);

				// return default mapping strategy
				return DefaultRowMappingStrategy.Instance;
			}
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			/// <param name="mappingStrategy">The <see cref="RowMappingStrategy"/> used by the list field.</param>
			protected abstract TField Create(IMansionWebContext context, ControlDefinition definition, RowMappingStrategy mappingStrategy);
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		/// <param name="mappingStrategy">The <see cref="RowMappingStrategy"/> used by this list.</param>
		protected ListField(ControlDefinition definition, RowMappingStrategy mappingStrategy) : base(definition)
		{
			// validate arguments
			if (mappingStrategy == null)
				throw new ArgumentNullException("mappingStrategy");

			// set values
			this.mappingStrategy = mappingStrategy;
		}
		#endregion
		#region Data Methods
		/// <summary>
		/// Sets the <paramref name="dataProvider"/> for this control.
		/// </summary>
		/// <param name="dataProvider">The <see cref="DataProvider{TDataType}"/> which to set.</param>
		public void SetDataProvider(DataProvider<Dataset> dataProvider)
		{
			// validate arguments
			if (dataProvider == null)
				throw new ArgumentNullException("dataProvider");
			if (provider != null)
				throw new InvalidOperationException("The data provider has already been set, cant override it.");

			// set value
			provider = dataProvider;
		}
		/// <summary>
		/// Gets the data bound to this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the data.</returns>
		private Dataset Retrieve(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if no provider was set
			if (provider == null)
				throw new InvalidOperationException("No data provider was set.");

			// return the data from the provider
			return provider.Retrieve(context);
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
			using (templateService.Render(context, GetType().Name + "Control"))
			{
				// get the data
				var dataset = Retrieve(context);

				//  create the loop and psuh  it to the stack
				var loop = new Loop(dataset);
				using (context.Stack.Push("Loop", loop, false))
				{
					foreach (var row in loop.Rows)
					{
						using (context.Stack.Push("OptionProperties", mappingStrategy.Map(context, row), false))
							templateService.Render(context, GetType().Name + "ControlOption").Dispose();
					}
				}
			}
		}
		#endregion
		#region Private Fields
		private readonly RowMappingStrategy mappingStrategy;
		private DataProvider<Dataset> provider;
		#endregion
	}
}