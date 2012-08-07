using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Patterns.Prioritized;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Implements the <see cref="Query"/> parser.
	/// </summary>
	public class QueryParser : IQueryParser
	{
		#region Constructors
		/// <summary>
		/// Constructs a query parser with the given <paramref name="processors"/>.
		/// </summary>
		/// <param name="processors">The <see cref="QueryArgumentProcessor"/>s which to use to parse the query.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="processors"/> is null.</exception>
		public QueryParser(IEnumerable<QueryArgumentProcessor> processors)
		{
			// validate arguments
			if (processors == null)
				throw new ArgumentNullException("processors");

			// sort the processor list
			this.processors = processors.OrderByPriority().ToList();
		}
		#endregion
		#region Parse Methods
		/// <summary>
		/// Parses the given <paramref name="parameters"/> into a <see cref="Query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parameters">The parameters which to parse.</param>
		/// <returns>Returns the <see cref="Query"/>.</returns>
		/// <exception cref="ArgumentNullException">Throw if <paramref name="context"/> or <paramref name="parameters"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if any of the parameters could not be parsed into a query.</exception>
		public Query Parse(IMansionContext context, IPropertyBag parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			// create a copy of the parameters, we do not want to change the parameters here
			var parametersCopy = parameters.Copy();

			// parse the query
			var query = DoParse(context, parametersCopy);

			// check to make sure all parameters are eaten
			if (parametersCopy.Count != 0)
				throw new InvalidOperationException(string.Format("Could not parse parameters {0} into a query", string.Join(", ", parametersCopy.Names)));

			// return the created query
			return query;
		}
		/// <summary>
		/// Parses the given <paramref name="parameters"/> into a <see cref="Query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parameters">The parameters which to parse.</param>
		/// <returns>Returns the <see cref="Query"/>.</returns>
		protected virtual Query DoParse(IMansionContext context, IPropertyBag parameters)
		{
			// create the query
			var query = new Query();

			// allow all the intepreters to modify the query
			foreach (var processor in Processors)
				processor.Process(context, parameters, query);

			// return the parsed query
			return query;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="QueryArgumentProcessor"/> of this processor.
		/// </summary>
		protected IEnumerable<QueryArgumentProcessor> Processors
		{
			get { return processors; }
		}
		#endregion
		#region Private Fields
		private readonly List<QueryArgumentProcessor> processors;
		#endregion
	}
}