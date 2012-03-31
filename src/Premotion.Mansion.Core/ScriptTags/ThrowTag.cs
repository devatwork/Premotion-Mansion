using System;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags
{
	/// <summary>
	/// Implements the mansion document tag.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "throw")]
	public class ThrowTag : ScriptTag
	{
		/// <summary>
		/// Executes the tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the message
			var message = GetAttribute<string>(context, "message");

			// throw the exception
			throw new ApplicationException(message);
		}
	}
}