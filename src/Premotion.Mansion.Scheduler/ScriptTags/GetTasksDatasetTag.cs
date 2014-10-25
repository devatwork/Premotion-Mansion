using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Scheduler.ScriptTags
{
	/// <summary>
	/// 
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "getTasksDataset")]
	public class GetTaskDatasetTag : GetDatasetBaseTag
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="attributes"></param>
		/// <returns></returns>
		protected override Dataset Get(IMansionContext context, IPropertyBag attributes)
		{
			var jobNode = GetRequiredAttribute<Node>(context, "source");
			var dataset = new Dataset();
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();
			var type = typeService.Load(context, jobNode.Type);
			var tasks = type.GetDescriptors<RegisterTaskDescriptor>().Select(descriptor => descriptor.TaskType);

			foreach (var task in tasks)
			{
				var taskProperties = new PropertyBag
				{
					{ "name", task.Name},
					{ "type", task.ToString()}
				};
				dataset.AddRow(taskProperties);
			}
			
			return dataset;
		}
	}
}