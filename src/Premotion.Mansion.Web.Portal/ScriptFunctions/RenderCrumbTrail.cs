using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.ScriptFunctions
{
	/// <summary>
	/// Renders a navigation tree.
	/// </summary>
	[ScriptFunction("RenderCrumbTrail")]
	public class RenderCrumbTrail : FunctionExpression
	{
		#region Nested type: Crumb
		/// <summary>
		/// Represents a single crumb.
		/// </summary>
		private class Crumb
		{
			#region Constructors
			/// <summary>
			/// Constructs a text crumb.
			/// </summary>
			/// <param name="name"></param>
			public Crumb(string name)
			{
				// validate arugments
				if (string.IsNullOrEmpty(name))
					throw new ArgumentNullException("name");

				// set values
				Name = name;
			}
			/// <summary>
			/// Constructs a linked crumb.
			/// </summary>
			/// <param name="name"></param>
			/// <param name="node"></param>
			public Crumb(string name, Node node)
			{
				// validate arugments
				if (string.IsNullOrEmpty(name))
					throw new ArgumentNullException("name");
				if (node == null)
					throw new ArgumentNullException("node");

				// set values
				Name = name;
				Node = node;
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets the name of this crumb.
			/// </summary>
			public string Name { get; private set; }
			/// <summary>
			/// Gets the <see cref="Node"/> of this crumb, might be null.
			/// </summary>
			public Node Node { get; private set; }
			/// <summary>
			/// Gets a flag indicating whether this crumb has a <see cref="Node"/>.
			/// </summary>
			public bool IsLinked
			{
				get { return Node != null; }
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="templateService"></param>
		/// <param name="conversionService"> </param>
		/// <exception cref="ArgumentNullException"></exception>
		public RenderCrumbTrail(ITemplateService templateService, IConversionService conversionService)
		{
			//  validate arguments
			if (templateService == null)
				throw new ArgumentNullException("templateService");
			if (conversionService == null)
				throw new ArgumentNullException("conversionService");

			// set values
			this.templateService = templateService;
			this.conversionService = conversionService;
		}
		#endregion
		#region Overrides Of FunctionExpression
		/// <summary>
		/// Renders a navigation tree.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the HTML of the navigation tree.</returns>
		public string Evaluate(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// assemble the crumb trail
			var trail = AssembleTrail(context);

			// render the trail
			return RenderTrail(context, trail);
		}
		#endregion
		#region Trail Methods
		/// <summary>
		/// Assembles the crumb trail.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private IEnumerable<Crumb> AssembleTrail(IMansionContext context)
		{
			// retrieve the crumb set
			var crumbSet = RetrieveCrumbSet(context);

			// transform the trail nodeset in a trail of crumbs
			return crumbSet.Rows.Select(row => Transform(context, (Node) row));
		}
		/// <summary>
		/// Retrieves the crumb dasaset.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static Dataset RetrieveCrumbSet(IMansionContext context)
		{
			// retrieve the content and page node
			var contentIndexRootNode = context.Stack.Peek<Node>("ContentIndexRootNode");
			var siteNode = context.Stack.Peek<Node>("SiteNode");
			var contentNode = context.Stack.Peek<Node>("ContentNode");
			var pageNode = context.Stack.Peek<Node>("PageNode");

			// nodeset in which to store the
			var crumbSet = new Dataset();

			// retrieve page node parent nodes
			var pageParentNodeset = context.Repository.Retrieve(context, new PropertyBag
				                                                            {
				                                                            {"childSource", pageNode},
				                                                            {"depth", "any"},
				                                                            {"sort", "depth asc"},
				                                                            });

			// start from the sitenode and take while the page node has not been reached
			foreach (var parentNode in pageParentNodeset.Nodes.SkipWhile(candidate => candidate.Pointer != siteNode.Pointer))
				crumbSet.AddRow(parentNode);

			// if the content node is not equal to the page node, the content node is not under the current site
			if (contentNode.Pointer != pageNode.Pointer)
			{
				// retrieve content node parent nodes
				var contentParentNodeset = context.Repository.Retrieve(context, new PropertyBag
																									 {
			                                                                		{"childSource", contentNode},
			                                                                		{"depth", "any"},
			                                                                		{"sort", "depth asc"},
																									 });

				// start from the content index root node
				foreach (var parentNode in contentParentNodeset.Nodes.SkipWhile(candidate => candidate.Pointer != contentIndexRootNode.Pointer))
					crumbSet.AddRow(parentNode);	
			}

			// finally add the content node itself
			crumbSet.AddRow(contentNode);
			return crumbSet;
		}
		/// <summary>
		/// Transforms the crumb <paramref name="node"/> into a <see cref="Crumb"/>.
		/// </summary>
		/// <param name="context"> </param>
		/// <param name="node"></param>
		/// <returns></returns>
		private Crumb Transform(IMansionContext context, Node node)
		{
			// load the types
			var contentType = conversionService.Convert<ITypeDefinition>(context, "Content");
			var pageType = conversionService.Convert<ITypeDefinition>(context, "Page");
			var nodeType = conversionService.Convert<ITypeDefinition>(context, node.Pointer);

			// if the node inherits either from content or from page, it is linkable, else it is a text only crumb
			if (nodeType.IsAssignable(contentType) || nodeType.IsAssignable(pageType))
				return new Crumb(node.Pointer.Name, node);

			// crumb is not clickable
			return new Crumb(node.Pointer.Name);
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Renders the crumb trail.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="trail"></param>
		/// <returns></returns>
		private string RenderTrail(IMansionContext context, IEnumerable<Crumb> trail)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (trail == null)
				throw new ArgumentNullException("trail");

			// create a dataset from the trail
			var dataset = new Dataset();
			foreach (var crumb in trail)
				dataset.AddRow(PropertyBagAdapterFactory.Adapt(context, crumb));

			// create a buffer in which to store the output
			var templateBuffer = new StringBuilder();
			using (var templateBufferPipe = new StringOutputPipe(templateBuffer))
			using (context.OutputPipeStack.Push(templateBufferPipe))
			using (templateService.Render(context, "CrumbTrail", TemplateServiceConstants.OutputTargetField))
			{
				// create the loop
				var loop = new Loop(dataset);
				using (context.Stack.Push("Loop", loop))
				{
					// render the leafs recursively
					foreach (var crumb in loop.Rows)
					{
						using (context.Stack.Push("CrumbProperties", crumb))
							templateService.Render(context, "CrumbTrailItem").Dispose();
					}
				}
			}

			// return the contents of the buffer
			return templateBuffer.ToString();
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		private readonly ITemplateService templateService;
		#endregion
	}
}