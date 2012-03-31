using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Boolean
{
	/// <summary>
	/// Implements the if condition tag.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "if")]
	public class IfTag : ScriptTag
	{
		/// <summary>
		/// Executes the tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the condition
			var condition = GetAttribute<bool>(context, "condition");
			if (condition)
				ExecuteChildTags(context);
			else
			{
				// check for alternative branch
				ElseTag elseTag;
				if (TryGetAlternativeChildTag(out elseTag))
					elseTag.Execute(context);
			}
		}
	}
}