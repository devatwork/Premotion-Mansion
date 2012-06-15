using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Repository.SqlServer.Converters;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Represents a select query.
	/// </summary>
	public class SelectQuery : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="command"></param>
		/// <param name="sqlStringBuilder"></param>
		/// <param name="schema"></param>
		/// <param name="originalQuery"></param>
		private SelectQuery(IDbCommand command, SqlStringBuilder sqlStringBuilder, Schema schema, NodeQuery originalQuery)
		{
			// validate arguments
			if (command == null)
				throw new ArgumentNullException("command");
			if (sqlStringBuilder == null)
				throw new ArgumentNullException("sqlStringBuilder");
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (originalQuery == null)
				throw new ArgumentNullException("originalQuery");

			// set values
			this.command = command;
			this.sqlStringBuilder = sqlStringBuilder;
			this.schema = schema;
			this.originalQuery = originalQuery;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Prepares an insert query.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="connection">The connection.</param>
		/// <param name="schemaProvider"></param>
		/// <param name="query"></param>
		/// <param name="converters"></param>
		/// <returns></returns>
		public static SelectQuery Prepare(IMansionContext context, SqlConnection connection, SchemaProvider schemaProvider, NodeQuery query, IEnumerable<IClauseConverter> converters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (schemaProvider == null)
				throw new ArgumentNullException("schemaProvider");
			if (query == null)
				throw new ArgumentNullException("query");
			if (converters == null)
				throw new ArgumentNullException("converters");

			// get the schema for this query
			var schema = SchemaProvider.Resolve(context, query);

			// create the command
			var queryBuilder = new SqlStringBuilder(schema.RootTable);
			var command = connection.CreateCommand();
			command.CommandType = CommandType.Text;

			converters = converters.ToList();

			// map all clauses
			foreach (var clause in query.Clauses)
			{
				IClauseConverter converter;
				if (Election<IClauseConverter, NodeQueryClause>.TryElect(context, converters, clause, out converter))
					converter.Map(context, schema, command, queryBuilder, clause);
			}

			// assemble the query
			return new SelectQuery(command, queryBuilder, schema, query);
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes this command and returns a nodeset.
		/// </summary>
		/// <param name="context">The application context.</param>
		/// <returns></returns>
		public Nodeset Execute(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// set the SQL statement
			command.CommandText = sqlStringBuilder.ToString();

			// execute the command
			using (var reader = command.ExecuteReader(CommandBehavior.Default))
			{
				// default mapping
				var nodeset = new Nodeset(context, MapRecords(context, reader), MapSetProperties(reader));

				// map the result of additional queries
				while (reader.NextResult())
				{
					// get the mapper from the query
					sqlStringBuilder.GetAdditionalQueryResultMapper()(nodeset, reader);
				}

				// return the result
				return nodeset;
			}
		}
		/// <summary>
		/// Executes this command and returns a single node.
		/// </summary>
		/// <param name="context">The application context.</param>
		/// <returns></returns>
		public Node ExecuteSingle(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// set the SQL statement
			command.CommandText = sqlStringBuilder.ToSingleNodeQuery();

			// execute the command
			using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
			{
				// first check if there is a result
				if (!reader.Read())
					return null;

				// map to node.
				return Map(context, reader);
			}
		}
		#endregion
		#region Map Methods
		/// <summary>
		/// Maps the input of the reader to a nodeset.
		/// </summary>
		/// <param name="context">The application context.</param>
		/// <param name="reader">The reader from which to read.</param>
		/// <returns>Returns a nodeset.</returns>
		private IEnumerable<Node> MapRecords(IMansionContext context, IDataReader reader)
		{
			var nodelist = new List<Node>();

			// read all nodes.
			while (reader.Read())
				nodelist.Add(Map(context, reader));

			return nodelist;
		}
		/// <summary>
		/// Maps the properties of a set.
		/// </summary>
		/// <param name="reader">The reader which to map.</param>
		/// <returns>Returns a property bag containing the properties.</returns>
		private IPropertyBag MapSetProperties(IDataReader reader)
		{
			var properties = new PropertyBag();

			// check for sort
			SortClause sortClause;
			if (originalQuery.TryGetClause(out sortClause))
				properties.Set("sort", string.Join(", ", sortClause.Sorts.Select(sort => sort.PropertyName + " " + (sort.Ascending ? "asc" : "desc"))));

			// check for paging
			PagingClause pagingClause;
			if (originalQuery.TryGetClause(out pagingClause))
			{
				properties.Set("pageNumber", pagingClause.PageNumber);
				properties.Set("pageSize", pagingClause.PageSize);
			}

			// check if there is a result set for the properties);
			if (!reader.NextResult())
				return properties;

			// make sure there is one result
			if (!reader.Read())
				return properties;

			// map the properties
			for (var index = 0; index < reader.FieldCount; index++)
				properties.Set(reader.GetName(index), reader.GetValue(index));

			return properties;
		}
		/// <summary>
		/// Maps a data record to a node.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		/// <returns></returns>
		private Node Map(IMansionContext context, IDataRecord record)
		{
			// validate argument
			if (record == null)
				throw new ArgumentNullException("record");

			// field indices
			var idIndex = record.GetOrdinal("id");
			var parentPointerIndex = record.GetOrdinal("parentPointer");
			var nameIndex = record.GetOrdinal("name");
			var parentPathIndex = record.GetOrdinal("parentPath");
			var typeIndex = record.GetOrdinal("type");
			var parentStructureIndex = record.GetOrdinal("parentStructure");
			var extendedPropertiesIndex = record.GetOrdinal("extendedProperties");

			// assemble the node pointer
			var pointer = (record.IsDBNull(parentPointerIndex) ? string.Empty : record.GetString(parentPointerIndex) + NodePointer.PointerSeparator) + record.GetInt32(idIndex);
			var structure = (record.IsDBNull(parentStructureIndex) ? string.Empty : record.GetString(parentStructureIndex) + NodePointer.StructureSeparator) + record.GetString(typeIndex);
			var path = (record.IsDBNull(parentPathIndex) ? string.Empty : record.GetString(parentPathIndex) + NodePointer.PathSeparator) + record.GetString(nameIndex);
			var nodePointer = NodePointer.Parse(pointer, structure, path);

			// create the properties
			var properties = new PropertyBag();

			// deserialize extended properties
			if (!record.IsDBNull(extendedPropertiesIndex))
			{
				// get the extended properties
				var extendedPropertiesLength = record.GetBytes(extendedPropertiesIndex, 0, null, 0, 0);
				var serializedProperties = new byte[extendedPropertiesLength];
				record.GetBytes(extendedPropertiesIndex, 0, serializedProperties, 0, serializedProperties.Length);

				// deserialize
				var conversionService = context.Nucleus.ResolveSingle<IConversionService>();
				var deserializedProperties = conversionService.Convert<IPropertyBag>(context, serializedProperties);

				// merge the deserialized properties
				properties.Merge(deserializedProperties);
			}

			// set all the column values as properties
			for (var i = 0; i < record.FieldCount; i++)
			{
				// check for invalid columns
				if (i == extendedPropertiesIndex)
					continue;

				// if the column is empty remove the value from the properties, otherwise set the value from the column
				if (record.IsDBNull(i))
				{
					object obj;
					properties.TryGetAndRemove(context, record.GetName(i), out obj);
				}
				else
					properties.Set(record.GetName(i), record.GetValue(i));
			}

			// normalize all the properties
			var normalizedProperties = new PropertyBag();
			foreach (var property in properties)
			{
				// normalize the value of the property
				PropertyColumn column;
				normalizedProperties.Set(property.Key, schema.TryFindPropertyColumn(property.Key, out column) ? column.Normalize(context, property.Value) : property.Value);
			}

			// create the node
			return new Node(context, nodePointer, normalizedProperties);
		}
		#endregion
		#region Dispose Implementation
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			if (!disposeManagedResources)
				return;

			// cleanup
			command.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly IDbCommand command;
		private readonly NodeQuery originalQuery;
		private readonly Schema schema;
		private readonly SqlStringBuilder sqlStringBuilder;
		#endregion
	}
}