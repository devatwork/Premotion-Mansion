﻿using System;
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
		/// <returns>Returns an <see cref="IDisposable"/> which cleans up the opened repository and the stack.</returns>
		public static IDisposable Open(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// resolve the data storaget
			BaseStorageEngine storageEngine;
			if (!context.Nucleus.TryResolveSingle(out storageEngine))
				throw new InvalidOperationException("There is no data storage engine configured within this application, please register a data store");

			var disposableChain = new DisposableChain();

			// create the repository
			IRepository repository = new OrchestratingRepository(
				context.Nucleus.ResolveSingle<BaseStorageEngine>(),
				context.Nucleus.Resolve<BaseQueryEngine>(),
				context.Nucleus.Resolve<BaseIndexEngine>()
				);

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