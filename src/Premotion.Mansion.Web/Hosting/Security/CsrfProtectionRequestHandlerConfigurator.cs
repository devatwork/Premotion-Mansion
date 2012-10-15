using System;
using System.Net;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Web.Hosting.Security
{
	/// <summary>
	/// Protects request handlers with session write-access from CSRF.
	/// </summary>
	public class CsrfProtectionRequestHandlerConfigurator : RequestHandlerConfigurator<ScriptRequestHandler>
	{
		#region Constants
		private const string CsrfSessionToken = "CsrfToken";
		private const string CsrfCookieToken = "CsrfToken";
		#endregion
		#region Constructors
		/// <summary></summary>
		/// <param name="conversionService"></param>
		/// <param name="encryptionService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public CsrfProtectionRequestHandlerConfigurator(IConversionService conversionService, IEncryptionService encryptionService)
		{
			// validate arguments
			if (conversionService == null)
				throw new ArgumentNullException("conversionService");
			if (encryptionService == null)
				throw new ArgumentNullException("encryptionService");

			// set values
			this.conversionService = conversionService;
			this.encryptionService = encryptionService;
		}
		#endregion
		#region Overrides of RequestHandlerConfigurator<RequestHandler>
		/// <summary>
		/// Allows the configuration of the given <paramref name="handler"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="handler">The <see cref="RequestHandler"/> selected to handle the request.</param>
		protected override void DoConfigure(IMansionWebContext context, ScriptRequestHandler handler)
		{
			handler.BeforePipeline.AddStageToBeginOfPipeline(ctx =>
			                                                 {
			                                                 	// check for post only
			                                                 	if ("POST".Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase))
			                                                 		return null;

			                                                 	// check for CSRF token in session
			                                                 	var sessionToken = ctx.Session[CsrfSessionToken] as CsrfToken;

			                                                 	// check for CSRF token in cookie
			                                                 	WebCookie csrfCookie;
			                                                 	CsrfToken cookieToken = null;
			                                                 	if (ctx.Request.Cookies.TryGetValue(CsrfSessionToken, out csrfCookie) && !string.IsNullOrEmpty(csrfCookie.Value))
			                                                 	{
			                                                 		// convert cookie into token
			                                                 		cookieToken = conversionService.Convert<CsrfToken>(ctx, csrfCookie.Value, null);
			                                                 	}

			                                                 	// if both tokens are null, the request is valid
			                                                 	if (sessionToken == null && cookieToken == null)
			                                                 		return null;

			                                                 	// if the tokens are equal, the request has not been tampered with
			                                                 	if (sessionToken == cookieToken)
			                                                 		return null;

			                                                 	// redirect to homepage and clear all the cookie data
			                                                 	var response = WebResponse.Create(ctx);
			                                                 	response.RedirectLocation = ctx.Request.ApplicationUrl;
			                                                 	response.StatusCode = HttpStatusCode.Found;
			                                                 	response.StatusDescription = "Tampered request";

			                                                 	// loop over all the request cookies and clear them
			                                                 	foreach (var cookie in ctx.Request.Cookies)
			                                                 	{
			                                                 		response.Cookies.Add(new WebCookie
			                                                 		                     {
			                                                 		                     	Expires = DateTime.Now.AddDays(-1),
			                                                 		                     	Name = cookie.Key
			                                                 		                     });
			                                                 	}

			                                                 	// clear the session
			                                                 	ctx.Session[CsrfCookieToken] = null;

			                                                 	return response;
			                                                 });
			handler.AfterPipeline.AddStageToEndOfPipeline((ctx, response) =>
			                                              {
			                                              	// check if the session contains a token
			                                              	var sessionToken = ctx.Session[CsrfSessionToken] as CsrfToken;
			                                              	if (sessionToken == null)
			                                              	{
			                                              		// generate a token
			                                              		sessionToken = CsrfToken.Create(ctx, encryptionService);

			                                              		// store the token in the session
			                                              		ctx.Session[CsrfSessionToken] = sessionToken;
			                                              	}

			                                              	// if the token was already in the original request and is not modified, do not set it
			                                              	WebCookie csrfCookie;
			                                              	if (ctx.Request.Cookies.TryGetValue(CsrfSessionToken, out csrfCookie) && !string.IsNullOrEmpty(csrfCookie.Value))
			                                              	{
			                                              		// convert cookie into token
			                                              		var cookieToken = conversionService.Convert<CsrfToken>(ctx, csrfCookie.Value, null);

			                                              		// check for equality
			                                              		if (sessionToken == cookieToken)
			                                              			return;
			                                              	}

			                                              	//store the token in a cookie
			                                              	response.Cookies.Add(new WebCookie
			                                              	                     {
			                                              	                     	HttpOnly = true,
			                                              	                     	Name = CsrfCookieToken,
			                                              	                     	Value = conversionService.Convert<string>(ctx, sessionToken)
			                                              	                     });
			                                              });
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		private readonly IEncryptionService encryptionService;
		#endregion
	}
}