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
			TaskType = Properties.Get<Type>(context, "type");
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the listener type.
		/// </summary>
		public Type TaskType { get; private set; }
		#endregion
	}
}