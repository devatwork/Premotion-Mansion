using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal
{
	/// <summary>
	/// Registers all the services used by portal applications.
	/// </summary>
	public class PortalApplicationBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public PortalApplicationBootstrapper() : base(50)
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
			nucleus.Register<IPortalService>(resolver => new PortalService(
			                                             	resolver.ResolveSingle<ICachingService>(),
			                                             	resolver.ResolveSingle<ITemplateService>(),
			                                             	resolver.ResolveSingle<IApplicationResourceService>(),
			                                             	resolver.ResolveSingle<ITagScriptService>(),
			                                             	resolver.ResolveSingle<IConversionService>()
			                                             	));
		}
		#endregion
	}
}