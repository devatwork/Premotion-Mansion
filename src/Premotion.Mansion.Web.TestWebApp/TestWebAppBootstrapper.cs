using Premotion.Mansion.Amazon.S3;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.IO.EmbeddedResources;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Core.Templating.Html;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Core.Types.Xml;
using Premotion.Mansion.Web.Assets;
using Premotion.Mansion.Web.Caching;
using Premotion.Mansion.Web.Mail;
using Premotion.Mansion.Web.Mail.Standard;
using Premotion.Mansion.Web.Security;
using Premotion.Mansion.Web.Urls;

namespace Premotion.Mansion.Web.TestWebApp
{
	/// <summary>
	/// Implements the <see cref="ApplicationBootstrapper"/> for the test web app.
	/// </summary>
	public class TestWebAppBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public TestWebAppBootstrapper() : base(100)
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
			nucleus.Register<ICachingService>(resolver => new HttpCachingService());
			nucleus.Register<IConversionService>(resolver => new ConversionService(resolver.Resolve<IConverter>(), resolver.Resolve<IComparer>()));
			nucleus.Register<ITemplateService>(resolver => new HtmlTemplateService(resolver.Resolve<SectionInterpreter>(), resolver.ResolveSingle<ICachingService>()));
			nucleus.Register<ITypeService>(resolver => new XmlTypeService(resolver.ResolveSingle<ICachingService>(), resolver.ResolveSingle<IApplicationResourceService>()));
			nucleus.Register<ISecurityService>(resolver => new WebSecurityService(resolver.ResolveSingle<IConversionService>(), resolver.Resolve<AuthenticationProvider>(), resolver.ResolveSingle<IEncryptionService>()));
			nucleus.Register<ISecurityPersistenceService>(resolver => new RepositorySecurityPersistenceService());
			nucleus.Register<ISecurityModelService>(resolver => new SecurityModelService(resolver.ResolveSingle<ISecurityPersistenceService>()));
			nucleus.Register<ITagScriptService>(resolver => new TagScriptService(resolver.ResolveSingle<ICachingService>()));
			nucleus.Register<IExpressionScriptService>(resolver => new ExpressionScriptService(resolver.Resolve<ExpressionPartInterpreter>(), resolver.ResolveSingle<ICachingService>()));
			nucleus.Register<IMailService>(resolver => new StandardMailService());
			nucleus.Register<INodeUrlService>(resolver => new NodeUrlService(resolver, resolver.ResolveSingle<ITypeService>()));
			nucleus.Register<IApplicationResourceService>(resolver => new EmbeddedApplicationResourceService("Web", resolver.Resolve<ResourcePathInterpreter>(), resolver.ResolveSingle<IReflectionService>()));
			nucleus.Register<IContentResourceService>(resolver => new S3ContentResourceService());
			nucleus.Register<IAssetService>(resolver => new AssetService(resolver.ResolveSingle<IContentResourceService>()));
		}
		#endregion
	}
}