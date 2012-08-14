using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represens a property column.
	/// </summary>
	public class PropertyColumn : Column
	{
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="columnName">The name of the column.</param>
		/// <param name="properties"></param>
		public PropertyColumn(string propertyName, string columnName, IPropertyBag properties) : base(columnName, propertyName)
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");

			// set values
			Properties = properties;
		}
		#endregion
		#region Initialize Methods
		/// <summary>
		/// Initializes this column.
		/// </summary>
		/// <param name="context">The application context.</param>
		public void Initialize(IMansionContext context)
		{
			// TODO: refactor to initializer
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the allow null flag
			AllowNullValue = Properties.Get(context, "allowNullValue", false);

			// check if there is an expression
			string expressionString;
			if (Properties.TryGet(context, "expression", out expressionString))
			{
				var expressionService = context.Nucleus.ResolveSingle<IExpressionScriptService>();
				HasExpression = true;
				Expression = expressionService.Parse(context, new LiteralResource(expressionString));
			}

			// get the default value
			string defaultValue;
			if (Properties.TryGet(context, "defaultValue", out defaultValue))
			{
				var expressionService = context.Nucleus.ResolveSingle<IExpressionScriptService>();
				var defaultValueExpression = expressionService.Parse(context, new LiteralResource(defaultValue));
				DefaultValue = Normalize(context, defaultValueExpression.Execute<object>(context));
				HasDefaultValue = true;
			}
		}
		#endregion
		#region Overrides of Column
		/// <summary>
		/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <param name="pair">The <see cref="TableColumnPair"/>.</param>
		/// <param name="values">The values on which to construct the where statement.</param>
		protected override void DoToWhereStatement(IMansionContext context, QueryCommandContext commandContext, TableColumnPair pair, IList<object> values)
		{
			// add the table to the query
			commandContext.QueryBuilder.AddTable(context, pair.Table, commandContext.Command);

			// check for single or multiple values
			if (values.Count == 1)
				commandContext.QueryBuilder.AppendWhere(" [{0}].[{1}] = @{2}", pair.Table.Name, pair.Column.ColumnName, commandContext.Command.AddParameter(Normalize(context, values[0])));
			else
			{
				// start the clause
				var buffer = new StringBuilder();
				buffer.AppendFormat("[{0}].[{1}] IN (", pair.Table.Name, pair.Column.ColumnName);

				// loop through all the values
				foreach (var value in values.Select(val => Normalize(context, val)))
					buffer.AppendFormat("@{0},", commandContext.Command.AddParameter(value));

				// finish the clause
				commandContext.QueryBuilder.AppendWhere("{0})", buffer.Trim());
			}
		}
		#endregion
		#region Value Methods
		/// <summary>
		/// Normalizes the value of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to normalize.</param>
		/// <returns>Returns the normalized value.</returns>
		public object Normalize(IMansionContext context, object input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// parse the value
			return DoNormalize(context, input);
		}
		/// <summary>
		/// Normalizes the value of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to normalize.</param>
		/// <returns>Returns the normalized value.</returns>
		protected virtual object DoNormalize(IMansionContext context, object input)
		{
			// check for empty strings
			if (input is string && string.IsNullOrEmpty((string) input))
				input = null;

			// check if there is no expression
			if (!HasExpression)
				return input;

			// push the column variables to the stack
			using (context.Stack.Push("Column", new PropertyBag {{"value", input}}, false))
				return Expression.Execute<object>(context);
		}
		/// <summary>
		/// Gets the value of this column.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input value from which to derive the column value.</param>
		/// <returns>Returns the column value.</returns>
		private object GetValue(IMansionContext context, object input)
		{
			// normalize the input
			input = Normalize(context, input);
			if (input != null)
				return input;

			//  try default value
			if (HasDefaultValue)
				input = DefaultValue;

			// check for null
			if (input == null && AllowNullValue)
				input = DBNull.Value;

			// value must be non null now
			if (input == null)
				throw new InvalidOperationException(string.Format("Column {0} does not allow null values", ColumnName));

			return input;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the properties of this column.
		/// </summary>
		private IPropertyBag Properties { get; set; }
		/// <summary>
		/// Gets a flag indicating whether this column has a default value.
		/// </summary>
		private bool HasDefaultValue { get; set; }
		/// <summary>
		/// Gets the default value of this column.
		/// </summary>
		private object DefaultValue { get; set; }
		/// <summary>
		/// Gets a flag indicating whether this column allows null values.
		/// </summary>
		private bool AllowNullValue { get; set; }
		/// <summary>
		/// Gets a flag indicating whether this column has an expression.
		/// </summary>
		private bool HasExpression { get; set; }
		/// <summary>
		/// Gets the <see cref="IExpressionScript"/> of this column.
		/// </summary>
		private IExpressionScript Expression { get; set; }
		#endregion
	}
}