using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Contains addintional information of an <see cref="ScriptTag"/>.
	/// </summary>
	public class TagInfo
	{
		#region Properties
		/// <summary>
		/// Gets/Sets the name of this tag.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Gets/Sets the namespace of this tag.
		/// </summary>
		public string Namespace { get; set; }
		/// <summary>
		/// Gets/Sets the linenumber on which this tag was found.
		/// </summary>
		public int LineNumber { get; set; }
		/// <summary>
		/// Gets the <see cref="IResource"/> from which this tag was loaded.
		/// </summary>
		public IResource Resource { get; set; }
		#endregion
	}
}