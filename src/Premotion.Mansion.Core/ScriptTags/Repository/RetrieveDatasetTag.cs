using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves a <see cref="Dataset"/> from the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "retrieveDataset")]
	public class RetrieveDatasetTag : RetrieveDatasetBaseTag
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="parser"></param>
		public RetrieveDatasetTag(IQueryParser parser)
		{
			// validate arguments
			if (parser == null)
				throw new ArgumentNullException("parser");

			// set value
			this.parser = parser;
		}
		#endregion
		#region Overrides of RetrieveDatasetBaseTag
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository)
		{
			// parse the query
			var query = parser.Parse(context, arguments);

			// execute and return the result
			return repository.Retrieve(context, query);
		}
		#endregion
		#region Private Fields
		private readonly IQueryParser parser;
		#endregion
	}
}