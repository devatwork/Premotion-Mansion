namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Used by the template service internally, do not use this interface directly.
	/// </summary>
	public interface ITemplateServiceInternal : ITemplateService
	{
		#region Render Methods
		/// <summary>
		/// Renders the section as a string.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render.</param>
		/// <returns>Returns the rendered content of the section.</returns>
		string RenderToString(IMansionContext context, string sectionName);
		#endregion
	}
}