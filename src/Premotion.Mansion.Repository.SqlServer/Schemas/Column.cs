using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns.Prioritized;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents a column of a <see cref="Table"/>.
	/// </summary>
	public abstract class Column : IPrioritized
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="columnName"></param>
		protected Column(string columnName) : this(columnName, columnName)
		{
		}
		/// <summary>
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="propertyName"></param>
		protected Column(string columnName, string propertyName) : this(columnName, propertyName, 100)
		{
		}
		/// <summary>
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="propertyName"></param>
		/// <param name="priority"> </param>
		protected Column(string columnName, string propertyName, int priority)
		{
			// validate arguments
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			ColumnName = columnName;
			PropertyName = propertyName;
			this.priority = priority;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this column.
		/// </summary>
		public string ColumnName { get; private set; }
		/// <summary>
		/// Gets the name of the property to which this column maps.
		/// </summary>
		public string PropertyName { get; private set; }
		#endregion
		#region Statement Mapping Methods
		/// <summary>
		/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <param name="values">The values on which to construct the where statement.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void ToWhereStatement(IMansionContext context, QueryCommandContext commandContext, IEnumerable<object> values)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (values == null)
				throw new ArgumentNullException("values");
			if (commandContext == null)
				throw new ArgumentNullException("commandContext");

			// check if there are any values
			var valueArray = values.ToArray();
			if (valueArray.Length == 0)
			{
				commandContext.QueryBuilder.AppendWhere("1 = 0");
				return;
			}

			// get the table in which the column exists from the schema
			var pair = commandContext.Schema.FindTableAndColumn(PropertyName);

			// invoke template method
			DoToWhereStatement(context, commandContext, pair, valueArray);
		}
		/// <summary>
		/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <param name="pair">The <see cref="TableColumnPair"/>.</param>
		/// <param name="values">The values on which to construct the where statement.</param>
		protected virtual void DoToWhereStatement(IMansionContext context, QueryCommandContext commandContext, TableColumnPair pair, IList<object> values)
		{
			throw new NotSupportedException(string.Format("Columns of type '{0}' do not support where mapping", GetType().Name));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="properties"></param>
		public void ToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			DoToInsertStatement(context, queryBuilder, properties);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="properties"></param>
		protected abstract void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, IPropertyBag properties);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="record"></param>
		/// <param name="modifiedProperties"></param>
		public void ToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Record record, IPropertyBag modifiedProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (record == null)
				throw new ArgumentNullException("record");
			if (modifiedProperties == null)
				throw new ArgumentNullException("modifiedProperties");
			DoToUpdateStatement(context, queryBuilder, record, modifiedProperties);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="record"></param>
		/// <param name="modifiedProperties"></param>
		protected abstract void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Record record, IPropertyBag modifiedProperties);
		/// <summary>
		/// Generates an sync statements of this colum.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="command"></param>
		/// <param name="node"></param>
		/// <param name="columnText"></param>
		/// <param name="valueText"></param>
		public void ToSyncStatement(IMansionContext context, SqlCommand command, Record node, StringBuilder columnText, StringBuilder valueText)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (command == null)
				throw new ArgumentNullException("command");
			if (node == null)
				throw new ArgumentNullException("node");
			if (columnText == null)
				throw new ArgumentNullException("columnText");
			if (valueText == null)
				throw new ArgumentNullException("valueText");

			// invoke template method
			DoToSyncStatement(context, command, node, columnText, valueText);
		}
		/// <summary>
		/// Generates an sync statements of this colum.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="command"></param>
		/// <param name="record"></param>
		/// <param name="columnText"></param>
		/// <param name="valueText"></param>
		protected virtual void DoToSyncStatement(IMansionContext context, SqlCommand command, Record record, StringBuilder columnText, StringBuilder valueText)
		{
			throw new NotSupportedException(string.Format("Column type '{0}' can not be synced", GetType()));
		}
		#endregion
		#region Implementation of IPrioritized
		/// <summary>
		/// Gets the relative priority of this object. The higher the priority, earlier this object is executed.
		/// </summary>
		public int Priority
		{
			get { return priority; }
		}
		#endregion
		#region Private Fields
		private readonly int priority;
		#endregion
	}
}