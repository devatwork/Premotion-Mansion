using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Listeners
{
	/// <summary>
	/// Implements the descriptor which adds a <see cref="NodeListener"/> for a particular <see cref="ITypeDefinition"/>.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "register")]
	public class RegisterListenerDescriptor : TypeDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public RegisterListenerDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the listener type.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <returns>Returns the listener <see cref="Type"/>.</returns>
		public Type GetListenerType(IContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return Properties.Get<Type>(context, "type");
		}
		#endregion
	}
}