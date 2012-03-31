using System.Collections.Generic;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Provides methods for type manipulation.
	/// </summary>
	public interface ITypeService
	{
		#region Load Methods
		/// <summary>
		/// Loads the type with the specified name.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeName">The name of the type which to load.</param>
		/// <returns>Returns the loaded type.</returns>
		/// <exception cref="TypeNotFoundException">Thrown when the type with the specified name can not be found.</exception>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		ITypeDefinition Load(IMansionContext context, string typeName);
		/// <summary>
		/// Tries to load the type with name <paramref name="typeName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeName">The name of the type which to load.</param>
		/// <param name="typeDefinition">The loaded type definition.</param>
		/// <returns>Returns true when the type is loaded, otherwise false.</returns>
		bool TryLoad(IMansionContext context, string typeName, out ITypeDefinition typeDefinition);
		/// <summary>
		/// Gets the type which represents the root of the type hierarchy.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		ITypeDefinition LoadRoot(IMansionContext context);
		/// <summary>
		/// Gets all the type definitions in this application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns all the types.</returns>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		IEnumerable<ITypeDefinition> LoadAll(IMansionContext context);
		#endregion
	}
}