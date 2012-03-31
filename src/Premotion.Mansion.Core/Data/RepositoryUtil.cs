using System;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Data.Caching;
using Premotion.Mansion.Core.Data.Listeners;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Provides utility functions for repositories.
	/// </summary>
	public static class RepositoryUtil
	{
		/// <summary>
		/// Opens a repository and pushes it to the stack.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="repositoryNamespace">The namespace in which the repository lives.</param>
		/// <param name="applicationSettings">The <see cref="IPropertyBag"/> containing the application settings.</param>
		/// <returns>Returns an <see cref="IDisposable"/> which cleans up the opened repository and the stack.</returns>
		public static IDisposable Open(IMansionContext context, string repositoryNamespace, IPropertyBag applicationSettings)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(repositoryNamespace))
				throw new ArgumentNullException("repositoryNamespace");
			if (applicationSettings == null)
				throw new ArgumentNullException("applicationSettings");

			var disposableChain = new DisposableChain();

			// create the repository
			var repository = context.Nucleus.ResolveSingle<IRepositoryFactory>(repositoryNamespace, "Factory").Create(context, applicationSettings);

			// decorate with listing capabilities
			repository = new ListeningRepositoryDecorator(repository, context.Nucleus.ResolveSingle<ITypeService>());

			// decorate the caching capabilities
			repository = new CachingRepositoryDecorator(repository, context.Nucleus.ResolveSingle<ICachingService>());

			// start the repository
			disposableChain.Add(repository);
			repository.Start(context);

			// push repository to the stack
			return disposableChain.Add(context.RepositoryStack.Push(repository));
		}
	}
}