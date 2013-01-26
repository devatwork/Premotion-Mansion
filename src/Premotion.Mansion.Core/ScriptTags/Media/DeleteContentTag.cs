using System;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media
{
	/// <summary>
	/// Deletes a content resource.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "deleteContent")]
	public class DeleteContentTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentResourceService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public DeleteContentTag(IContentResourceService contentResourceService)
		{
			// validate arguments
			if (contentResourceService == null)
				throw new ArgumentNullException("contentResourceService");

			// set values
			this.contentResourceService = contentResourceService;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// parse the path to the resource
			var resourcePath = contentResourceService.ParsePath(context, GetAttributes(context));

			// delete the content
			contentResourceService.DeleteResource(context, resourcePath);
		}
		#endregion
		#region Private Fields
		private readonly IContentResourceService contentResourceService;
		#endregion
	}
}