using System;
using Premotion.Mansion.Core.Data.Caching;
using Premotion.Mansion.Core.Data.Listeners;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;
using Premotion.Mansion.Core.Patterns;

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
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="repositoryNamespace">The namespace in which the repository lives.</param>
		/// <param name="applicationSettings">The <see cref="IPropertyBag"/> containing the application settings.</param>
		/// <returns>Returns an <see cref="IDisposable"/> which cleans up the opened repository and the stack.</returns>
		public static IDisposable Open(MansionContext context, string repositoryNamespace, IPropertyBag applicationSettings)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(repositoryNamespace))
				throw new ArgumentNullException("repositoryNamespace");
			if (applicationSettings == null)
				throw new ArgumentNullException("applicationSettings");

			// look up the factory for the repository
			var namingService = context.Nucleus.Get<ITypeDirectoryService>(context);
			var objectFactoryService = context.Nucleus.Get<IObjectFactoryService>(context);
			Type repositoryFactoryType;
			if (!namingService.TryLookupSingle<IRepositoryFactory>(repositoryNamespace, "Factory", out repositoryFactoryType))
				throw new InvalidOperationException(string.Format("Could not find repository factory in namespace '{0}'", repositoryNamespace));

			var disposableChain = new DisposableChain();

			// create the repository
			var repository = objectFactoryService.Create<IRepositoryFactory>(repositoryFactoryType).Create(context, applicationSettings);

			// decorate with listing capabilities
			repository = new ListeningRepositoryDecorator(repository);

			// decorate the caching capabilities
			repository = new CachingRepositoryDecorator(repository);

			// start the repository
			disposableChain.Add(repository);
			repository.Start(context);

			// push repository to the stack
			return disposableChain.Add(context.RepositoryStack.Push(repository));
		}
	}
}