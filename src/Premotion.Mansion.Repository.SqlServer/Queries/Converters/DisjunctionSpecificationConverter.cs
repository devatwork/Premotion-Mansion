using System;
using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Converts <see cref="Disjunction"/> to a statement.
	/// </summary>
	public class DisjunctionSpecificationConverter : SpecificationConverter<Disjunction>
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="nucleus"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public DisjunctionSpecificationConverter(INucleus nucleus)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// create factory
			converterFactory = new Lazy<IEnumerable<ISpecificationConverter>>(nucleus.Resolve<ISpecificationConverter>);
		}
		#endregion
		#region Overrides of SpecificationConverter<Disjunction>
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, Disjunction specification, QueryCommandContext commandContext)
		{
			// construct the statement
			var buffer = new StringBuilder();
			var first = true;
			using (commandContext.QueryBuilder.WhereBuilderStack.Push(buffer))
			{
				foreach (var component in specification.Components)
				{
					// check separator
					if (first)
						first = false;
					else
						buffer.Append(" OR ");

					// append the specification
					converterFactory.Value.Elect(context, component).Convert(context, component, commandContext);
				}
			}

			// append the statement
			commandContext.QueryBuilder.AppendWhere(buffer.ToString());
		}
		#endregion
		#region Private Fields
		private readonly Lazy<IEnumerable<ISpecificationConverter>> converterFactory;
		#endregion
	}
}