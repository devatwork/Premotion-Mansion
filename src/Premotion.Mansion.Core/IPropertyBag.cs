using System.Collections.Generic;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Represents a case insensive bag of properties.
	/// </summary>
	/// <remarks></remarks>
	public interface IPropertyBag : IEnumerable<KeyValuePair<string, object>>
	{
		#region Property Methods
		/// <summary>
		/// Tries to get the value from this bag.
		/// </summary>
		/// <typeparam name="TValue">The type of value.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="target">The target.</param>
		/// <returns>Returns true when the property could be retrieved otherwise false.</returns>
		bool TryGet<TValue>(IContext context, string propertyName, out TValue target);
		/// <summary>
		/// Tries to get the value from this bag and remove it.
		/// </summary>
		/// <typeparam name="TValue">The type of value.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="target">The target.</param>
		/// <returns>Returns true when the property could be retrieved otherwise false.</returns>
		bool TryGetAndRemove<TValue>(IContext context, string propertyName, out TValue target);
		/// <summary>
		/// Sets a new property in the bag with the name <paramref name="propertyName"/> and the value <paramref name="value"/>. Overwrites any value.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="value">The new value of the property.</param>
		void Set(string propertyName, object value);
		/// <summary>
		/// Sets a new property in the bag with the name <paramref name="propertyName"/> and the value <paramref name="value"/> only when the property does not already exist.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="value">The new value of the property.</param>
		bool TrySet(string propertyName, object value);
		/// <summary>
		/// Merges properties into this bag.
		/// </summary>
		/// <param name="properties"></param>
		void Merge(IEnumerable<KeyValuePair<string, object>> properties);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the names of all the properties in this bag.
		/// </summary>
		IEnumerable<string> Names { get; }
		/// <summary>
		/// Gets the number of properties in this bag.
		/// </summary>
		int Count { get; }
		#endregion
	}
}