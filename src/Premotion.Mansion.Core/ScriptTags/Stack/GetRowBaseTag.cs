using System;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Provides base functionality for row tags.
	/// </summary>
	public abstract class GetRowBaseTag : ScriptTag
	{
		#region Execute Methods
		/// <summary>
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(MansionContext context)
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
			var result = Get(context, attributes);

			// check if the node was found
			if (result != null)
			{
				// push the node to the stack in the target dataspace
				using (context.Stack.Push(target, result, global))
					ExecuteChildTags(context);
			}
			else
			{
				// execute not found tag
				NotFoundTag notFoundTag;
				if (TryGetAlternativeChildTag(out notFoundTag))
					notFoundTag.Execute(context);
			}
		}
		/// <summary>
		/// Gets the row.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected abstract IPropertyBag Get(MansionContext context, IPropertyBag attributes);
		#endregion
	}
}