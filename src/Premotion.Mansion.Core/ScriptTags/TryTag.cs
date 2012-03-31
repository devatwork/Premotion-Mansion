using System;
using System.Threading;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags
{
	/// <summary>
	/// Implements the mansion document tag.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "try")]
	public class TryTag : ScriptTag
	{
		/// <summary>
		/// Executes the tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			try
			{
				ExecuteChildTags(context);
			}
			catch (ThreadAbortException)
			{
				// thread is aborted, so don't throw any new exceptions
			}
			catch (Exception ex)
			{
				// get the catch tag
				CatchTag catchTag;
				if (!TryGetAlternativeChildTag(out catchTag))
					throw new ScriptTagException(this, ex);

				// get the exception details
				var exceptionDetails = new PropertyBag
				                       {
				                       	{"type", ex.GetType().FullName},
				                       	{"message", ex.Message},
				                       	{"stacktrace", ex.StackTrace}
				                       };

				// push error to the stack
				using (context.Stack.Push("Exception", exceptionDetails, false))
					catchTag.Execute(context);
			}
		}
	}
}