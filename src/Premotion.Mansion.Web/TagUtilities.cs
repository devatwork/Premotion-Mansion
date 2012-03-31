using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Provides utility method for managing tags.
	/// </summary>
	public static class TagUtilities
	{
		#region Retrieve Methods
		/// <summary>
		/// Retrieves the tag index <see cref="Node"/>. Creates one when no exists.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="Node"/> representing the tag index.</returns>
		public static Node RetrieveTagIndexNode(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the repository
			var repository = context.Repository;

			// retrieve the root node
			var rootNode = repository.RetrieveRootNode(context);

			// retrieve the tag index or create it
			var tagIndexNode = repository.RetrieveSingle(context, new PropertyBag
			                                                      {
			                                                      	{"type", "TagIndex"},
			                                                      	{"parentSource", rootNode},
			                                                      	{"depth", "any"}
			                                                      }) ?? repository.Create(context, rootNode, new PropertyBag
			                                                                                                 {
			                                                                                                 	{"type", "TagIndex"},
			                                                                                                 	{"name", "Tags"},
			                                                                                                 	{"approved", true}
			                                                                                                 });
			return tagIndexNode;
		}
		/// <summary>
		/// Retrieves the tag <see cref="Node"/> with name <paramref name="tagName"/>. Creates one when no exists.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="tagIndexNode">The tag index node.</param>
		/// <param name="tagName">The name of the tag.</param>
		/// <returns>Returns the <see cref="Node"/> representing the tag index.</returns>
		public static Node RetrieveTagNode(IMansionContext context, Node tagIndexNode, string tagName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (tagIndexNode == null)
				throw new ArgumentNullException("tagIndexNode");
			tagName = Normalize(tagName);
			if (string.IsNullOrEmpty(tagName))
				throw new ArgumentNullException("tagName");

			// get the repository
			var repository = context.Repository;

			// retrieve the tag or create it
			return repository.RetrieveSingle(context, new PropertyBag
			                                          {
			                                          	{"parentSource", tagIndexNode},
			                                          	{"name", tagName},
			                                          	{"type", "Tag"}
			                                          }) ?? repository.Create(context, tagIndexNode, new PropertyBag
			                                                                                         {
			                                                                                         	{"name", tagName},
			                                                                                         	{"type", "Tag"},
			                                                                                         	{"approved", true},
			                                                                                         });
		}
		/// <summary>
		/// Tries to retrieve the tag <see cref="Node"/> with name <paramref name="tagName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="tagIndexNode">The tag index node.</param>
		/// <param name="tagName">The name of the tag.</param>
		/// <param name="tagNode"></param>
		/// <returns>Returns true when the node was found, otherwise false.</returns>
		public static bool TryRetrieveTagNode(IMansionContext context, Node tagIndexNode, string tagName, out Node tagNode)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (tagIndexNode == null)
				throw new ArgumentNullException("tagIndexNode");
			tagName = Normalize(tagName);
			if (string.IsNullOrEmpty(tagName))
				throw new ArgumentNullException("tagName");

			// get the repository
			var repository = context.Repository;

			// retrieve the node
			tagNode = repository.RetrieveSingle(context, new PropertyBag
			                                             {
			                                             	{"parentSource", tagIndexNode},
			                                             	{"name", tagName},
			                                             	{"type", "Tag"}
			                                             });
			return tagNode != null;
		}
		#endregion
		#region Tag Methods
		/// <summary>
		/// Splits <paramref name="tagNames"/> and normalizes the results.
		/// </summary>
		/// <param name="tagNames">A CSV of tag names.</param>
		/// <returns>Returns the normalized tag names.</returns>
		public static IEnumerable<string> NormalizeNames(string tagNames)
		{
			return (tagNames ?? string.Empty).Split(new[] {','}).Select(Normalize).Where(tagName => !string.IsNullOrEmpty(tagName)).Distinct();
		}
		/// <summary>
		/// Normalizes the given <paramref name="tagName"/>.
		/// </summary>
		/// <param name="tagName">The tag name.</param>
		/// <returns>Returns the normalized tag names.</returns>
		public static string Normalize(string tagName)
		{
			return (tagName ?? string.Empty).Trim().ToLower();
		}
		/// <summary>
		/// Removes a tag name from the <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties containing the current tag names.</param>
		/// <param name="tagName">The name of the tag which to remove.</param>
		public static void RemoveTagName(IMansionContext context, IPropertyBag properties, string tagName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");
			tagName = Normalize(tagName);
			if (string.IsNullOrEmpty(tagName))
				throw new ArgumentNullException("tagName");

			// get the current tag names
			var tagNames = properties.Get(context, "tags", string.Empty).Split(new[] {','}).Select(Normalize).Distinct().ToList();

			// remove the tag
			tagNames.Remove(tagName);

			// set the new tag names
			properties.Set("tags", string.Join(", ", tagNames));
		}
		#endregion
		#region Sync Methods
		/// <summary>
		/// Manages the tags.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The new properties.</param>
		public static void ToGuids(IMansionContext context, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// get the tag name string
			string tagNameString;
			if (!properties.TryGet(context, "_tags", out tagNameString))
				return;

			// normalize the tag names
			var tagNames = NormalizeNames(tagNameString).ToList();

			// retrieve the tag index node
			var tagIndexNode = RetrieveTagIndexNode(context);

			// loop over all the tag names
			var tagNodes = tagNames.Select(tagName => RetrieveTagNode(context, tagIndexNode, tagName));

			// set the new tag guids
			properties.Set("tagGuids", string.Join(",", tagNodes.Select(x => x.PermanentId)));
			properties.Remove("_tags");
		}
		/// <summary>
		/// Manages the tags.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The new properties.</param>
		/// <returns>A CSV of all the tag names.</returns>
		public static string ToNames(IMansionContext context, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");

			var tagNames = string.Empty;

			// retrieve the guids
			string tagGuids;
			if (properties.TryGet(context, "tagGuids", out tagGuids) && !string.IsNullOrEmpty(tagGuids))
			{
				// retrieve the tag index node
				var tagIndexNode = RetrieveTagIndexNode(context);

				// retrieve the tags by their guid
				var tagNodes = context.Repository.Retrieve(context, new PropertyBag
				                                                    {
				                                                    	{"parentSource", tagIndexNode},
				                                                    	{"guid", tagGuids}
				                                                    });

				// assemble the tag names
				tagNames = string.Join(", ", tagNodes.Nodes.Select(x => x.Pointer.Name));
			}

			return tagNames;
		}
		#endregion
	}
}