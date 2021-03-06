using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Implements <see cref="IQueryComponentConverter"/> for <see cref="SpecificationQueryComponent"/>s.
	/// </summary>
	public class SpecificationQueryComponentConverter : QueryComponentConverter<SpecificationQueryComponent>
	{
		#region Constructors
		/// <summary>
		/// Constructs an <see cref="SpecificationQueryComponentConverter"/>.
		/// </summary>
		/// <param name="converters">The <see cref="ISpecificationConverter"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="converters"/> is null.</exception>
		public SpecificationQueryComponentConverter(IEnumerable<ISpecificationConverter> converters)
		{
			// validate arguments
			if (converters == null)
				throw new ArgumentNullException("converters");

			// set value
			this.converters = converters.ToArray();
		}
		#endregion
		#region Overrides of QueryComponentConverter<SpecificationQueryComponent>
		/// <summary>
		/// Converts the given <paramref name="component"/> into an statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="component">The <see cref="QueryComponent"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, SpecificationQueryComponent component, QueryCommandContext commandContext)
		{
			converters.Elect(context, component.Specification).Convert(context, component.Specification, commandContext);
		}
		#endregion
		#region Private Fields
		private readonly IEnumerable<ISpecificationConverter> converters;
		#endregion
	}
}