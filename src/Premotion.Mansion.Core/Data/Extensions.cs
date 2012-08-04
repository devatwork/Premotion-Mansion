using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Specifications;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Defines extension methods for several types.
	/// </summary>
	public static class Extensions
	{
		#region Extensions of IRepository
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> on which to execute the query.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		public static Nodeset Retrieve(this IRepository repository, IMansionContext context, IPropertyBag arguments)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// parse the query
			var query = repository.ParseQuery(context, arguments);

			// execute the query
			return repository.Retrieve(context, query);
		}
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> on which to execute the query.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		public static Node RetrieveSingle(this IRepository repository, IMansionContext context, IPropertyBag arguments)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// parse the query
			var query = repository.ParseQuery(context, arguments);

			// execute the query
			return repository.RetrieveSingle(context, query);
		}
		/// <summary>
		/// Retrieves the root <see cref="Node"/>.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> from which to retrieve the node.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the root node when found.</returns>
		/// <exception cref="InvalidOperationException">Thrown when root node could not be found in the repository.</exception>
		public static Node RetrieveRootNode(this IRepository repository, IMansionContext context)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");

			// retrieve the node
			var rootNode = repository.RetrieveSingle(context, new PropertyBag
			                                                  {
			                                                  	{"id", 1},
			                                                  	{"bypassAuthorization", true}
			                                                  });
			if (rootNode == null)
				throw new InvalidOperationException("Could not find root node, please check repository");
			return rootNode;
		}
		/// <summary>
		/// Retrieves the <see cref="Node"/> by it's <paramref name="id"/>.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> from which to retrieve the node.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="id">The ID of the <see cref="Node"/> which to retrieve.</param>
		/// <returns>Returns the <see cref="Node"/> when found.</returns>
		public static Node RetrieveSingle(this IRepository repository, IMansionContext context, int id)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");

			// retrieve the node
			return repository.RetrieveSingle(context, new PropertyBag
			                                          {
			                                          	{"id", id},
			                                          	{"bypassAuthorization", true}
			                                          });
		}
		/// <summary>
		/// Retrieves the <see cref="Node"/> by it's <paramref name="guid"/>.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> from which to retrieve the node.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="guid">The <see cref="Guid"/> of the <see cref="Node"/> which to retrieve.</param>
		/// <returns>Returns the <see cref="Node"/> when found.</returns>
		public static Node RetrieveSingle(this IRepository repository, IMansionContext context, Guid guid)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");

			// retrieve the node
			return repository.RetrieveSingle(context, new PropertyBag
			                                          {
			                                          	{"guid", guid},
			                                          	{"bypassAuthorization", true}
			                                          });
		}
		#endregion
		#region Extensions of IMansionContext
		/// <summary>
		/// Gets the unwrapped <see cref="IRepository"/> which is stripped from all decorators.
		/// </summary>
		/// <returns>Returns the unwrapped <see cref="IRepository"/>.</returns>
		public static IRepository GetUnwrappedRepository(this IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the current repository
			var repository = context.Repository;

			// unwrap the decorators
			while (repository is IRepositoryDecorator)
			{
				// unwrap it
				repository = ((IRepositoryDecorator) repository).DecoratedRepository;
			}

			return repository;
		}
		#endregion
		#region Extensions of Query
		/// <summary>
		/// Adds the <paramref name="specification"/> to the <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The <see cref="Query"/>.</param>
		/// <param name="specification">The <see cref="Specification"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> or <paramref name="specification"/> is null.</exception>
		public static void Add(this Query query, Specification specification)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");
			if (specification == null)
				throw new ArgumentNullException("specification");

			// wrap the specification in a component
			var component = new SpecificationQueryComponent(specification);

			// add the component to the query
			query.Add(component);
		}
		/// <summary>
		/// Adds the <paramref name="sorts"/> to the <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The <see cref="Query"/>.</param>
		/// <param name="sorts">The <see cref="Specification"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> or <paramref name="sorts"/> is null.</exception>
		public static void Add(this Query query, IEnumerable<Sort> sorts)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");
			if (sorts == null)
				throw new ArgumentNullException("sorts");

			// wrap the specification in a component
			var component = new SortQueryComponent(sorts);

			// add the component to the query
			query.Add(component);
		}
		#endregion
	}
}