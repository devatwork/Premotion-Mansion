using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Registers all the services used by repository applications.
	/// </summary>
	public class DataApplicationBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public DataApplicationBootstrapper() : base(10)
		{
		}
		#endregion
		#region Overrides of ApplicationBootstrapper
		/// <summary>
		/// Registers all the services used by the application in the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> in which to register the services used by the application.</param>
		protected override void DoBoostrap(IConfigurableNucleus nucleus)
		{
			// registers the portal service
			nucleus.Register<IQueryParser>(resolver => new QueryParser(resolver.Resolve<QueryArgumentProcessor>()));
		}
		#endregion
	}
}