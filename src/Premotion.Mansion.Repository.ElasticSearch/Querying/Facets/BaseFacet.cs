using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Data.Facets;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Facets
{
	/// <summary>
	/// Base class for all facets.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class BaseFacet
	{
		#region Constructors
		/// <summary>
		/// Construct this facet.
		/// </summary>
		/// <param name="definition">The <see cref="FacetDefinition"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="definition"/> is null.</exception>
		protected BaseFacet(FacetDefinition definition)
		{
			// validate arguments
			if (definition == null)
				throw new ArgumentNullException("definition");

			// set the values
			Definition = definition;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="FacetDefinition"/>.
		/// </summary>
		public FacetDefinition Definition { get; private set; }
		#endregion
	}
}