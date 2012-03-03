using System.Collections.Generic;

namespace Premotion.Mansion.Core.Patterns.Descriptors
{
	/// <summary>
	/// Represents a described object.
	/// </summary>
	public interface IDescriptee
	{
		#region Descriptor Methods
		/// <summary>
		/// Gets all the <see cref="IDescriptor"/>s of the specified type <typeparamref name="TDescriptor"/>.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of descriptor.</typeparam>
		/// <returns>Returnt an enumerable of <see cref="IDescriptor"/> of type <typeparamref name="TDescriptor"/>.</returns>
		IEnumerable<TDescriptor> GetDescriptors<TDescriptor>() where TDescriptor : class, IDescriptor;
		/// <summary>
		/// Gets the descriptor of the specified type. When no descriptor is found the default value of <typeparamref name="TDescriptor"/> is returned.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of descriptor.</typeparam>
		/// <param name="descriptor">The found descriptor.</param>
		/// <returns>Returns true when the descriptor is found, otherwise false.</returns>
		bool TryGetDescriptor<TDescriptor>(out TDescriptor descriptor) where TDescriptor : class, IDescriptor;
		#endregion
	}
}