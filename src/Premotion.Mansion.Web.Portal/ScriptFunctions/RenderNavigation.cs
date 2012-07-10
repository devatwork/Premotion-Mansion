using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Portal.ScriptFunctions
{
	/// <summary>
	/// Renders a navigation tree.
	/// </summary>
	[ScriptFunction("renderNavigation")]
	public class RenderNavigation : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService"></param>
		/// <param name="templateService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RenderNavigation(IApplicationResourceService applicationResourceService, ITemplateService templateService)
		{
			//  validate arguments
			if (applicationResourceService == null)
				throw new ArgumentNullException("applicationResourceService");
			if (templateService == null)
				throw new ArgumentNullException("templateService");

			// set values
			this.applicationResourceService = applicationResourceService;
			this.templateService = templateService;
		}
		#endregion
		#region Nested Type: Leaf
		/// <summary>
		/// Represents a leaf within the navigation tree.
		/// </summary>
		private class Leaf
		{
			#region Constructors
			/// <summary>
			/// Private constructor, <see cref="Create"/>.
			/// </summary>
			private Leaf(Node node, Leaf parentLeaf = null, Node targetNode = null)
			{
				this.parentLeaf = parentLeaf;
				this.node = node;
				this.targetNode = targetNode;
			}
			#endregion
			#region Factory Methods
			/// <summary>
			/// Creates a leaf from the <paramref name="navigationNode"/>
			/// </summary>
			/// <param name="context"></param>
			/// <param name="navigationNode"></param>
			/// <param name="parentLeaf"></param>
			/// <returns></returns>
			public static Leaf Create(IMansionContext context, Node navigationNode, Leaf parentLeaf = null)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (navigationNode == null)
					throw new ArgumentNullException("navigationNode");

				// if the node does not have a targetGuid set then it does not have a target node
				Guid targetGuid;
				if (!navigationNode.TryGet(context, "targetGuid", out targetGuid) || targetGuid == Guid.NewGuid())
					return new Leaf(navigationNode, parentLeaf);

				// retrieve the target node
				var repository = context.Repository;
				var targetNodeQuery = repository.ParseQuery(context, new PropertyBag {{"guid", targetGuid}});
				var targetNode = repository.RetrieveSingle(context, targetNodeQuery);

				// return the leaf with target node
				return new Leaf(navigationNode, parentLeaf, targetNode);
			}
			#endregion
			#region Methods
			/// <summary>
			/// Adds the <paramref name="child"/> to this leaf.
			/// </summary>
			/// <param name="child"></param>
			public void Add(Leaf child)
			{
				// validate arguments
				if (child == null)
					throw new ArgumentNullException("child");
				children.Add(child);
			}
			/// <summary>
			/// Sets this leaf as active, which is broadcasted to the parent.
			/// </summary>
			public void SetActive()
			{
				IsActive = true;
				if (parentLeaf != null)
					parentLeaf.SetActive();
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets a flag indicating whether this leaf is active.
			/// </summary>
			public bool IsActive { get; private set; }
			/// <summary>
			/// Gets the <see cref="Node"/> backing this leaf.
			/// </summary>
			public Node Node
			{
				get { return node; }
			}
			/// <summary>
			/// Flag indicating whether this leaf has a <see cref="TargetNode"/>.
			/// </summary>
			public bool HasTarget
			{
				get { return targetNode != null; }
			}
			/// <summary>
			/// Gets the target <see cref="Node"/> of this leaf.
			/// </summary>
			public Node TargetNode
			{
				get { return targetNode; }
			}
			/// <summary>
			/// Flag indicating whether the leaf has children or not.
			/// </summary>
			public bool HasChildren
			{
				get { return children.Count > 0; }
			}
			/// <summary>
			/// Gets the children of this leaf.
			/// </summary>
			public IEnumerable<Leaf> Children
			{
				get { return children; }
			}
			#endregion
			#region Private Fields
			private readonly List<Leaf> children = new List<Leaf>();
			private readonly Node node;
			private readonly Leaf parentLeaf;
			private readonly Node targetNode;
			#endregion
		}
		#endregion
		#region Overrides Of FunctionExpression
		/// <summary>
		/// Renders a navigation tree.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="navigationNode">Root <see cref="Node"/> of the navigation tree.</param>
		/// <returns>Returns the HTML of the navigation tree.</returns>
		public string Evaluate(IMansionContext context, Node navigationNode)
		{
			return Evaluate(context, navigationNode, "nav");
		}
		/// <summary>
		/// Renders a navigation tree.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="navigationNode">Root <see cref="Node"/> of the navigation tree.</param>
		/// <param name="cssClasses">The CSS classes used for this navigation.</param>
		/// <returns>Returns the HTML of the navigation tree.</returns>
		public string Evaluate(IMansionContext context, Node navigationNode, string cssClasses)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (navigationNode == null)
				throw new ArgumentNullException("navigationNode");
			if (string.IsNullOrEmpty(cssClasses))
				throw new ArgumentNullException("cssClasses");

			// get the current URL node
			var urlNode = context.Stack.Peek<Node>("UrlNode");

			// retrieve all the children of the navigation root node
			var navigationItemNodeset = RetrieveNavigationItemNodeset(context, navigationNode);

			// build the tree structure
			var navigationTree = BuildTreeStructure(context, navigationNode, navigationItemNodeset, urlNode.Pointer);

			// render the tree
			using (context.Stack.Push("ControlProperties", new PropertyBag
			                                               {
			                                               	{"cssClasses", cssClasses}
			                                               }))
				return RenderNavigationTree(context, navigationTree);
		}
		#endregion
		#region Retrieve Methods
		/// <summary>
		/// Retrieves the navigation item nodeset.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="navigationNode"></param>
		/// <returns></returns>
		private static Nodeset RetrieveNavigationItemNodeset(IMansionContext context, Node navigationNode)
		{
			var repository = context.Repository;
			var navigationItemQuery = repository.ParseQuery(context, new PropertyBag
			                                                         {
			                                                         	{"parentSource", navigationNode},
			                                                         	{"baseType", "NavigationItem"},
			                                                         	{"depth", "any"},
			                                                         	{"status", "published"}
			                                                         });
			var navigationItemNodeset = repository.Retrieve(context, navigationItemQuery);
			return navigationItemNodeset;
		}
		#endregion
		#region Tree Methods
		/// <summary>
		/// Creates the tree structure from the <paramref name="navigationItemNodeset"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="navigationNode"></param>
		/// <param name="navigationItemNodeset"></param>
		/// <param name="currentPagePointer"></param>
		/// <returns></returns>
		private Leaf BuildTreeStructure(IMansionContext context, Node navigationNode, Nodeset navigationItemNodeset, NodePointer currentPagePointer)
		{
			// first sort the set by depth ASC, than order ASC
			var sortedSet = navigationItemNodeset.Nodes.OrderBy(x => x, new ComparisonComparer<Node>((x, y) =>
			                                                                                         {
			                                                                                         	// first compare on depth
			                                                                                         	var depthComparison = x.Pointer.Depth.CompareTo(y.Pointer.Depth);
			                                                                                         	if (depthComparison != 0)
			                                                                                         		return depthComparison;

			                                                                                         	// next compare on order
			                                                                                         	return x.Order.CompareTo(y.Order);
			                                                                                         }));

			// create the parent leaf
			var rootLeaf = Leaf.Create(context, navigationNode);
			var leafSet = new List<Leaf> {rootLeaf};

			// loop over the sorted set
			foreach (var leafNode in sortedSet)
			{
				// find the parent leaf
				var parentLeaf = leafSet.Single(candidate => candidate.Node.Pointer == leafNode.Pointer.Parent);

				// create the leaf
				var leaf = Leaf.Create(context, leafNode, parentLeaf);

				// add the leaf to the parent leaf
				parentLeaf.Add(leaf);

				// add the leaf to the set
				leafSet.Add(leaf);
			}

			// find the deepest node who has an internal link and is parent of or equal to the current page pointer
			var activeLeaf = ((IEnumerable<Leaf>) leafSet).Reverse().Where(x => x.HasTarget && (currentPagePointer.IsChildOf(x.TargetNode.Pointer) || currentPagePointer == x.TargetNode.Pointer)).FirstOrDefault();
			if (activeLeaf != null)
				activeLeaf.SetActive();

			// return the parent leaf
			return rootLeaf;
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Renders the navigation tree.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="navigationTree"></param>
		/// <returns></returns>
		private string RenderNavigationTree(IMansionContext context, Leaf navigationTree)
		{
			// create a buffer in which to store the output
			var templateBuffer = new StringBuilder();
			using (var templateBufferPipe = new StringOutputPipe(templateBuffer))
			using (context.OutputPipeStack.Push(templateBufferPipe))
				// render the main navigation section
			using (context.Stack.Push("NavigationNode", navigationTree.Node, false))
			using (templateService.Open(context, applicationResourceService.Get(context, applicationResourceService.ParsePath(context, new PropertyBag
			                                                                                                                           {
			                                                                                                                           	{"type", navigationTree.Node.Pointer.Type},
			                                                                                                                           	{"extension", TemplateServiceConstants.DefaultTemplateExtension}
			                                                                                                                           }))))
			using (templateService.Render(context, "NavigationRoot", TemplateServiceConstants.OutputTargetField))
			{
				// render the leafs recursively
				RenderLeafs(context, navigationTree.Children);
			}

			// return the contents of the buffer
			return templateBuffer.ToString();
		}
		/// <summary>
		/// Renders the leaf recursively.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="leafs"></param>
		private void RenderLeafs(IMansionContext context, IEnumerable<Leaf> leafs)
		{
			// loop over all the leafs
			foreach (var leaf in leafs)
			{
				// render the sections
				using (context.Stack.Push("NavigationItemNode", leaf.Node, false))
				using (context.Stack.Push("NavigationItemProperties", PropertyBagAdapterFactory.Adapt(context, leaf), false))
				using (templateService.Open(context, applicationResourceService.Get(context, applicationResourceService.ParsePath(context, new PropertyBag
				                                                                                                                           {
				                                                                                                                           	{"type", leaf.Node.Pointer.Type},
				                                                                                                                           	{"extension", TemplateServiceConstants.DefaultTemplateExtension}
				                                                                                                                           }))))
				using (templateService.Render(context, "NavigationItem"))
				{
					// if the leaf has children of it's own render them
					if (leaf.HasChildren)
					{
						using (templateService.Render(context, "SubNavigationItem"))
						{
							// render the leafs recursively
							RenderLeafs(context, leaf.Children);
						}
					}
				}
			}
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		private readonly ITemplateService templateService;
		#endregion
	}
}