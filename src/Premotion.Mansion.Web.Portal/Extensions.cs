﻿using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web.Portal
{
	/// <summary>
	/// Implements extension methods used by the portal module.
	/// </summary>
	public static class Extensions
	{
		#region Repository Extension
		/// <summary>
		/// Retrieves the content index root <see cref="Node"/>.
		/// </summary>
		/// <remarks>If the content index root node does not exist it will be created.</remarks>
		/// <param name="repository">The <see cref="IRepository"/> from which to retrieve the root node.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the content index root <see cref="Node"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="repository"/> or <paramref name="context"/> is null.</exception>
		public static Node RetrieveContentIndexRootNode(this IRepository repository, IMansionContext context)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");

			// first retrieve the root node
			var rootNode = repository.RetrieveRootNode(context);

			// retrieve the node or create it when it does not exist
			return repository.RetrieveSingle(context, new PropertyBag
			                                          {
			                                          	{"parentSource", rootNode},
			                                          	{"type", "ContentIndexRoot"},
			                                          	{"bypassAuthorization", true}
			                                          }) ?? repository.Create(context, rootNode, new PropertyBag
			                                                                                     {
			                                                                                     	{"type", "ContentIndexRoot"},
			                                                                                     	{"name", "Shared content"}
			                                                                                     });
		}
		/// <summary>
		/// Retrieves the taxonomy <see cref="Node"/>.
		/// </summary>
		/// <remarks>If the taxonomy node does not exist it will be created.</remarks>
		/// <param name="repository">The <see cref="IRepository"/> from which to retrieve the root node.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the taxonomy <see cref="Node"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="repository"/> or <paramref name="context"/> is null.</exception>
		public static Node RetrieveTaxonomyNode(this IRepository repository, IMansionContext context)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");

			// first retrieve the root node
			var contentIndexRootNode = repository.RetrieveContentIndexRootNode(context);

			// retrieve the node or create it when it does not exist
			return repository.RetrieveSingle(context, new PropertyBag
			                                          {
			                                          	{"parentSource", contentIndexRootNode},
			                                          	{"type", "Taxonomy"},
			                                          	{"bypassAuthorization", true}
			                                          }) ?? repository.Create(context, contentIndexRootNode, new PropertyBag
			                                                                                                 {
			                                                                                                 	{"type", "Taxonomy"},
			                                                                                                 	{"name", "Taxonomy"}
			                                                                                                 });
		}
		#endregion
	}
}