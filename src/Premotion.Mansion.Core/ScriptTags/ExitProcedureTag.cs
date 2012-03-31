using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags
{
	/// <summary>
	/// Breaks the execution flow of the current procedure.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "exitProcedure")]
	public class ExitProcedureTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// set the break flag
			context.BreakTopMostProcedure = true;
		}
		#endregion
	}
}