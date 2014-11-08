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
			TaskId = Properties.Get<int>(context, "id");
			TaskLabel = Properties.Get<String>(context, "label");
			TaskType = Properties.Get<Type>(context, "type");
			TaskWaitsFor = Properties.Get<String>(context, "waitsFor", null);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Task identifier
		/// </summary>
		public int TaskId { get; private set; }
		/// <summary>
		/// Gets the task type.
		/// </summary>
		public Type TaskType { get; private set; }
		/// <summary>
		/// Gets the task friendly name.
		/// </summary>
		public String TaskLabel { get; private set; }
		/// <summary>
		/// Task should wait on these id's before executing.
		/// </summary>
		public String TaskWaitsFor { get; private set; }
		#endregion
	}
}