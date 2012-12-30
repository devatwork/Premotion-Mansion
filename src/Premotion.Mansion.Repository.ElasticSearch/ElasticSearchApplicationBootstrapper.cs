using System.Configuration;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Connection;
using Premotion.Mansion.Repository.ElasticSearch.Indexing;
using Premotion.Mansion.Repository.ElasticSearch.Querying;
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
			// if elastic search is not configured, do not register its services
			var appSettings = ConfigurationManager.AppSettings;
			var connectionString = appSettings[Constants.ConnectionStringApplicationSettingKey];
			if (string.IsNullOrEmpty(connectionString))
				return;

			// register the ElasticSearch services
			nucleus.Register(resolver => new ConnectionManager());
			nucleus.Register(resolver => new IndexDefinitionResolver(resolver.ResolveSingle<ITypeService>(), resolver.ResolveSingle<ICachingService>()));
			nucleus.Register(resolver => new Indexer(resolver.ResolveSingle<ConnectionManager>(), resolver.ResolveSingle<IndexDefinitionResolver>()));
			nucleus.Register(resolver => new Searcher(resolver.ResolveSingle<ConnectionManager>(), resolver.ResolveSingle<IndexDefinitionResolver>(), resolver.Resolve<IQueryComponentMapper>()));
			nucleus.Register<BaseIndexEngine>("elasticsearch:indexengine", resolver => new ElasticSearchIndexEngine(resolver.ResolveSingle<Indexer>()));
			nucleus.Register<BaseQueryEngine>("elasticsearch:queryengine", resolver => new ElasticSearchQueryEngine(resolver.ResolveSingle<Searcher>(), resolver.ResolveSingle<IndexDefinitionResolver>()));
		}
		#endregion
	}
}