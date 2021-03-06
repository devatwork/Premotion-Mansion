﻿using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Edits an existing node.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "editNode")]
	public class EditNodeTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get arguments
			var node = GetRequiredAttribute<Node>(context, "source");

			// get the properties which to edit
			var editProperties = new PropertyBag(GetAttributes(context));
			editProperties.Remove("source");
			using (context.Stack.Push("EditProperties", editProperties))
				ExecuteChildTags(context);

			// store the updated node
			context.Repository.UpdateNode(context, node, editProperties);
		}
	}
}