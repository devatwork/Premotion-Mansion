using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Web.Social.Facebook;

namespace Premotion.Mansion.Web.Social
{
	/// <summary>
	/// Registers all the social services.
	/// </summary>
	public class SocialServicesApplicationBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public SocialServicesApplicationBootstrapper() : base(50)
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
			// initialize the Facebook service
			nucleus.Register<IFacebookSocialService>(resolver => new FacebookSocialService(resolver.ResolveSingle<IConversionService>()));
			nucleus.Register<ISocialService>("facebook", resolver => resolver.ResolveSingle<IFacebookSocialService>());

			// register the social discovery service, initialized with all the social services
			nucleus.Register<ISocialServiceDiscoveryService>(resolver => new SocialServiceDiscoveryService(resolver.Resolve<ISocialService>()));
		}
		#endregion
	}
}