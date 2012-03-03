using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.IO.Windows;
using Premotion.Mansion.Core.Nucleus.Facilities.Dependencies;
using Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;
using Premotion.Mansion.Core.Nucleus.Implementation;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Core.Templating.Html;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Core.Types.Xml;
using Premotion.Mansion.Repository.SqlServer;
using Premotion.Mansion.Web;
using Premotion.Mansion.Web.Assets;
using Premotion.Mansion.Web.Caching;
using Premotion.Mansion.Web.Http;
using Premotion.Mansion.Web.Mail;
using Premotion.Mansion.Web.Mail.Standard;
using Premotion.Mansion.Web.Security;
using Premotion.Mansion.Web.Url;

namespace Premotion.Mansion.TestWebApp
{
	/// <summary>
	/// Implements <see cref="MansionHttpApplicationBase"/>.
	/// </summary>
	public class Global : MansionHttpApplicationBase
	{
		#region Overrides of MansionHttpApplicationBase
		/// <summary>
		/// Initializes the <see cref="MansionApplicationContext"/> used by this application.
		/// </summary>
		/// <returns>Return the initialized <see cref="MansionApplicationContext"/>.</returns>
		protected override MansionApplicationContext InitializeApplicationContext()
		{
			// create the nucleus
			var nucleus = new DefaultNucleus();
			var context = new MansionApplicationContext(nucleus);

			// augment the nucleus
			nucleus.Augment(context, new DependencyCheckerFacility());
			nucleus.Augment(context, new ServiceLifecycleManagementFacility());
			nucleus.Augment(context, new ReflectionFacility());

			// register all the assemblies participating in this application
			var assemblyRegistrationService = nucleus.Get<IAssemblyRegistrationService>(context);
			assemblyRegistrationService.RegisterAssembly(AssemblyModel.Create<MansionContext>("Mansion"));
			assemblyRegistrationService.RegisterAssembly(AssemblyModel.Create<MansionWebContext>("MansionWeb"));
			assemblyRegistrationService.RegisterAssembly(AssemblyModel.Create<SqlServerRepositoryFactory>("SqlServer"));
			assemblyRegistrationService.RegisterAssembly(AssemblyModel.Create<Global>("/"));

			// register all the services
			nucleus.Register<IConversionService>(context, new ConversionService());
			nucleus.Register<IApplicationResourceService>(context, new WindowsApplicationResourceService(HttpRuntime.AppDomainAppPath, "Web"));
			nucleus.Register<IContentResourceService>(context, new WindowsContentResourceService(HttpRuntime.AppDomainAppPath, "Content"));
			nucleus.Register<IExpressionScriptService>(context, new ExpressionScriptService());
			nucleus.Register<ITagScriptService>(context, new TagScriptService());
			nucleus.Register<ITemplateService>(context, new HtmlTemplateService());
			nucleus.Register<ITypeService>(context, new XmlTypeService());
			nucleus.Register<ICachingService>(context, new HttpCachingService());
			nucleus.Register<IMailService>(context, new StandardMailService());
			nucleus.Register<ISecurityService>(context, new WebSecurityService());
			nucleus.Register<INodeUrlService>(context, new NodeUrlService());
			nucleus.Register<ISecurityPersistenceService>(context, new RepositorySecurityPersistenceService());
			nucleus.Register<ISecurityModelService>(context, new SecurityModelService());
			nucleus.Register<IAssetService>(context, new AssetService());

			return context;
		}
		#endregion
	}
}