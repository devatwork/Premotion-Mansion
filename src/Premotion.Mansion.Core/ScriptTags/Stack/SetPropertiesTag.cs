using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Opens a template.
	/// </summary>
	[Named(Constants.NamespaceUri, "setProperties")]
	public class SetPropertiesTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the attribute
			var attributes = GetAttributes(context);
			string dataspaceName;
			if (!attributes.TryGetAndRemove(context, "dataspaceName", out dataspaceName) || string.IsNullOrEmpty(dataspaceName))
				throw new InvalidOperationException("The attribute dataspaceName must be non null and non empty");
			bool global;
			if (!attributes.TryGetAndRemove(context, "global", out global))
				global = false;

			// get the value from the stack
			IPropertyBag dataspace;
			if (!context.Stack.TryPeek(dataspaceName, out dataspace))
			{
				dataspace = new PropertyBag();
				using (context.Stack.Push(dataspaceName, dataspace, global))
					SetProperties(context, dataspace, attributes);
			}
			else
				SetProperties(context, dataspace, attributes);
		}
		/// <summary>
		/// Sets the properties.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dataspace"></param>
		/// <param name="attributes"></param>
		private void SetProperties(MansionContext context, IPropertyBag dataspace, IEnumerable<KeyValuePair<string, object>> attributes)
		{
			// copy the attributes
			dataspace.Merge(attributes);

			// execute children
			ExecuteChildTags(context);
		}
	}
}