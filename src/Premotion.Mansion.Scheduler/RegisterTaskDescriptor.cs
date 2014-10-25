using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Scheduler
{
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "registerTask")]
	public class RegisterTaskDescriptor : TypeDescriptor
	{
		#region Overrides of TypeDescriptor
		/// <summary>Initializes this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoInitialize(IMansionContext context)
		{
			TaskLabel = Properties.Get<String>(context, "label");
			TaskType = Properties.Get<Type>(context, "type");
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the task type.
		/// </summary>
		public Type TaskType { get; private set; }
		public String TaskLabel { get; private set; }
		#endregion
	}
}