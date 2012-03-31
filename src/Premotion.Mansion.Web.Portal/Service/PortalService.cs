﻿using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Descriptors;

namespace Premotion.Mansion.Web.Portal.Service
{
	/// <summary>
	/// Provides the default implementation of <see cref="IPortalService"/>.
	/// </summary>
	public class PortalService : IPortalService
	{
		#region Nested Class: TemplatePageMapCachedObject
		/// <summary>
		/// Contains the template page map for a site.
		/// </summary>
		private class TemplatePageMapCachedObject : CachedObject<Dictionary<NodePointer, Node>>, IDependableCachedObject
		{
			#region Constructors
			/// <summary>
			/// </summary>
			/// <param name="obj"></param>
			public TemplatePageMapCachedObject(Dictionary<NodePointer, Node> obj) : base(obj)
			{
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cachingService"></param>
		/// <param name="templateService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public PortalService(ICachingService cachingService, ITemplateService templateService)
		{
			// validaet arguments
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");
			if (templateService == null)
				throw new ArgumentNullException("templateService");

			// set values
			this.cachingService = cachingService;
			this.templateService = templateService;
		}
		#endregion
		#region Template Page Methods
		/// <summary>
		/// Resolves the template page for an particual content node..
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="siteNode">The site <see cref="Node"/>.</param>
		/// <param name="contentNode">The content <see cref="Node"/>.</param>
		/// <param name="templatePageNode">When found, the template page <see cref="Node"/>.</param>
		/// <returns>Returns true when the template page could be resolved, otherwise false.</returns>
		public bool TryResolveTemplatePage(IMansionContext context, Node siteNode, Node contentNode, out Node templatePageNode)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (siteNode == null)
				throw new ArgumentNullException("siteNode");
			if (contentNode == null)
				throw new ArgumentNullException("contentNode");

			// generate the cache key
			var cacheKey = string.Format("{0}TemplatePageMap_{1}", cacheKeyPrefix, siteNode.Pointer.Id);

			// get the template page map for this site
			var templatePageMap = cachingService.GetOrAdd(context, (StringCacheKey) cacheKey, () =>
			                                                                                  {
			                                                                                  	// retrieve all the template pages
			                                                                                  	var repository = context.Repository;
			                                                                                  	var query = repository.ParseQuery(context, new PropertyBag
			                                                                                  	                                           {
			                                                                                  	                                           	{"parentSource", siteNode},
			                                                                                  	                                           	{"depth", "any"},
			                                                                                  	                                           	{"baseType", "TemplatePage"}
			                                                                                  	                                           });
			                                                                                  	var nodeset = repository.Retrieve(context, query);

			                                                                                  	// create the map of all the pages
			                                                                                  	var map = nodeset.Nodes.ToDictionary(node =>
			                                                                                  	                                     {
			                                                                                  	                                     	// get the content source guid
			                                                                                  	                                     	var contentSourceGuid = node.Get<Guid>(context, "contentSourceGuid");

			                                                                                  	                                     	// retrieve the intended node
			                                                                                  	                                     	var contentSourceNodeQuery = repository.ParseQuery(context, new PropertyBag
			                                                                                  	                                     	                                                            {
			                                                                                  	                                     	                                                            	{"guid", contentSourceGuid}
			                                                                                  	                                     	                                                            });
			                                                                                  	                                     	var contentSourceNode = repository.RetrieveSingle(context, contentSourceNodeQuery);

			                                                                                  	                                     	// return the pointer of that node
			                                                                                  	                                     	return contentSourceNode.Pointer;
			                                                                                  	                                     });

			                                                                                  	// return the cached object
			                                                                                  	return new TemplatePageMapCachedObject(map);
			                                                                                  });

			// loop over the pointer
			foreach (var pointer in contentNode.Pointer.HierarchyReverse)
			{
				if (templatePageMap.TryGetValue(pointer, out templatePageNode))
					return true;
			}

			// no template page found
			templatePageNode = null;
			return false;
		}
		#endregion
		#region Column Methods
		/// <summary>
		/// Renders a column with the specified <paramref name="columnName"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="columnName">The name of the column which to render.</param>
		/// <param name="ownerProperties">The <see cref="IPropertyBag"/> to which the column belongs.</param>
		/// <param name="blockDataset">The <see cref="Dataset"/> containing the all blocks of the <paramref name="ownerProperties"/>.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		public void RenderColumn(IMansionContext context, string columnName, IPropertyBag ownerProperties, Dataset blockDataset, string targetField)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");
			if (ownerProperties == null)
				throw new ArgumentNullException("ownerProperties");
			if (blockDataset == null)
				throw new ArgumentNullException("blockDataset");
			if (string.IsNullOrEmpty(targetField))
				throw new ArgumentNullException("targetField");

			// render the column section
			using (context.Stack.Push("OwnerNode", ownerProperties))
			using (context.Stack.Push("BlockColumn", new PropertyBag
			                                         {
			                                         	{"name", columnName}
			                                         }))
			using (templateService.Render(context, "BlockColumn", targetField))
			{
				// loop through all the blocks to render them
				foreach (var blockProperties in blockDataset.Rows.Where(node =>
				                                                        {
				                                                        	// check if the block belongs in this column
				                                                        	string targetColumn;
				                                                        	return node.TryGet(context, "column", out targetColumn) && columnName.Equals(targetColumn, StringComparison.OrdinalIgnoreCase);
				                                                        }))
					RenderBlock(context, blockProperties, "Block");
			}
		}
		/// <summary>
		/// Gets a <see cref="Dataset"/> containing all the columns available for the specified <paramref name="type"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/> for which to get the column <see cref="Dataset"/>.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing all the columns.</returns>
		public Dataset GetColumnDataset(IMansionContext context, ITypeDefinition type)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (type == null)
				throw new ArgumentNullException("type");

			// get the schema of the type
			var columnSchema = ColumnSchema.GetSchema(context, type);

			// create a dataset
			var columnSet = new Dataset();

			// loop over all the column
			foreach (var column in columnSchema.Columns)
			{
				columnSet.AddRow(new PropertyBag
				                 {
				                 	{"value", column},
				                 	{"label", column}
				                 });
			}

			return columnSet;
		}
		#endregion
		#region Block Methods
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		public void RenderBlock(IMansionContext context, IPropertyBag blockProperties, string targetField)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (blockProperties == null)
				throw new ArgumentNullException("blockProperties");
			if (string.IsNullOrEmpty(targetField))
				throw new ArgumentNullException("targetField");

			// first get the block behavior
			var blockType = blockProperties.Get<ITypeDefinition>(context, "type");
			BlockBehaviorDescriptor behavior;
			if (!blockType.TryFindDescriptorInHierarchy(out behavior))
				throw new InvalidOperationException(string.Format("Block type '{0}' does not have a behavior", blockType.Name));

			// perform the rendering on the behavior
			behavior.Render(context, blockProperties, targetField);
		}
		#endregion
		#region Private Fields
		/// <summary>
		/// This prefix uniquely identifies this response template tag. different tags with the same cacheKey will yield different results.
		/// </summary>
		private readonly string cacheKeyPrefix = "PortalService" + "_" + Guid.NewGuid() + "_";
		private readonly ICachingService cachingService;
		private readonly ITemplateService templateService;
		#endregion
	}
}