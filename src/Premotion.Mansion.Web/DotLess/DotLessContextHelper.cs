using System;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Hosting.AspNet;

namespace Premotion.Mansion.Web.DotLess
{
	/// <summary>
	/// Contains helper functions for dotLess plugins.
	/// </summary>
	public static class DotLessContextHelper
	{
		#region Mansion Accessors
		/// <summary>
		/// Gets the <see cref="IApplicationResourceService"/>.
		/// </summary>
		private static readonly Lazy<IApplicationResourceService> ApplicationResourceServiceResolver = new Lazy<IApplicationResourceService>(() => MansionWebApplicationContextFactory.Instance.Nucleus.ResolveSingle<IApplicationResourceService>());
		/// <summary>
		/// Gets the <see cref="IExpressionScriptService"/>.
		/// </summary>
		private static readonly Lazy<IExpressionScriptService> ExpressionScriptServiceResolver = new Lazy<IExpressionScriptService>(() => MansionWebApplicationContextFactory.Instance.Nucleus.ResolveSingle<IExpressionScriptService>());
		/// <summary>
		/// Gets the <see cref="IApplicationResourceService"/>.
		/// </summary>
		public static IApplicationResourceService ApplicationResourceService
		{
			get { return ApplicationResourceServiceResolver.Value; }
		}
		/// <summary>
		/// Gets the <see cref="IExpressionScriptService"/>.
		/// </summary>
		public static IExpressionScriptService ExpressionScriptService
		{
			get { return ExpressionScriptServiceResolver.Value; }
		}
		/// <summary>
		/// Get the <see cref="IMansionContext"/> of the current request.
		/// </summary>
		/// <returns>Returns the <see cref="IMansionContext"/>.</returns>
		public static IMansionWebContext GetContext()
		{
			// wrap the http context
			var wrappedContext = new HttpContextWrapper(HttpContext.Current);

			// get the mansion application context
			var applicationContext = MansionWebApplicationContextFactory.Instance;

			// create the request
			var request = HttpContextAdapter.Adapt(wrappedContext);

			// get the mansion request context
			return MansionWebContext.Create(applicationContext, request);
		}
		#endregion
	}
}