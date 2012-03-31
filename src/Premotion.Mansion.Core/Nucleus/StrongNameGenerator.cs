using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Generates strong names for components registered in the <see cref="INucleus"/>.
	/// </summary>
	public static class StrongNameGenerator
	{
		#region Generate Methods
		/// <summary>
		/// Generates the strong component name from the <paramref name="attribute"/>.
		/// </summary>
		/// <param name="attribute">The <see cref="NamedAttribute"/>.</param>
		/// <returns>Returns the generated strong name.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="attribute"/> is null.</exception>
		public static string Generate(NamedAttribute attribute)
		{
			// validate arguments
			if (attribute == null)
				throw new ArgumentNullException("attribute");

			return Generate(attribute.NamespaceUri, attribute.Name);
		}
		/// <summary>
		/// Generates the strong component name from the <paramref name="namespaceUri"/> and <paramref name="name"/>.
		/// </summary>
		/// <param name="namespaceUri">The namespace in which the component lives.</param>
		/// <param name="name">The name of the component.</param>
		/// <returns>Returns the generated strong name.</returns>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="namespaceUri"/> or <paramref name="name"/> is null.</exception>
		public static string Generate(string namespaceUri, string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(namespaceUri))
				throw new ArgumentNullException("namespaceUri");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			return namespaceUri.ToLowerInvariant() + ":" + name.ToLowerInvariant();
		}
		#endregion
	}
}