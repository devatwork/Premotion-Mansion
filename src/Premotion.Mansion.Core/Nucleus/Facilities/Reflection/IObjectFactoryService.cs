using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Reflection
{
	/// <summary>
	/// Represents the public interface of a service which can instantiate types.
	/// </summary>
	public interface IObjectFactoryService : IService
	{
		#region Create Methods
		/// <summary>
		/// Creates instances of the specified <paramref name="types"/>.
		/// </summary>
		/// <typeparam name="TType">The type of object.</typeparam>
		/// <param name="types">The <see cref="Type"/>s which to create instances for.</param>
		/// <param name="constructorParameters">The constructor parameters.</param>
		/// <returns>Returns the created instance.</returns>
		IEnumerable<TType> Create<TType>(IEnumerable<Type> types, params object[] constructorParameters);
		/// <summary>
		/// Creates an instance of the specified <paramref name="type"/>.
		/// </summary>
		/// <typeparam name="TType">The type of object.</typeparam>
		/// <param name="type">The <see cref="Type"/> for which to create an instance.</param>
		/// <param name="constructorParameters">The constructor parameters.</param>
		/// <returns>Returns the created instance.</returns>
		TType Create<TType>(Type type, params object[] constructorParameters);
		#endregion
	}
}