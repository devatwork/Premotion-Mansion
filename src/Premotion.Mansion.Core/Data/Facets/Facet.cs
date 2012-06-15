using System;

namespace Premotion.Mansion.Core.Data.Facets
{
	/// <summary>
	/// Represents the definition of a facet.
	/// </summary>
	public class Facet
	{
		#region Constructors
		/// <summary>
		/// Creates a facet with the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to facet.</param>
		/// <param name="friendlyName">The friendly name.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null or empty.</exception>
		public Facet(string propertyName, string friendlyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (string.IsNullOrEmpty(friendlyName))
				throw new ArgumentNullException("friendlyName");

			//set values
			PropertyName = propertyName;
			FriendlyName = friendlyName;
		}
		/// <summary>
		/// Copies the given <paramref name="facet"/>/
		/// </summary>
		/// <param name="facet">The <see cref="Facet"/> which to copy.</param>
		protected Facet(Facet facet)
		{
			// validate arguments
			if (facet == null)
				throw new ArgumentNullException("facet");

			//  set values
			FriendlyName = facet.FriendlyName;
			PropertyName = facet.PropertyName;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the frienldy name of this facet.
		/// </summary>
		public string FriendlyName { get; set; }
		/// <summary>
		/// Gets the name of the property on which to facet.
		/// </summary>
		public string PropertyName { get; set; }
		#endregion
	}
}