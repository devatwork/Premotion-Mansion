using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Social.ScriptTags
{
	/// <summary>
	/// Base class for all social tags.
	/// </summary>
	/// <typeparam name="TModel">The type of accessed by this service.</typeparam>
	public abstract class SocialTagBase<TModel> : ScriptTag where TModel : class
	{
		#region Constructors
		/// <summary>
		/// Constructs this tag.
		/// </summary>
		/// <param name="discoveryService"></param>
		protected SocialTagBase(ISocialServiceDiscoveryService discoveryService)
		{
			// validate arguments
			if (discoveryService == null)
				throw new ArgumentNullException("discoveryService");

			// set values
			this.discoveryService = discoveryService;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// case the context
			var webContext = context.Cast<IMansionWebContext>();

			// retrieve the provider
			var providerName = GetRequiredAttribute<string>(webContext, "providerName");
			var socialService = discoveryService.Locate(providerName);

			// invoke the template method
			var result = ExecuteRequest(webContext, socialService);

			// if the result was successful, execute the children
			if (result.IsSuccessful)
				ProcessResult(webContext, result.Model);
			else if (result.IsOAuthRedirect)
			{
				// redirect to oAuth flow
				WebUtilities.RedirectRequest(context, result.RedirectUrl);
			}
			else
			{
				// throw the exception
				throw result.Exception;
			}
		}
		/// <summary>
		/// Executes the request to the given <paramref name="socialService"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>,</param>
		/// <param name="socialService">The <see cref="ISocialService"/> on which to perform the request.</param>
		/// <returns>Returns the <see cref="Result{TModel}"/>.</returns>
		protected abstract Result<TModel> ExecuteRequest(IMansionWebContext context, ISocialService socialService);
		/// <summary>
		/// Process the model when the request is execute successfully.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="model">The model.</param>
		protected abstract void ProcessResult(IMansionWebContext context, TModel model);
		#endregion
		#region Private Fields
		private readonly ISocialServiceDiscoveryService discoveryService;
		#endregion
	}
}