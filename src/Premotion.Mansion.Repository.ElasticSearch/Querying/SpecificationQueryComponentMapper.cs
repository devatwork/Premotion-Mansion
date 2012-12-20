using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying
{
	/// <summary>
	/// Maps <see cref="SpecificationQueryComponent"/>s.
	/// </summary>
	public class SpecificationQueryComponentMapper : QueryComponentMapper<SpecificationQueryComponent>
	{
		#region Constructors
		/// <summary>
		/// Constructs an <see cref="SpecificationQueryComponentMapper"/>.
		/// </summary>
		/// <param name="mappers">The <see cref="ISpecificationMapper"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="mappers"/> is null.</exception>
		public SpecificationQueryComponentMapper(IEnumerable<ISpecificationMapper> mappers)
		{
			// validate arguments
			if (mappers == null)
				throw new ArgumentNullException("mappers");

			// set value
			this.mappers = mappers.ToArray();
		}
		#endregion
		#region Overrides of QueryComponentMapper<SpecificationQueryComponent>
		/// <summary>
		/// Maps the given <paramref name="component"/> to the <paramref name="search"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="component">The source <see cref="QueryComponent"/> being mapped.</param>
		/// <param name="search">The <see cref="SearchDescriptor"/> to which to map <paramref name="component"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, SpecificationQueryComponent component, SearchDescriptor search)
		{
			mappers.Elect(context, component.Specification).Map(context, query, component.Specification, search);
		}
		#endregion
		#region Private Fields
		private readonly IEnumerable<ISpecificationMapper> mappers;
		#endregion
	}
}