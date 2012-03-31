using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Represents a template
	/// </summary>
	public interface ITemplate
	{
		#region Section Methods
		/// <summary>
		/// Tries to get the section with the name <paramref name="sectionName"/> from this template.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName">The name of the section.</param>
		/// <param name="section">The section when found.</param>
		/// <returns>Returns true when the section was found, otherwise false.</returns>
		bool TryGet(IMansionContext context, string sectionName, out ISection section);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the path to the resource from which this template is loaded.
		/// </summary>
		IResourcePath Path { get; }
		#endregion
	}
}