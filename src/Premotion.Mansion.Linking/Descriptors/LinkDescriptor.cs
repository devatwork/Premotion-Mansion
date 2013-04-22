using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking.Descriptors
{
	/// <summary>
	/// Describes a relation this type can have.
	/// </summary>
	public abstract class LinkDescriptor : TypeDescriptor
	{
		/// <summary>
		/// Gets the <see cref="LinkDefinition"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="LinkDefinition"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the arugments is null.</exception>
		public LinkDefinition GetDefinition(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the name
			var name = Properties.Get<string>(context, "name");

			// invoke template method
			return GetDefinition(context, name);
		}
		/// <summary>
		/// Gets the <see cref="LinkDefinition"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="name">The name of the link definition.</param>
		/// <returns>Returns the <see cref="LinkDefinition"/>.</returns>
		protected abstract LinkDefinition GetDefinition(IMansionContext context, string name);
	}
}