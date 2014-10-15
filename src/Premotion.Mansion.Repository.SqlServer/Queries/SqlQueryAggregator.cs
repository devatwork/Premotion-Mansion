using System;
using System.Text;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
    /// <summary>
    /// Combines several statements using the given combinator.
    /// </summary>
    public class SqlQueryAggregator
    {
        #region Constructors
        /// <summary>
        /// Constructs an instance of <see cref="SqlQueryAggregator"/>.
        /// </summary>
        /// <param name="combinator">The combinator which to use.</param>
        private SqlQueryAggregator(string combinator)
        {
            // Validate arguments
            if (string.IsNullOrEmpty(combinator))
                throw new ArgumentNullException("combinator");

            // Remember values
            this.combinator = combinator;
            Buffer = new StringBuilder();
        }
        #endregion
        #region Factory Methods
        /// <summary>
        /// Creates an <see cref="SqlQueryAggregator"/> instance using the AND operator.
        /// </summary>
        /// <returns>Returns the created instance of <see cref="SqlQueryAggregator"/>.</returns>
        public static SqlQueryAggregator And()
        {
            return new SqlQueryAggregator(" AND ");
        }
        /// <summary>
        /// Creates an <see cref="SqlQueryAggregator"/> instance using the OR operator.
        /// </summary>
        /// <returns>Returns the created instance of <see cref="SqlQueryAggregator"/>.</returns>
        public static SqlQueryAggregator Or()
        {
            return new SqlQueryAggregator(" OR ");
        }
        #endregion
        #region Append Methods
        /// <summary>
        /// Appends a where clause.
        /// </summary>
        /// <param name="clause">The clause which to append.</param>
        public void AppendWhere(string clause)
        {
            // validate arguments
            if (string.IsNullOrEmpty(clause))
                throw new ArgumentNullException("clause");

            // check for separator
            if (Buffer.Length > 0)
                Buffer.Append(combinator);

            Buffer.Append("(");
            Buffer.Append(clause);
            Buffer.Append(")");
        }
        /// <summary>
        /// Appends a where clause.
        /// </summary>
        /// <param name="clause">The clause which to append.</param>
        /// <param name="parameters">The parameters to use in the format string.</param>
        public void AppendWhere(string clause, params object[] parameters)
        {
            // validate arguments
            if (string.IsNullOrEmpty(clause))
                throw new ArgumentNullException("clause");

            // check for separator
            if (Buffer.Length > 0)
                Buffer.Append(combinator);

            Buffer.Append("(");
            Buffer.AppendFormat(clause, parameters);
            Buffer.Append(")");
        }
        #endregion
        #region Properties
        /// <summary>
        /// The buffer of this aggregator.
        /// </summary>
        public StringBuilder Buffer { get; private set; }
        #endregion
        #region Fields
        /// <summary>
        /// The combining operator.
        /// </summary>
        private readonly string combinator;
        #endregion
    }
}