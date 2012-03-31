using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Decorated classes are identified as script tags.
	/// </summary>
	public class ScriptTagAttribute : NamedAttribute
	{
		#region Constructors
		/// <summary>
		/// Constructs a script tag attribute.
		/// </summary>
		/// <param name="namespaceUri">The namespace in which the scrip tag lives.</param>
		/// <param name="name">The name of the script tag.</param>
		public ScriptTagAttribute(string namespaceUri, string name) : base(typeof (ScriptTag), namespaceUri, name)
		{
		}
		#endregion
	}
}