using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Social.ScriptTags
{
	/// <summary>
	/// Retrieves the social profile of the authenticated user. This tag might start the OAuth workflow.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "retrieveProfile")]
	public class RetrieveProfileTag : SocialTagBase<Profile>
	{
		#region Constructors
		/// <summary>
		/// Constructs this tag.
		/// </summary>
		/// <param name="discoveryService"></param>
		public RetrieveProfileTag(ISocialServiceDiscoveryService discoveryService) : base(discoveryService)
		{
		}
		#endregion
		#region Overrides of SocialTagBase<Profile>
		/// <summary>
		/// Executes the request to the given <paramref name="socialService"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>,</param>
		/// <param name="socialService">The <see cref="ISocialService"/> on which to perform the request.</param>
		/// <returns>Returns the <see cref="Result{TModel}"/>.</returns>
		protected override Result<Profile> ExecuteRequest(IMansionWebContext context, ISocialService socialService)
		{
			return socialService.RetrieveProfile(context.Cast<IMansionWebContext>());
		}
		/// <summary>
		/// Process the model when the request is execute successfully.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="model">The model.</param>
		protected override void ProcessResult(IMansionWebContext context, Profile model)
		{
			//  push the profile to the stack and allow children to modify it
			using (context.Stack.Push(GetRequiredAttribute<string>(context, "target"), PropertyBagAdapterFactory.Adapt(context, model), GetAttribute(context, "global", false)))
				ExecuteChildTags(context);
		}
		#endregion
	}
}