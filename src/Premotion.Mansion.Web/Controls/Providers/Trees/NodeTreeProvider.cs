using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Providers.Trees
{
	/// <summary>
	/// Provides a tree from a nodes query.
	/// </summary>
	public class NodeTreeProvider : TreeProvider
	{
		#region Nested type: NodeTreeProviderFactoryTag
		/// <summary>
		/// Constructs node tree providers.
		/// </summary>
		[ScriptTag(Constants.DataProviderTagNamespaceUri, "nodeTreeProvider")]
		public class NodeTreeProviderFactoryTag : DataProviderFactoryTag<NodeTreeProvider>
		{
			#region Constructors
			/// <summary>
			/// 
			/// </summary>
			/// <param name="expressionScriptService"></param>
			/// <exception cref="ArgumentNullException"></exception>
			public NodeTreeProviderFactoryTag(IExpressionScriptService expressionScriptService)
			{
				// validate arguments
				if (expressionScriptService == null)
					throw new ArgumentNullException("expressionScriptService");

				// set values
				this.expressionScriptService = expressionScriptService;
			}
			#endregion
			#region Overrides of DataProviderFactoryTag<NodeTreeProvider>
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override NodeTreeProvider Create(IMansionWebContext context)
			{
				// get the values
				var rootPointer = GetRequiredAttribute<NodePointer>(context, "rootPointer");
				var valueProperty = GetRequiredAttribute<string>(context, "valueProperty");
				var labelProperty = GetRequiredAttribute<string>(context, "labelProperty");

				// create the provider
				var provider = new NodeTreeProvider(rootPointer, valueProperty, labelProperty);

				// check if there is a disabled expression
				var disabledExpressionString = GetAttribute<string>(context, "disabledExpression");
				if (!string.IsNullOrEmpty(disabledExpressionString))
				{
					// parse into an expression
					provider.DisabledExpression = expressionScriptService.Parse(context, new LiteralResource(disabledExpressionString));
				}

				// return the created provider
				return provider;
			}
			#endregion
			#region Private Fields
			private readonly IExpressionScriptService expressionScriptService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a node tree provider.
		/// </summary>
		/// <param name="rootPointer">The rootpointer from which the tree is started.</param>
		/// <param name="valueProperty">The name of the property specifying the value.</param>
		/// <param name="labelProperty">The name of the property specifying the label.</param>
		private NodeTreeProvider(NodePointer rootPointer, string valueProperty, string labelProperty)
		{
			// validate arguments
			if (rootPointer == null)
				throw new ArgumentNullException("rootPointer");
			if (string.IsNullOrEmpty(valueProperty))
				throw new ArgumentNullException("valueProperty");
			if (string.IsNullOrEmpty(labelProperty))
				throw new ArgumentNullException("labelProperty");

			// set values
			this.rootPointer = rootPointer;
			this.valueProperty = valueProperty;
			this.labelProperty = labelProperty;
		}
		#endregion
		#region Overrides of DataProvider<Leaf>
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the retrieve data.</returns>
		protected override Leaf DoRetrieve(IMansionContext context)
		{
			// get the current repository
			var repository = context.Repository;

			// retrieve the root node
			var rootnode = repository.RetrieveSingleNode(context, repository.ParseQuery(context, new PropertyBag
			                                                                                 {
			                                                                                 	{"pointer", rootPointer}
			                                                                                 }));
			return new Leaf(ExtractProperties(context, rootnode), RetrieveChildren(context, repository, rootnode));
		}
		/// <summary>
		/// Retrieves the children of the node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="repository">The <see cref="IRepository"/> from which to retrieve the children.</param>
		/// <param name="parentNode">The parent <see cref="Node"/>.</param>
		/// <returns>Returns the child <see cref="Leaf"/>s.</returns>
		private IEnumerable<Leaf> RetrieveChildren(IMansionContext context, IRepository repository, Node parentNode)
		{
			// retrieve it's direct children
			var childNodeset = repository.RetrieveNodeset(context, repository.ParseQuery(context, new PropertyBag
			                                                                               {
			                                                                               	{"parentPointer", parentNode.Pointer}
			                                                                               }));

			// loop through all the nodes and create leafs from them
			return childNodeset.Nodes.Select(node => new Leaf(ExtractProperties(context, node), RetrieveChildren(context, repository, node)));
		}
		/// <summary>
		/// Extracts the tree attributes from the node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> from which to extract the data.</param>
		/// <returns>Returns the extracted data.</returns>
		private IPropertyBag ExtractProperties(IMansionContext context, Node node)
		{
			// assemble the properties
			var properties = new PropertyBag
			                 {
			                 	{"id", node.Pointer.Id},
			                 	{"name", node.Pointer.Name},
			                 	{"type", node.Pointer.Type},
			                 	{"pointer", node.Pointer},
			                 	{"value", node.Get(context, valueProperty, string.Empty)},
			                 	{"label", node.Get(context, labelProperty, string.Empty)}
			                 };

			// check for disabled expression
			if (DisabledExpression != null)
			{
				using (context.Stack.Push("Node", node, false))
					properties.Set("disabled", DisabledExpression.Execute<bool>(context));
			}

			return properties;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="IExpressionScript"/> to determine the disabled value.
		/// </summary>
		protected IExpressionScript DisabledExpression { private get; set; }
		#endregion
		#region Private Fields
		private readonly string labelProperty;
		private readonly NodePointer rootPointer;
		private readonly string valueProperty;
		#endregion
	}
}