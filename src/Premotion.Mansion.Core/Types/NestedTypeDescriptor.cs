using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Represents a <see cref="TypeDescriptor"/> which can contain nested <see cref="TypeDescriptor"/>s.
	/// </summary>
	public abstract class NestedTypeDescriptor : TypeDescriptor, IDescriptee
	{
		#region IDescriptee Members
		/// <summary>
		/// Gets all the <see cref="IDescriptor"/>s of the specified type <typeparamref name="TDescriptor"/>.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of descriptor.</typeparam>
		/// <returns>Returnt an enumerable of <see cref="IDescriptor"/> of type <typeparamref name="TDescriptor"/>.</returns>
		public IEnumerable<TDescriptor> GetDescriptors<TDescriptor>() where TDescriptor : class, IDescriptor
		{
			// select the descriptors
			return from candidate in descriptors
			       where typeof (TDescriptor).IsAssignableFrom(candidate.GetType())
			       select (TDescriptor) candidate;
		}
		/// <summary>
		/// Gets the descriptor of the specified type. When no descriptor is found the default value of <typeparamref name="TDescriptor"/> is returned.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of descriptor.</typeparam>
		/// <param name="descriptor">The found descriptor.</param>
		/// <returns>Returns true when the descriptor is found, otherwise false.</returns>
		public bool TryGetDescriptor<TDescriptor>(out TDescriptor descriptor) where TDescriptor : class, IDescriptor
		{
			// select the descriptor
			descriptor = (from candidate in descriptors
			              where typeof (TDescriptor).IsAssignableFrom(candidate.GetType())
			              select (TDescriptor) candidate).SingleOrDefault();

			// check for success
			return descriptor != null;
		}
		#endregion
		#region Descriptor Methods
		/// <summary>
		/// Adds a descriptor.
		/// </summary>
		/// <param name="descriptor">The descriptor which to add.</param>
		public void AddDescriptor(IDescriptor descriptor)
		{
			// validate arguments
			if (descriptor == null)
				throw new ArgumentNullException("descriptor");

			descriptors.Add(descriptor);
		}
		#endregion
		#region Private Fields
		private readonly List<IDescriptor> descriptors = new List<IDescriptor>();
		#endregion
	}
}