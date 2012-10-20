using System;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Social.ScriptTags
{
	/// <summary>
	/// Exchanges the code for an access token.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUrl, "exchangeCodeForAccessToken")]
	public class ExchangeCodeForAccessTokenTag : SocialTagBase<Url>
	{
		#region Constructors
		/// <summary>
		/// Constructs this tag.
		/// </summary>
		/// <param name="discoveryService"></param>
		public ExchangeCodeForAccessTokenTag(ISocialServiceDiscoveryService discoveryService) : base(discoveryService)
		{
		}
		#endregion
		#region Overrides of SocialTagBase<Url>
		/// <summary>
		/// Executes the request to the given <paramref name="socialService"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>,</param>
		/// <param name="socialService">The <see cref="ISocialService"/> on which to perform the request.</param>
		/// <returns>Returns the <see cref="Result{TModel}"/>.</returns>
		protected override Result<Url> ExecuteRequest(IMansionWebContext context, ISocialService socialService)
		{
			return socialService.ExchangeCodeForAccessToken(context.Cast<IMansionWebContext>());
		}
		/// <summary>
		/// Process the model when the request is execute successfully.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="model">The model.</param>
		protected override void ProcessResult(IMansionWebContext context, Url model)
		{
			// redirect to originating page
			WebUtilities.RedirectRequest(context, model);
		}
		#endregion
	}
}