using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Stack;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Provides base functionality for building nodes queries
	/// </summary>
	public abstract class RetrieveNodesetBaseTag : GetDatasetBaseTag
	{
		#region Execute Methods
		/// <summary>
		/// Gets the dataset.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override Dataset Get(IMansionContext context, IPropertyBag attributes)
		{
			// get the result
			return Retrieve(context, attributes, context.Repository);
		}
		/// <summary>
		/// Builds and executes the query.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="arguments">The arguments from which to build the query.</param>
		/// <param name="repository"></param>
		/// <returns>Returns the result.</returns>
		protected abstract Nodeset Retrieve(IMansionContext context, IPropertyBag arguments, IRepository repository);
		#endregion
	}
}