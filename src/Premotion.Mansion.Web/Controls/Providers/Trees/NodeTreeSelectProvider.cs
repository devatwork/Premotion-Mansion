using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Controls.Providers.Trees
{
	/// <summary>
	/// Provides a tree from a nodes query.
	/// </summary>
	public class NodeTreeSelectProvider : TreeProvider
	{
		#region Nested type: NodeTreeSelectProviderFactoryTag
		/// <summary>
		/// Constructs node tree providers.
		/// </summary>
		[Named(Constants.DataProviderTagNamespaceUri, "nodeTreeSelectProvider")]
		public class NodeTreeSelectProviderFactoryTag : DataProviderFactoryTag<NodeTreeSelectProvider>
		{
			#region Overrides of DataProviderFactoryTag<NodeTreeProvider>
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override NodeTreeSelectProvider Create(MansionWebContext context)
			{
				// get the values
				var properties = new PropertyBag(GetRequiredAttribute<IPropertyBag>(context, "source"));
				string valueProperty;
				if (!properties.TryGetAndRemove(context, "valueProperty", out valueProperty) || string.IsNullOrEmpty(valueProperty))
					valueProperty = "guid";
				string labelProperty;
				if (!properties.TryGetAndRemove(context, "labelProperty", out labelProperty) || string.IsNullOrEmpty(labelProperty))
					labelProperty = "name";
				string selectableExpressionString;
				IScript selectableExpression = null;
				if (properties.TryGetAndRemove(context, "selectableExpression", out selectableExpressionString) && !string.IsNullOrEmpty(selectableExpressionString))
					selectableExpression = context.Nucleus.Get<IScriptingService<IExpressionScript>>(context).Parse(context, new LiteralResource(selectableExpressionString));
				string notSelectableExpressionString;
				IScript notSelectableExpression = null;
				if (properties.TryGetAndRemove(context, "notSelectableExpression", out notSelectableExpressionString) && !string.IsNullOrEmpty(notSelectableExpressionString))
					notSelectableExpression = context.Nucleus.Get<IScriptingService<IExpressionScript>>(context).Parse(context, new LiteralResource(notSelectableExpressionString));

				// ignore some properties
				string tmp;
				properties.TryGetAndRemove(context, "value", out tmp);
				properties.TryGetAndRemove(context, "controlId", out tmp);

				// create the provider
				var provider = new NodeTreeSelectProvider
				               {
				               	ValueProperty = valueProperty,
				               	LabelProperty = labelProperty,
				               	SelectableExpression = selectableExpression,
				               	NotSelectableExpression = notSelectableExpression
				               };
				provider.QueryParameters.Merge(properties);
				return provider;
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a node tree provider.
		/// </summary>
		private NodeTreeSelectProvider()
		{
		}
		#endregion
		#region Overrides of DataProvider<Leaf>
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the retrieve data.</returns>
		protected override Leaf DoRetrieve(MansionContext context)
		{
			// get the current repository
			var repository = context.Repository;

			// retrieve the root node
			var rootnode = repository.RetrieveSingle(context, repository.ParseQuery(context, QueryParameters));
			return new Leaf(ExtractProperties(context, rootnode), RetrieveChildren(context, repository, rootnode));
		}
		/// <summary>
		/// Retrieves the children of the node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="repository">The <see cref="IRepository"/> from which to retrieve the children.</param>
		/// <param name="parentNode">The parent <see cref="Node"/>.</param>
		/// <returns>Returns the child <see cref="Leaf"/>s.</returns>
		private IEnumerable<Leaf> RetrieveChildren(MansionContext context, IRepository repository, Node parentNode)
		{
			// retrieve it's direct children
			var childNodeset = repository.Retrieve(context, repository.ParseQuery(context, new PropertyBag
			                                                                               {
			                                                                               	{"parentPointer", parentNode.Pointer}
			                                                                               }));

			// loop through all the nodes and create leafs from them
			return childNodeset.Nodes.Select(node => new Leaf(ExtractProperties(context, node), RetrieveChildren(context, repository, node)));
		}
		/// <summary>
		/// Extracts the tree attributes from the node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> from which to extract the data.</param>
		/// <returns>Returns the extracted data.</returns>
		private IPropertyBag ExtractProperties(MansionContext context, Node node)
		{
			var disabled = false;
			using (context.Stack.Push("Row", node))
			{
				if (SelectableExpression != null)
					disabled |= !SelectableExpression.Execute<bool>(context);
				if (NotSelectableExpression != null)
					disabled |= NotSelectableExpression.Execute<bool>(context);
			}

			// assemble the properties
			var properties = new PropertyBag(node)
			                 {
			                 	{"value", node.Get(context, ValueProperty, string.Empty)},
			                 	{"label", node.Get(context, LabelProperty, string.Empty)},
			                 	{"disabled", disabled}
			                 };

			return properties;
		}
		#endregion
		#region Properties
		private IPropertyBag QueryParameters
		{
			get { return queryParameters; }
		}
		private string ValueProperty { get; set; }
		private string LabelProperty { get; set; }
		private IScript SelectableExpression { get; set; }
		private IScript NotSelectableExpression { get; set; }
		#endregion
		#region Private Fields
		private readonly IPropertyBag queryParameters = new PropertyBag();
		#endregion
	}
}