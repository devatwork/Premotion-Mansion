using System;
using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents a column of a table.
	/// </summary>
	public abstract class Column
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
		protected Column(string columnName, string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			ColumnName = columnName;
			PropertyName = propertyName;
		}
		#endregion
		#region ToStatement Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="properties"></param>
		public void ToInsertStatement(MansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (newPointer == null)
				throw new ArgumentNullException("newPointer");
			if (properties == null)
				throw new ArgumentNullException("properties");
			DoToInsertStatement(context, queryBuilder, newPointer, properties);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="properties"></param>
		protected abstract void DoToInsertStatement(MansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag properties);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		public void ToUpdateStatement(MansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (node == null)
				throw new ArgumentNullException("node");
			if (modifiedProperties == null)
				throw new ArgumentNullException("modifiedProperties");
			DoToUpdateStatement(context, queryBuilder, node, modifiedProperties);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		protected abstract void DoToUpdateStatement(MansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties);
		/// <summary>
		/// Generates an sync statements of this colum.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="command"></param>
		/// <param name="node"></param>
		/// <param name="columnText"></param>
		/// <param name="valueText"></param>
		public void ToSyncStatement(MansionContext context, SqlCommand command, Node node, StringBuilder columnText, StringBuilder valueText)
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
		/// <param name="node"></param>
		/// <param name="columnText"></param>
		/// <param name="valueText"></param>
		protected virtual void DoToSyncStatement(MansionContext context, SqlCommand command, Node node, StringBuilder columnText, StringBuilder valueText)
		{
			throw new NotSupportedException(string.Format("Column type '{0}' can not be synced", GetType()));
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
	}
}