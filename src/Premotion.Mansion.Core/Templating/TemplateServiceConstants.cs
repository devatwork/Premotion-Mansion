namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Defines constants for <see cref="ITemplateService"/>
	/// </summary>
	public static class TemplateServiceConstants
	{
		#region Constants
		/// <summary>
		/// Specifies that the section should be rendered directly to the top-most output pipe.
		/// </summary>
		public const string OutputTargetField = "__output__";
		/// <summary>
		/// Specifies the default template extension.
		/// </summary>
		public const string DefaultTemplateExtension = "htm";
		#endregion
	}
}