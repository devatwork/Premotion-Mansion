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
	/// Converts <see cref="Negation"/> to a statement.
	/// </summary>
	public class NegationSpecificationConverter : SpecificationConverter<Negation>
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="nucleus"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public NegationSpecificationConverter(INucleus nucleus)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// create factory
			converterFactory = new Lazy<IEnumerable<ISpecificationConverter>>(nucleus.Resolve<ISpecificationConverter>);
		}
		#endregion
		#region Overrides of SpecificationConverter<Negation>
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, Negation specification, QueryCommandContext commandContext)
		{
			// construct the statement
			var buffer = new StringBuilder();
			using (commandContext.QueryBuilder.WhereBuilderStack.Push(buffer))
				converterFactory.Value.Elect(context, specification.Negated).Convert(context, specification.Negated, commandContext);

			// append the statement
			commandContext.QueryBuilder.AppendWhere("NOT (" + buffer + ')');
		}
		#endregion
		#region Private Fields
		private readonly Lazy<IEnumerable<ISpecificationConverter>> converterFactory;
		#endregion
	}
}