using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Queries
{
	/// <summary>
	/// A query that matches documents matching boolean combinations of other queries. The bool query maps to Lucene BooleanQuery. It is built using one or more boolean clauses, each clause with a typed occurrence.
	/// </summary>
	[JsonConverter(typeof (BoolQueryConverter))]
	public class BoolQuery : BaseQuery
	{
		#region Nested type: BoolQueryConverter
		/// <summary>
		/// Converts <see cref="BoolQuery"/>.
		/// </summary>
		private class BoolQueryConverter : BaseWriteConverter<BoolQuery>
		{
			#region Overrides of BaseWriteConverter<BoolQuery>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, BoolQuery value, JsonSerializer serializer)
			{
				writer.WriteStartObject(); // root
				writer.WritePropertyName("bool");
				writer.WriteStartObject(); // bool

				// write must
				if (value.must.Count > 0)
				{
					writer.WritePropertyName("must");
					writer.WriteStartArray(); // must
					foreach (var query in value.must)
						serializer.Serialize(writer, query);
					writer.WriteEndArray(); // /must
				}

				// write should
				if (value.should.Count > 0)
				{
					writer.WritePropertyName("should");
					writer.WriteStartArray(); // should
					foreach (var query in value.should)
						serializer.Serialize(writer, query);
					writer.WriteEndArray(); // /should
				}

				// write should
				if (value.mustNot.Count > 0)
				{
					writer.WritePropertyName("must_not");
					writer.WriteStartArray(); // must_not
					foreach (var query in value.mustNot)
						serializer.Serialize(writer, query);
					writer.WriteEndArray(); // /must_not
				}

				writer.WriteEndObject(); // /bool
				writer.WriteEndObject(); // root
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a bool query.
		/// </summary>
		/// <param name="must">The clauses (query) must appear in matching documents.</param>
		/// <param name="should">The clauses (query) should appear in the matching document. A boolean query with no must clauses, one or more should clauses must match a document.</param>
		/// <param name="mustNot">The clauses (query) must not appear in the matching documents.</param>
		public BoolQuery(IEnumerable<BaseQuery> must = null, IEnumerable<BaseQuery> should = null, IEnumerable<BaseQuery> mustNot = null)
		{
			// set the values
			this.must = must != null ? new Queue<BaseQuery>(must) : new Queue<BaseQuery>();
			this.should = should != null ? new Queue<BaseQuery>(should) : new Queue<BaseQuery>();
			this.mustNot = mustNot != null ? new Queue<BaseQuery>(mustNot) : new Queue<BaseQuery>();
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds the given <paramref name="queries"/> as must queries.
		/// </summary>
		/// <param name="queries">The <see cref="BaseQuery"/>s which to add.</param>
		/// <returns>Returns this for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="queries"/> is null.</exception>
		public BoolQuery AddMust(params BaseQuery[] queries)
		{
			return AddMust((IEnumerable<BaseQuery>) queries);
		}
		/// <summary>
		/// Adds the given <paramref name="queries"/> as must queries.
		/// </summary>
		/// <param name="queries">The <see cref="BaseQuery"/>s which to add.</param>
		/// <returns>Returns this for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="queries"/> is null.</exception>
		public BoolQuery AddMust(IEnumerable<BaseQuery> queries)
		{
			// validate arguments
			if (queries == null)
				throw new ArgumentNullException("queries");

			// add the queries
			foreach (var query in queries)
				must.Enqueue(query);
			return this;
		}
		/// <summary>
		/// Adds the given <paramref name="queries"/> as should queries.
		/// </summary>
		/// <param name="queries">The <see cref="BaseQuery"/>s which to add.</param>
		/// <returns>Returns this for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="queries"/> is null.</exception>
		public BoolQuery AddShould(params BaseQuery[] queries)
		{
			return AddShould((IEnumerable<BaseQuery>) queries);
		}
		/// <summary>
		/// Adds the given <paramref name="queries"/> as should queries.
		/// </summary>
		/// <param name="queries">The <see cref="BaseQuery"/>s which to add.</param>
		/// <returns>Returns this for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="queries"/> is null.</exception>
		public BoolQuery AddShould(IEnumerable<BaseQuery> queries)
		{
			// validate arguments
			if (queries == null)
				throw new ArgumentNullException("queries");

			// add the queries
			foreach (var query in queries)
				should.Enqueue(query);
			return this;
		}
		/// <summary>
		/// Adds the given <paramref name="queries"/> as must_not queries.
		/// </summary>
		/// <param name="queries">The <see cref="BaseQuery"/>s which to add.</param>
		/// <returns>Returns this for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="queries"/> is null.</exception>
		public BoolQuery AddMustNot(params BaseQuery[] queries)
		{
			return AddMustNot((IEnumerable<BaseQuery>) queries);
		}
		/// <summary>
		/// Adds the given <paramref name="queries"/> as must_not queries.
		/// </summary>
		/// <param name="queries">The <see cref="BaseQuery"/>s which to add.</param>
		/// <returns>Returns this for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="queries"/> is null.</exception>
		public BoolQuery AddMustNot(IEnumerable<BaseQuery> queries)
		{
			// validate arguments
			if (queries == null)
				throw new ArgumentNullException("queries");

			// add the queries
			foreach (var query in queries)
				mustNot.Enqueue(query);
			return this;
		}
		#endregion
		#region Private Fields
		private readonly Queue<BaseQuery> must = new Queue<BaseQuery>();
		private readonly Queue<BaseQuery> mustNot = new Queue<BaseQuery>();
		private readonly Queue<BaseQuery> should = new Queue<BaseQuery>();
		#endregion
	}
}