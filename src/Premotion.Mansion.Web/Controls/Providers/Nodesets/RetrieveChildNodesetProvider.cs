using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
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
			#region Overrides of DataProviderFactoryTag
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override NodesetProvider Create(IMansionWebContext context)
			{
				return new RetrieveChildNodesetProvider(GetAttributes(context));
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a retrieve child nodeset provider.
		/// </summary>
		/// <param name="queryParameters">The parameters for the query.</param>
		private RetrieveChildNodesetProvider(IPropertyBag queryParameters)
		{
			// validate arguments
			if (queryParameters == null)
				throw new ArgumentNullException("queryParameters");

			this.queryParameters = queryParameters;
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

			// parse the query
			var query = repository.ParseQuery(context, queryParameters);

			// make sure a child of clause is specified
			if (!query.HasClause<ChildOfClause>())
				throw new InvalidOperationException("The parent node was not specified.");

			// execute the query
			return repository.Retrieve(context, query);
		}
		#endregion
		#region Private Fields
		private readonly IPropertyBag queryParameters;
		#endregion
	}
}