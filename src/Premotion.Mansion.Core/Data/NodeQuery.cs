using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Represents a query to a node.
	/// </summary>
	public class NodeQuery : IEnumerable<NodeQueryClause>
	{
		#region Clause Methods
		/// <summary>
		/// Adds a clause to this query.
		/// </summary>
		/// <param name="clause">The clause which to add.</param>
		public void Add(NodeQueryClause clause)
		{
			// validate arguments
			if (clause == null)
				throw new ArgumentNullException("clause");

			// add
			clauses.Add(clause);
		}
		/// <summary>
		/// Adds a range of clauses to this query.
		/// </summary>
		/// <param name="newClauses">The clauses which to add.</param>
		public void AddRange(IEnumerable<NodeQueryClause> newClauses)
		{
			// validate arguments
			if (newClauses == null)
				throw new ArgumentNullException("newClauses");

			// add
			clauses.AddRange(newClauses.ToArray());
		}
		/// <summary>
		/// Checks whether the query contains the specified clause.
		/// </summary>
		/// <typeparam name="TClause">The type of clause. Must inherit from <see cref="NodeQueryClause"/></typeparam>
		/// <returns>Returns true when the clause is found, otherwise false.</returns>
		public bool HasClause<TClause>() where TClause : NodeQueryClause
		{
			return Clauses.OfType<TClause>().Count() > 0;
		}
		#endregion
		#region Clause Methods
		/// <summary>
		/// Tries to get a specific clause from this query.
		/// </summary>
		/// <typeparam name="TClauseType">The type of <see cref="NodeQueryClause"/> which to get.</typeparam>
		/// <param name="clause">The found clause.</param>
		/// <returns>Returns true when the clause was found, otherwise false.</returns>
		public bool TryGetClause<TClauseType>(out TClauseType clause) where TClauseType : NodeQueryClause
		{
			// try to find the clause
			var queryClause = Clauses.SingleOrDefault(candidate => typeof (TClauseType).IsAssignableFrom(candidate.GetType()));
			if (queryClause == null)
			{
				clause = default(TClauseType);
				return false;
			}
			clause = (TClauseType) queryClause;
			return true;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the clauses of this query.
		/// </summary>
		public IEnumerable<NodeQueryClause> Clauses
		{
			get { return clauses; }
		}
		#endregion
		#region Implementation of IEnumerable{NodeQueryClause}
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<NodeQueryClause> GetEnumerator()
		{
			return clauses.GetEnumerator();
		}
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
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
			return string.Join("_", (Clauses.Select(candidate => candidate.ToString()).OrderBy(x => x, StringComparer.OrdinalIgnoreCase)));
		}
		#endregion
		#region Private Fields
		private readonly List<NodeQueryClause> clauses = new List<NodeQueryClause>();
		#endregion
	}
}