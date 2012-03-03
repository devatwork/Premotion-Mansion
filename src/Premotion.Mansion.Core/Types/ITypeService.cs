using System.Collections.Generic;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Provides methods for type manipulation.
	/// </summary>
	public interface ITypeService : IService
	{
		#region Load Methods
		/// <summary>
		/// Loads the type with the specified name.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="typeName">The name of the type which to load.</param>
		/// <returns>Returns the loaded type.</returns>
		/// <exception cref="TypeNotFoundException">Thrown when the type with the specified name can not be found.</exception>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		ITypeDefinition Load(IContext context, string typeName);
		/// <summary>
		/// Tries to load the type with name <paramref name="typeName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="typeName">The name of the type which to load.</param>
		/// <param name="typeDefinition">The loaded type definition.</param>
		/// <returns>Returns true when the type is loaded, otherwise false.</returns>
		bool TryLoad(IContext context, string typeName, out ITypeDefinition typeDefinition);
		/// <summary>
		/// Gets the type which represents the root of the type hierarchy.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		ITypeDefinition LoadRoot(IContext context);
		/// <summary>
		/// Gets all the type definitions in this application.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <returns>Returns all the types.</returns>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		IEnumerable<ITypeDefinition> LoadAll(IContext context);
		#endregion
	}
}