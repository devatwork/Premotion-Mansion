using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Creates a new <see cref="Record"/> in the topmost repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "createRecord")]
	public class CreateRecordTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the properties which to edit
			var properties = new PropertyBag(GetAttributes(context));
			properties.Remove("target");
			properties.Remove("global");
			using (context.Stack.Push("Properties", properties))
				ExecuteChildTags(context);

			// store the updated node
			var record = context.Repository.Create(context, properties);

			// push the new node to the stack
			context.Stack.Push(GetRequiredAttribute<string>(context, "target"), record, GetAttribute<bool>(context, "global")).Dispose();
		}
	}
}