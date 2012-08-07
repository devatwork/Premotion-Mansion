using System;
using Premotion.Mansion.Core.Data.Facets;

namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Represents a <see cref="FacetDefinition"/> <see cref="QueryComponent"/>.
	/// </summary>
	public class FacetQueryComponent : QueryComponent
	{
		#region Constructors
		/// <summary>
		/// Constructs this facet query componnent with the given <paramref name="facet"/>.
		/// </summary>
		/// <param name="facet">The <see cref="Facet"/>.</param>
		public FacetQueryComponent(FacetDefinition facet)
		{
			// validate arguments
			if (facet == null)
				throw new ArgumentNullException("facet");

			// set values
			this.facet = facet;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="FacetDefinition"/>.
		/// </summary>
		public FacetDefinition Facet
		{
			get { return facet; }
		}
		#endregion
		#region Private Fields
		private readonly FacetDefinition facet;
		#endregion
	}
}