using System;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Implements extensions for <see cref="ITypeDefinition"/>.
	/// </summary>
	public static class Extensions
	{
		#region ITypeDefinition Extensions
		/// <summary>
		/// Tries to find the <typeparamref name="TDescriptor"/> in the reverse type hierarchy of <paramref name="typeDefinition"/>.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of <see cref="IDescriptor"/>.</typeparam>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> for which to get the <paramref name="descriptor"/>.</param>
		/// <param name="descriptor">The instance of <typeparamref name="TDescriptor"/>.</param>
		/// <returns>Returns true when found, otherwise false.</returns>
		public static bool TryFindDescriptorInHierarchy<TDescriptor>(this ITypeDefinition typeDefinition, out TDescriptor descriptor) where TDescriptor : class, IDescriptor
		{
			// validate arguments
			if (typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");

			// loop through all the types in the hierarchy
			foreach (var type in typeDefinition.HierarchyReverse)
			{
				if (type.TryGetDescriptor(out descriptor))
					return true;
			}

			// descriptor not found
			descriptor = default(TDescriptor);
			return false;
		}
		/// <summary>
		/// Tries to find the <typeparamref name="TDescriptor"/> in the reverse type hierarchy of <paramref name="typeDefinition"/>.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of <see cref="IDescriptor"/>.</typeparam>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> for which to get the <paramref name="descriptor"/>.</param>
		/// <param name="predicate">The <see cref="Predicate{TDescriptor}"/> which can filter the returned descriptor.</param>
		/// <param name="descriptor">The instance of <typeparamref name="TDescriptor"/>.</param>
		/// <returns>Returns true when found, otherwise false.</returns>
		public static bool TryFindDescriptorInHierarchy<TDescriptor>(this ITypeDefinition typeDefinition, Predicate<TDescriptor> predicate, out TDescriptor descriptor) where TDescriptor : class, IDescriptor
		{
			// validate arguments
			if (typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			// loop through all the types in the hierarchy
			foreach (var type in typeDefinition.HierarchyReverse)
			{
				if (type.TryGetDescriptor(out descriptor) && predicate(descriptor))
					return true;
			}

			// descriptor not found
			descriptor = default(TDescriptor);
			return false;
		}
		#endregion
	}
}