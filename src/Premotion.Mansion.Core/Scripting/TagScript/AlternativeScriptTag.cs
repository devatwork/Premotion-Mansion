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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			ExecuteChildTags(context);
		}
		#endregion
	}
}