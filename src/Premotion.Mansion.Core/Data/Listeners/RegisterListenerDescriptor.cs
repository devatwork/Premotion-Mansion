using System;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Listeners
{
	/// <summary>
	/// Implements the descriptor which adds a <see cref="NodeListener"/> for a particular <see cref="ITypeDefinition"/>.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "register")]
	public class RegisterListenerDescriptor : TypeDescriptor
	{
		#region Overrides of TypeDescriptor
		/// <summary>
		/// Initializes this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoInitialize(IMansionContext context)
		{
			ListenerType = Properties.Get<Type>(context, "type");
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the listener type.
		/// </summary>
		public Type ListenerType { get; private set; }
		#endregion
	}
}