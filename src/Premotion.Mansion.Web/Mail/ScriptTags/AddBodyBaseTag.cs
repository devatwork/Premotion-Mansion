using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Mail.ScriptTags
{
	/// <summary>
	/// Base class for all add body tags.
	/// </summary>
	public abstract class AddBodyBaseTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="isContentHtml">Flag indicating whether the content is html or plain text.</param>
		protected AddBodyBaseTag(bool isContentHtml)
		{
			// set values
			this.isContentHtml = isContentHtml;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			var webContext = context.Cast<IMansionWebContext>();

			// invoke template method
			var content = GetContent(webContext);

			// check if the body is not set already
			var message = webContext.Message;

			// create the view
			var alternateView = AlternateView.CreateAlternateViewFromString(content, encoding, isContentHtml ? MediaTypeNames.Text.Html : MediaTypeNames.Text.Plain);

			// add the alternate view to the message
			message.AlternateViews.Add(alternateView);
		}
		/// <summary>
		/// Gets the content which to add to the message.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the content.</returns>
		protected abstract string GetContent(IMansionWebContext context);
		#endregion
		#region Private Fields
		private readonly Encoding encoding = Encoding.UTF8;
		private readonly bool isContentHtml;
		#endregion
	}
}