using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Parser;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Registers the data services in a data driven application.
	/// </summary>
	public class RepositoryApplicationBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// Constructs <see cref="RepositoryApplicationBootstrapper"/>.
		/// </summary>
		public RepositoryApplicationBootstrapper() : base(10)
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
			// register the query argument parser
			nucleus.Register<IQueryParser>(resolver => new QueryParser(resolver.Resolve<QueryArgumentProcessor>()));
		}
		#endregion
	}
}