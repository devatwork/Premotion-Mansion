using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Reflection
{
	/// <summary>
	/// Represents the public interface of a service knowing about <see cref="Type"/>s.
	/// </summary>
	public interface ITypeDirectoryService : IService
	{
		#region Lookup Methods
		///<summary>
		/// Looks up all the types matching the types.
		///</summary>
		///<typeparam name="TType">The <see cref="Type"/> which to get.</typeparam>
		///<returns>Returns all the matching types.</returns>
		IEnumerable<Type> Lookup<TType>();
		///<summary>
		/// Looks up all the types matching the types.
		///</summary>
		///<typeparam name="TType">The <see cref="Type"/> which to get.</typeparam>
		/// <param name="namespaceUri">The namespace in which the types live.</param>
		///<returns>Returns all the matching types.</returns>
		IEnumerable<Type> Lookup<TType>(string namespaceUri);
		///<summary>
		/// Tries to look up a type by it's <paramref name="namespaceUri"/> and <paramref name="name"/>.
		///</summary>
		///<typeparam name="TType">The <see cref="Type"/> which to get.</typeparam>
		/// <param name="namespaceUri">The namespace in which the types live.</param>
		/// <param name="name">The name of the type.</param>
		/// <param name="type">The type.</param>
		///<returns>Returns true when a matching type was found otherwise false.</returns>
		bool TryLookupSingle<TType>(string namespaceUri, string name, out Type type);
		#endregion
	}
}