using System;
using System.Collections.Generic;
using System.Text;
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
		#region Overrides of QueryComponent
		/// <summary>
		/// Gets the names of the properties used by this query component.
		/// </summary>
		/// <returns>Returns the property hints.</returns>
		protected override IEnumerable<string> DoGetPropertyHints()
		{
			return new[] {facet.PropertyName};
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.AppendFormat("facet:{0}", facet.PropertyName);
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