using System;
using System.Linq;
using Premotion.Mansion.Core.Data.Facets;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Parses the facet parameters.
	/// </summary>
	public class FacetArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public FacetArgumentProcessor() : base(100)
		{
		}
		#endregion
		#region Overrides of QueryArgumentProcessor
		/// <summary>
		/// Processes the <paramref name="parameters"/> and turn them into <paramref name="query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parameters">The parameters which to process.</param>
		/// <param name="query">The <see cref="Query"/> in which to set the parameters.</param>
		protected override void DoProcess(IMansionContext context, IPropertyBag parameters, Query query)
		{
			// loop over all the facet properties
			var facetPropertyNames = parameters.Names.Where(candidate => candidate.StartsWith("facet")).ToList();
			foreach (var facetPropertyName in facetPropertyNames)
			{
				// retrieve the facet from the property
				FacetDefinition facet;
				if (!parameters.TryGetAndRemove(context, facetPropertyName, out facet))
					throw new InvalidOperationException(string.Format("Property {0} did not contain a valid facet", facetPropertyName));

				// add the query component
				query.Add(new FacetQueryComponent(facet));
			}
		}
		#endregion
	}
}