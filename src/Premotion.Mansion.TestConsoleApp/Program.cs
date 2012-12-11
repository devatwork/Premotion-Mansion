using System;
using System.Linq;
using System.Reflection;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.IO.EmbeddedResources;
using Premotion.Mansion.Core.IO.Windows;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Dynamo;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Core.Templating.Html;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Core.Types.Xml;
using Premotion.Mansion.Web.Security;

namespace Premotion.Mansion.TestConsoleApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// create a nucleus
			var nucleus = new DynamoNucleusAdapter();
			nucleus.Register<IReflectionService>(t => new ReflectionService());
			nucleus.ResolveSingle<IReflectionService>().Initialize(nucleus, new[] {Assembly.Load("Premotion.Mansion.Core")});
			nucleus.Register<IConversionService>(resolver => new ConversionService(resolver.Resolve<IConverter>(), resolver.Resolve<IComparer>()));
			nucleus.Register<IApplicationResourceService>(resolver => new EmbeddedApplicationResourceService("Web", resolver.Resolve<ResourcePathInterpreter>(), resolver.ResolveSingle<IReflectionService>()));
			nucleus.Register<ITypeService>(resolver => new XmlTypeService(resolver.ResolveSingle<ICachingService>(), resolver.ResolveSingle<IApplicationResourceService>()));
			nucleus.Register<ICachingService>(resolver => new TmpCachingService());
			nucleus.Register<ITemplateService>(resolver => new HtmlTemplateService(resolver.Resolve<SectionInterpreter>(), resolver.ResolveSingle<ICachingService>()));
			nucleus.Register<IContentResourceService>(resolver => new WindowsContentResourceService(Environment.CurrentDirectory, "Content"));
			nucleus.Register<ISecurityService>(resolver => new WebSecurityService(resolver.ResolveSingle<IConversionService>(), resolver.Resolve<AuthenticationProvider>(), resolver.ResolveSingle<IEncryptionService>()));
			nucleus.Register<ISecurityPersistenceService>(resolver => new RepositorySecurityPersistenceService());
			nucleus.Register<ISecurityModelService>(resolver => new SecurityModelService(resolver.ResolveSingle<ISecurityPersistenceService>()));
			nucleus.Register<ITagScriptService>(resolver => new TagScriptService(resolver.ResolveSingle<ICachingService>()));
			nucleus.Register<IExpressionScriptService>(resolver => new ExpressionScriptService(resolver.Resolve<ExpressionPartInterpreter>(), resolver.ResolveSingle<ICachingService>()));
			nucleus.Register(resolver => new MyService());
			nucleus.Register<IMyService>("myService", resolver => resolver.ResolveSingle<MyService>());
			// get all the application bootstrappers from the nucleus and allow them to bootstrap the application
			foreach (var bootstrapper in nucleus.Resolve<ApplicationBootstrapper>().OrderBy(bootstrapper => bootstrapper.Weight))
				bootstrapper.Bootstrap(nucleus);
			nucleus.Optimize();

			// create a context
			var context = new MansionContext(nucleus);

			// test resolve
			Type contractType;
			nucleus.TryResolveComponentContract("myService", out contractType);

			IMyService contractInstance;
			nucleus.TryResolveSingle("myService", out contractInstance);



			// invoke the add method
			Console.WriteLine(nucleus.ResolveSingle<MyService>().InvokeMethod<MyService, int>(context, "Add", new PropertyBag
			                                                                                                  {
			                                                                                                  	{"a", 1},
			                                                                                                  	{"b", 2},
			                                                                                                  }));
			Console.WriteLine(context.InvokeMethodOnComponent<int>("myService", "Add", new PropertyBag
			                                                                           {
			                                                                           	{"a", 1},
			                                                                           	{"b", 2},
			                                                                           }));
		}
	}
	public interface IMyService
	{
		int Add(int a, int b);
		int Increment(int a, int b = 1);
		void DoStuff(int a, int b = 1);
	}
	public class MyService : IMyService
	{
		public int Add(int a, int b)
		{
			return a + b;
		}
		public int Increment(int a, int b = 1)
		{
			return a + b;
		}
		public void DoStuff(int a, int b = 1)
		{
		}
	}
	public class TmpCachingService : ICachingService
	{
		#region Implementation of ICachingService
		/// <summary>
		/// Tries to get an object from this cache by its <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the stored object.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The <see cref="CacheKey"/>.</param>
		/// <param name="value">The <see cref="CachedObject{TObject}"/> found in this cache.</param>
		/// <returns>Returns true when the object was found, otherwise false.</returns>
		public bool TryGet<TObject>(IMansionContext context, CacheKey key, out CachedObject<TObject> value)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Gets an object of type <typeparamref name="TObject"/> from the cache byt it's key. If the item does not exist in the cache add and return it using <paramref name="valueFactory"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the stored object.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The <see cref="CacheKey"/>.</param>
		/// <param name="valueFactory">The value factory which provides the object if it doesn't exist in this cache.</param>
		/// <returns>Returns the value.</returns>
		public TObject GetOrAdd<TObject>(IMansionContext context, CacheKey key, Func<CachedObject<TObject>> valueFactory)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Adds or replaces an object of type <typeparamref name="TObject"/> in the cache.
		/// </summary>
		/// <typeparam name="TObject">The type of stored object.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The <see cref="CacheKey"/>.</param>
		/// <param name="valueFactory">The value factory which produces the cache object.</param>
		/// <returns>Returns the value.</returns>
		public TObject AddOrReplace<TObject>(IMansionContext context, CacheKey key, Func<CachedObject<TObject>> valueFactory)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Clears all the items in the cache matching <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The cache key.</param>
		public void Clear(CacheKey key)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Clears all items from the cache.
		/// </summary>
		public void ClearAll()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}