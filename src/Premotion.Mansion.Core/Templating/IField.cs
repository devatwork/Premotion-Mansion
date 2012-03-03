namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Represents a field in a section.
	/// </summary>
	public interface IField
	{
		#region Append Methods
		/// <summary>
		/// Appends content to this field.
		/// </summary>
		/// <param name="content">The content which to append.</param>
		void Append(string content);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the content of this field.
		/// </summary>
		string Content { get; }
		#endregion
	}
}