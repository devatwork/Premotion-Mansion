using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns.Prioritized;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Repository.SqlServer.QueryCommands.Converters;
using Premotion.Mansion.Repository.SqlServer.QueryCommands.Mappers;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.QueryCommands
{
	/// <summary>
	/// Represents an executable select command.
	/// </summary>
	public class SelectCommand : QueryCommand
	{
		#region Constructors
		/// <summary>
		/// Constructs an <see cref="SelectCommand"/>.
		/// </summary>
		/// <param name="converters">The <see cref="IQueryComponentConverter"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="converters"/> is null.</exception>
		public SelectCommand(IEnumerable<IQueryComponentConverter> converters)
		{
			// validate arguments
			if (converters == null)
				throw new ArgumentNullException("converters");

			// set value
			this.converters = converters;
		}
		#endregion
		#region Prepare Methods
		/// <summary>
		/// Prepares this command for execution.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="connection">The <see cref="SqlConnection"/>.</param>
		/// <param name="query">The <see cref="Query"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="connection"/> or <paramref name="query"/> is null.</exception>
		public void Prepare(IMansionContext context, SqlConnection connection, Query query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (query == null)
				throw new ArgumentNullException("query");
			CheckDisposed();

			// find the root type for this query
			var rootType = query.TypeHints.FindCommonAncestor(context);

			// get the schema of the root type
			Schema = SchemaProvider.Resolve(context, rootType);

			// create the command
			QueryBuilder = new SqlStringBuilder(Schema.RootTable);
			Command = connection.CreateCommand();
			Command.CommandType = CommandType.Text;

			// loop over all the query components and convert them
			foreach (var component in query.Components)
				converters.Elect(context, component).Convert(context, component, this);

			// safe the query text
			Command.CommandText = QueryBuilder.ToString();
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes this command.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the record when found, otherwise null.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="Prepare"/> is not called.</exception>
		public IPropertyBag ExecuteSingle(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

			// check if the command is initialized properly
			if (Command == null)
				throw new InvalidOperationException("The command is not prepared. Call the prepare method before calling execute.");

			// get the column mappers in prioritized order
			var recordMappers = QueryBuilder.Tables.SelectMany(table => table.GetRecordMappers(context)).OrderByPriority();

			// execute the command
			using (var reader = Command.ExecuteReader(CommandBehavior.SingleRow))
			{
				// first check if there is a result
				if (!reader.Read())
					return null;

				// map to node.
				return Map(context, recordMappers, reader);
			}
		}
		/// <summary>
		/// Executes this command.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the resulting <see cref="Dataset"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="Prepare"/> is not called.</exception>
		public Dataset Execute(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

			// check if the command is initialized properly
			if (Command == null)
				throw new InvalidOperationException("The command is not prepared. Call the prepare method before calling execute.");

			// get the column mappers in prioritized order
			var recordMappers = QueryBuilder.Tables.SelectMany(table => table.GetRecordMappers(context)).OrderByPriority().ToList();

			// create the dataset
			var dataset = new Dataset();

			// execute the command
			using (var reader = Command.ExecuteReader(CommandBehavior.Default))
			{
				// read the input
				while (reader.Read())
					dataset.AddRow(Map(context, recordMappers, reader));
			}

			// return the result
			return dataset;
		}
		#endregion
		#region Map Methods
		/// <summary>
		/// Maps the <paramref name="record"/> into a <see cref="IPropertyBag"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="recordMappers">The <see cref="IRecordMapper"/>s.</param>
		/// <param name="record">The <see cref="IDataRecord"/> which to map.</param>
		/// <returns>Returns the mapped record.</returns>
		private static IPropertyBag Map(IMansionContext context, IEnumerable<IRecordMapper> recordMappers, IDataRecord record)
		{
			//  create a new property bag
			var properties = new PropertyBag();

			// loop over all the mappers and map the result
			foreach (var recordMapper in recordMappers)
				recordMapper.Map(context, record, properties);

			// return the properties
			return properties;
		}
		#endregion
		#region Private Fields
		private readonly IEnumerable<IQueryComponentConverter> converters;
		#endregion
	}
}