namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Represents constants.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Defines the name of the property in which the linkbase data is stored on a record.
		/// </summary>
		public const string LinkbaseDataKey = "link:linkbasedata";
		/// <summary>
		/// The namespace in which the Linking descriptors live.
		/// </summary>
		public const string DescriptorNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/link-descriptors.xsd";
		/// <summary>
		/// The namespace in which the Linking tags live.
		/// </summary>
		public const string TagNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/link-tags.xsd";
	}
}