using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Providers.Nodesets
{
	/// <summary>
	/// Retrieves a set of child nodes.
	/// </summary>
	public class RetrieveChildNodesetProvider : NodesetProvider
	{
		#region Nested type: RetrieveChildNodesetProviderFactoryTag
		/// <summary>
		/// Creates <see cref="RetrieveChildNodesetProvider"/>s.
		/// </summary>
		[ScriptTag(Constants.DataProviderTagNamespaceUri, "retrieveChildNodesetProvider")]
		public class RetrieveChildNodesetProviderFactoryTag : DatasetProviderFactoryTag<NodesetProvider>
		{
			#region Constructors
			/// <summary>
			/// </summary>
			/// <param name="parser"></param>
			/// <exception cref="ArgumentNullException"></exception>
			public RetrieveChildNodesetProviderFactoryTag(IQueryParser parser)
			{
				// validate arguments
				if (parser == null)
					throw new ArgumentNullException("parser");

				// set value
				this.parser = parser;
			}
			#endregion
			#region Overrides of DataProviderFactoryTag
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override NodesetProvider Create(IMansionWebContext context)
			{
				// parse the query
				var query = parser.Parse(context, GetAttributes(context));

				// make sure a child of clause is specified
				if (!query.HasSpecification<ChildOfSpecification>())
					throw new InvalidOperationException("The parent node was not specified.");

				// return the provider
				return new RetrieveChildNodesetProvider(query);
			}
			#endregion
			#region Private Fields
			private readonly IQueryParser parser;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a retrieve child nodeset provider.
		/// </summary>
		/// <param name="query">The parameters for the query.</param>
		private RetrieveChildNodesetProvider(Query query)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");

			this.query = query;
		}
		#endregion
		#region Overrides of DataProvider<Nodeset>
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the retrieve data.</returns>
		protected override Nodeset DoRetrieve(IMansionContext context)
		{
			// get the repository
			var repository = context.Repository;

			// execute the query
			return repository.RetrieveNodeset(context, query);
		}
		#endregion
		#region Private Fields
		private readonly Query query;
		#endregion
	}
}