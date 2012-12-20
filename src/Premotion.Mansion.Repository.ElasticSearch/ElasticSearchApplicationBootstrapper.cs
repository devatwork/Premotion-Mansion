﻿using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Connection;
using Premotion.Mansion.Repository.ElasticSearch.Indexing;
using Premotion.Mansion.Repository.ElasticSearch.Quering;
using Premotion.Mansion.Repository.ElasticSearch.Schema;

namespace Premotion.Mansion.Repository.ElasticSearch
{
	/// <summary>
	/// Bootstraps ElasticSearch enabled applications.
	/// </summary>
	public class ElasticSearchApplicationBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public ElasticSearchApplicationBootstrapper() : base(50)
		{
		}
		#endregion
		#region Overrides of ApplicationBootstrapper
		/// <summary>
		/// Registers all the services used by the application in the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> in which to register the services used by the application.</param>
		protected override void DoBootstrap(IConfigurableNucleus nucleus)
		{
			// register the ElasticSearch services
			nucleus.Register(resolver => new ConnectionManager());
			nucleus.Register(resolver => new IndexDefinitionResolver(resolver.ResolveSingle<ITypeService>()));
			nucleus.Register(resolver => new Indexer(resolver.ResolveSingle<ConnectionManager>(), resolver.ResolveSingle<IndexDefinitionResolver>()));
			nucleus.Register(resolver => new Searcher(resolver.ResolveSingle<ConnectionManager>(), resolver.ResolveSingle<IndexDefinitionResolver>()));
		}
		#endregion
	}
}