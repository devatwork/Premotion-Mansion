using System.Collections.Generic;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Represents a type.
	/// </summary>
	public interface ITypeDefinition : IDescriptee
	{
		#region Property Methods
		/// <summary>
		/// Gets a property by it's name.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns>Returns the property.</returns>
		/// <exception cref="PropertyNotFoundException">Thrown when a property with the specified name can not be found.</exception>
		IPropertyDefinition GetProperty(string name);
		#endregion
		#region Relational Methods
		/// <summary>
		/// Checks whether this type inherits from <paramref name="targetType"/>.
		/// </summary>
		/// <param name="targetType">The parent type.</param>
		/// <returns>Returns true when this type does intherit from <paramref name="targetType"/>, otherwise false.</returns>
		bool IsAssignable(ITypeDefinition targetType);
		/// <summary>
		/// Gets the types inheriting from this type.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <returns>Returns the inheriting types.</returns>
		IEnumerable<ITypeDefinition> GetInheritingTypes(IContext context);
		#endregion
		#region Relational Properties
		/// <summary>
		/// Gets a flag indicating whether this type has a parent.
		/// </summary>
		bool HasParent { get; }
		/// <summary>
		/// Gets the parent of this type definition.
		/// </summary>
		ITypeDefinition Parent { get; }
		/// <summary>
		/// Gets the hierarchy of this type. Top down.
		/// </summary>
		IEnumerable<ITypeDefinition> Hierarchy { get; }
		/// <summary>
		/// Gets the reverse hierarchy of this type. Bottom up.
		/// </summary>
		IEnumerable<ITypeDefinition> HierarchyReverse { get; }
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this type definition.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Gets the properties of this type.
		/// </summary>
		IEnumerable<IPropertyDefinition> Properties { get; }
		#endregion
	}
}