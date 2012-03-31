using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Provides base functionality for building nodes queries
	/// </summary>
	public abstract class GetDatasetBaseTag : ScriptTag
	{
		#region Execute Methods
		/// <summary>
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get all attributes
			var attributes = GetAttributes(context);

			// get the target attribute
			string target;
			if (!attributes.TryGetAndRemove(context, "target", out target) || string.IsNullOrEmpty(target))
				throw new InvalidOperationException("The target attribute is manditory");
			bool global;
			if (!attributes.TryGetAndRemove(context, "global", out global))
				global = false;

			// get the result
			var results = Get(context, attributes);

			// push the node to the stack in the target dataspace
			using (context.Stack.Push(target, results, global))
			{
				// check if the node was found
				if (results != null && results.RowCount > 0)
					ExecuteChildTags(context);
				else
				{
					// execute not found tag
					NotFoundTag notFoundTag;
					if (TryGetAlternativeChildTag(out notFoundTag))
						notFoundTag.Execute(context);
				}
			}
		}
		/// <summary>
		/// Gets the dataset.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected abstract Dataset Get(IMansionContext context, IPropertyBag attributes);
		#endregion
	}
}