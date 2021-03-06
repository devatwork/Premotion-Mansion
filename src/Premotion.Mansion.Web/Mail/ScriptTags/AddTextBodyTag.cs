﻿using System.Text;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Mail.ScriptTags
{
	/// <summary>
	/// Adds an text body to a message.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "addTextBody")]
	public class AddTextBodyTag : AddBodyBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public AddTextBodyTag() : base(false)
		{
		}
		#endregion
		#region Overrides of AddBodyBaseTag
		/// <summary>
		/// Gets the content which to add to the message.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the content.</returns>
		protected override string GetContent(IMansionWebContext context)
		{
			// get the content
			var content = new StringBuilder();
			using (var pipe = new StringOutputPipe(content))
			using (context.OutputPipeStack.Push(pipe))
				ExecuteChildTags(context);
			return content.ToString();
		}
		#endregion
	}
}