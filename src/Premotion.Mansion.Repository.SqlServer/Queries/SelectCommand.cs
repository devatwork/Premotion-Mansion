using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Patterns.Prioritized;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Repository.SqlServer.Queries.Converters;
using Premotion.Mansion.Repository.SqlServer.Queries.Mappers;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Represents an executable select command.
	/// </summary>
	public abstract class SelectCommand<TSet, TRow> : DisposableBase where TSet : Dataset where TRow : IPropertyBag
	{
		#region Constructors
		/// <summary>
		/// Constructs an <see cref="SelectCommand"/>.
		/// </summary>
		/// <param name="converters">The <see cref="IQueryComponentConverter"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="converters"/> is null.</exception>
		protected SelectCommand(IEnumerable<IQueryComponentConverter> converters)
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

			// remember value
			originalQuery = query;

			// find the root type for this query
			var rootType = query.TypeHints.FindCommonAncestor(context);

			// get the schema of the root type
			var schema = Resolver.Resolve(context, rootType);

			// create the context
			commandContext = new QueryCommandContext(schema, connection.CreateCommand());

			// loop over all the query components and convert them
			foreach (var component in query.Components)
				converters.Elect(context, component).Convert(context, component, commandContext);
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
		public TRow ExecuteSingle(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

			// check if the command is initialized properly
			if (commandContext.Command == null)
				throw new InvalidOperationException("The command is not prepared. Call the prepare method before calling execute.");
			commandContext.Command.CommandText = commandContext.QueryBuilder.ToSingleNodeQuery();

			// get the column mappers in prioritized order
			var recordMappers = commandContext.Schema.GetRecordMappers(context).OrderByPriority();

			// execute the command
			using (var reader = commandContext.Command.ExecuteReader(CommandBehavior.SingleRow))
			{
				// first check if there is a result
				if (!reader.Read())
					return default(TRow);

				// map to node.
				return Map(context, recordMappers, new Record(reader));
			}
		}
		/// <summary>
		/// Executes this command.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the resulting <see cref="Dataset"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="Prepare"/> is not called.</exception>
		public TSet Execute(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

			// check if the command is initialized properly
			if (commandContext.Command == null)
				throw new InvalidOperationException("The command is not prepared. Call the prepare method before calling execute.");
			commandContext.Command.CommandText = commandContext.QueryBuilder.ToString();

			// get the column mappers in prioritized order
			var recordMappers = commandContext.Schema.GetRecordMappers(context).OrderByPriority().ToList();

			// execute the command
			using (var reader = commandContext.Command.ExecuteReader(CommandBehavior.Default))
			{
				// read the input
				var rows = new List<TRow>();
				while (reader.Read())
					rows.Add(Map(context, recordMappers, new Record(reader)));

				// map the set properties
				var setProperties = MapSetProperties(reader);

				// create the dataset
				var dataset = CreateSet(context, rows, setProperties);

				// map the result of additional queries
				while (reader.NextResult())
				{
					// get the mapper from the query
					commandContext.QueryBuilder.GetAdditionalQueryResultMapper()(dataset, reader);
				}

				// return the result
				return dataset;
			}
		}
		#endregion
		#region Map Methods
		/// <summary>
		/// Maps the <paramref name="record"/> into a <see cref="IPropertyBag"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="recordMappers">The <see cref="IRecordMapper"/>s.</param>
		/// <param name="record">The <see cref="Record"/> which to map.</param>
		/// <returns>Returns the mapped record.</returns>
		private TRow Map(IMansionContext context, IEnumerable<IRecordMapper> recordMappers, Record record)
		{
			//  create a new property bag
			var properties = CreateRow();

			// loop over all the mappers and map the result
			foreach (var recordMapper in recordMappers)
				recordMapper.Map(context, record, properties);

			// initialize the row
			Initialize(context, properties);

			// return the properties
			return properties;
		}
		/// <summary>
		/// Maps the set properties.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> reader.</param>
		/// <returns>Returns the mapped properties.</returns>
		private IPropertyBag MapSetProperties(IDataReader reader)
		{
			var properties = new PropertyBag();

			// check for sort
			SortQueryComponent sortQueryComponent;
			if (originalQuery.TryGetComponent(out sortQueryComponent))
				properties.Set("sort", string.Join(", ", sortQueryComponent.Sorts.Select(sort => sort.PropertyName + " " + (sort.Ascending ? "asc" : "desc"))));

			// check for paging
			PagingQueryComponent pagingQueryComponent;
			if (originalQuery.TryGetComponent(out pagingQueryComponent))
			{
				properties.Set("pageNumber", pagingQueryComponent.PageNumber);
				properties.Set("pageSize", pagingQueryComponent.PageSize);
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
		#endregion
		#region Create Methods
		/// <summary>
		/// Creates an set of type <typeparamref name="TSet"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="rows">The <typeparamref name="TRow"/>s.</param>
		/// <param name="setProperties">The properties of the set.</param>
		/// <returns>Returns the created <typeparamref name="TSet"/>.</returns>
		protected abstract TSet CreateSet(IMansionContext context, IEnumerable<TRow> rows, IPropertyBag setProperties);
		/// <summary>
		/// Creates a new <typeparamref name="TRow"/>.
		/// </summary>
		/// <returns>Returns the created <typeparamref name="TRow"/>.</returns>
		protected abstract TRow CreateRow();
		/// <summary>
		/// Initializes the given row.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="row">The <typeparamref name="TRow"/> which to intialize.</param>
		protected virtual void Initialize(IMansionContext context, TRow row)
		{
			// do nothing
		}
		#endregion
		#region Overrides of DisposableBase
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
			if (commandContext != null)
				commandContext.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly IEnumerable<IQueryComponentConverter> converters;
		private QueryCommandContext commandContext;
		private Query originalQuery;
		#endregion
	}
	/// <summary>
	/// Represents an executable select command.
	/// </summary>
	public class SelectCommand : SelectCommand<Dataset, IPropertyBag>
	{
		#region Constructors
		/// <summary>
		/// Constructs an <see cref="SelectCommand"/>.
		/// </summary>
		/// <param name="converters">The <see cref="IQueryComponentConverter"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="converters"/> is null.</exception>
		public SelectCommand(IEnumerable<IQueryComponentConverter> converters) : base(converters)
		{
		}
		#endregion
		#region Overrides of SelectCommand<Dataset,IPropertyBag>
		/// <summary>
		/// Creates a set.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="rows">The rows.</param>
		/// <param name="setProperties">The properties of the set.</param>
		/// <returns>Returns the created set.</returns>
		protected override Dataset CreateSet(IMansionContext context, IEnumerable<IPropertyBag> rows, IPropertyBag setProperties)
		{
			return new Dataset(context, setProperties, rows);
		}
		/// <summary>
		/// Creates a new row.
		/// </summary>
		/// <returns>Returns the created row.</returns>
		protected override IPropertyBag CreateRow()
		{
			return new PropertyBag();
		}
		#endregion
	}
}