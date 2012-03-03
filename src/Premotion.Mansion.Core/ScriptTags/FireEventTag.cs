using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags
{
	/// <summary>
	/// Calls a procedure.
	/// </summary>
	[Named(Constants.NamespaceUri, "fireEvent")]
	public class FireEventTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get all the attributes
			var procedureArguments = GetAttributes(context);

			// get the name of the procedure
			string eventName;
			if (!procedureArguments.TryGetAndRemove(context, "eventName", out eventName) || string.IsNullOrEmpty(eventName))
				throw new InvalidOperationException("The attribute eventName must have a valid event name.");

			// invoke the procedure
			using (context.Stack.Push("Arguments", procedureArguments, false))
			{
				foreach (var handler in context.EventHandlerStack.PeekAll(eventName))
					handler.Execute(context);
			}
		}
	}
}