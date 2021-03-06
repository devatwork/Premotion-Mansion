﻿using System;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;

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
		public static Nodeset RetrieveNodeset(this IRepository repository, IMansionContext context, IPropertyBag arguments)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// parse the query
			var parser = context.Nucleus.ResolveSingle<IQueryParser>();
			var query = parser.Parse(context, arguments);

			// execute the query
			return repository.RetrieveNodeset(context, query);
		}
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> on which to execute the query.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		public static Node RetrieveSingleNode(this IRepository repository, IMansionContext context, IPropertyBag arguments)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// parse the query
			var parser = context.Nucleus.ResolveSingle<IQueryParser>();
			var query = parser.Parse(context, arguments);

			// execute the query
			return repository.RetrieveSingleNode(context, query);
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
			var rootNode = repository.RetrieveSingleNode(context, new Query().Add(new IsPropertyEqualSpecification("id", 1)).Add(new StorageOnlyQueryComponent()));
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
		public static Node RetrieveSingleNode(this IRepository repository, IMansionContext context, int id)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");

			// retrieve the node
			return repository.RetrieveSingleNode(context, new Query().Add(new IsPropertyEqualSpecification("id", id)));
		}
		/// <summary>
		/// Retrieves the <see cref="Node"/> by it's <paramref name="guid"/>.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> from which to retrieve the node.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="guid">The <see cref="Guid"/> of the <see cref="Node"/> which to retrieve.</param>
		/// <returns>Returns the <see cref="Node"/> when found.</returns>
		public static Node RetrieveSingleNode(this IRepository repository, IMansionContext context, Guid guid)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");

			// retrieve the node
			return repository.RetrieveSingleNode(context, new Query().Add(new IsPropertyEqualSpecification("guid", guid)));
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
	}
}