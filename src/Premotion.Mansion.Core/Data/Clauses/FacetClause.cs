using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Data.Facets;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// Represents a facet clause.
	/// </summary>
	public class FacetClause : NodeQueryClause
	{
		#region Nested type: FacetClauseInterpreter
		/// <summary>
		/// Interprets <see cref = "FacetClause" />s.
		/// </summary>
		public class FacetClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// 
			/// </summary>
			public FacetClauseInterpreter() : base(10)
			{
			}
			#endregion
			#region Overrides of QueryInterpreter
			/// <summary>
			/// Interprets the input.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext" />.</param>
			/// <param name="input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(IMansionContext context, IPropertyBag input)
			{
				// loop over all the facet properties
				var facetPropertyNames = input.Names.Where(candidate => candidate.StartsWith("facet")).ToList();
				foreach (var facetPropertyName in facetPropertyNames)
				{
					// retrieve the facet from the property
					FacetDefinition facet;
					if (!input.TryGetAndRemove(context, facetPropertyName, out facet))
						throw new InvalidOperationException(string.Format("Property {0} did not contain a valid facet", facetPropertyName));

					// turn the facet into a clause
					yield return new FacetClause(facet);
				}
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this facet clause with the given <paramref name="facet"/>.
		/// </summary>
		/// <param name="facet">The <see cref="Facet"/>.</param>
		public FacetClause(FacetDefinition facet)
		{
			// validate arguments
			if (facet == null)
				throw new ArgumentNullException("facet");

			// set values
			this.facet = facet;
		}
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("facet:", facet.PropertyName);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="Facet"/>.
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