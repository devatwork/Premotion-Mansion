namespace Premotion.Mansion.Core.Scripting.TagScript
{
	///<summary>
	/// Implements alternative tags.
	///</summary>
	public class AlternativeScriptTag : ScriptTag
	{
		#region Execute Methods
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			ExecuteChildTags(context);
		}
		#endregion
	}
}