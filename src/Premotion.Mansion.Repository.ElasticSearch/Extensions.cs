using System;
using System.Linq;
using Premotion.Mansion.Repository.ElasticSearch.Schema;
using Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings;

namespace Premotion.Mansion.Repository.ElasticSearch
{
	/// <summary>
	/// Provides extension methods used by the ElasticSearch implementation.
	/// </summary>
	public static class Extensions
	{
		#region IndexDefinition Extensions
		/// <summary>
		/// Finds the <see cref="TypeMapping"/> of the given <paramref name="typeName"/> in the <paramref name="indexDefinition"/>.
		/// </summary>
		/// <param name="indexDefinition">The <see cref="IndexDefinition"/> in which to look for the <see cref="TypeMapping"/>.</param>
		/// <param name="typeName">The name of the type for which to look.</param>
		/// <returns>Returns the found <see cref="TypeMapping"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if the type mapping for <paramref name="typeName"/> was not found in <paramref name="indexDefinition"/>.</exception>
		public static TypeMapping FindTypeMapping(this IndexDefinition indexDefinition, string typeName)
		{
			// validate arguments
			if (indexDefinition == null)
				throw new ArgumentNullException("indexDefinition");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			// find the mapping
			return indexDefinition.Mappings.Values.First(candidate => candidate.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));
		}
		#endregion
		#region TypeMapping Extensions
		/// <summary>
		/// Finds the <typeparamref name="TPropertyMapping"/> of property <paramref name="propertyName"/> of type <paramref name="typeMapping"/>.
		/// </summary>
		/// <param name="typeMapping">The <see cref="TypeMapping"/> from which to get the property.</param>
		/// <param name="propertyName">The name of the property for which to get the <typeparamref name="TPropertyMapping"/>.</param>
		/// <typeparam name="TPropertyMapping">The type of <see cref="PropertyMapping"/> which to find.</typeparam>
		/// <returns>Returns the <typeparamref name="TPropertyMapping"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if the mapping was not found or could not be cast to <typeparamref name="TPropertyMapping"/>.</exception>
		public static TPropertyMapping FindPropertyMapping<TPropertyMapping>(this TypeMapping typeMapping, string propertyName) where TPropertyMapping : PropertyMapping
		{
			// validate arguments
			if (typeMapping == null)
				throw new ArgumentNullException("typeMapping");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// try to find the property mapping
			PropertyMapping mapping;
			if (!typeMapping.Properties.TryGetValue(propertyName, out mapping))
				throw new InvalidOperationException(string.Format("Could not find property mapping '{0}'.'{1}'", typeMapping.Name, propertyName));

			// check type
			if (!(mapping is TPropertyMapping))
				throw new InvalidOperationException(string.Format("Could not find property mapping '{0}'.'{1}' of type '{2}' to '{3}'", typeMapping.Name, propertyName, mapping.GetType(), typeof (TPropertyMapping)));

			// return the mapping
			return (TPropertyMapping) mapping;
		}
		/// <summary>
		/// Finds the <typeparamref name="TPropertyMapping"/> of property <paramref name="propertyName"/> of type <paramref name="typeMapping"/>.
		/// </summary>
		/// <param name="typeMapping">The <see cref="TypeMapping"/> from which to get the property.</param>
		/// <param name="propertyName">The name of the property for which to get the <typeparamref name="TPropertyMapping"/>.</param>
		/// <param name="mapping">Returns the <typeparamref name="TPropertyMapping"/>.</param>
		/// <typeparam name="TPropertyMapping">The type of <see cref="PropertyMapping"/> which to find.</typeparam>
		/// <returns>Returns true when the mapping is found, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if the mapping was not found or could not be cast to <typeparamref name="TPropertyMapping"/>.</exception>
		public static bool TryFindPropertyMapping<TPropertyMapping>(this TypeMapping typeMapping, string propertyName, out TPropertyMapping mapping) where TPropertyMapping : PropertyMapping
		{
			// validate arguments
			if (typeMapping == null)
				throw new ArgumentNullException("typeMapping");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// try to find the property mapping
			PropertyMapping value;
			if (!typeMapping.Properties.TryGetValue(propertyName, out value))
			{
				mapping = default(TPropertyMapping);
				return false;
			}

			// check type
			mapping = value as TPropertyMapping;
			return mapping != null;
		}
		#endregion
		#region String Extensions
		/// <summary>
		/// Checks whether the given <paramref name="name"/> is a valid index name.
		/// </summary>
		/// <param name="name">The name which to validate.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is larger or smaller than allowed or when it contains invalid characters.</exception>
		public static void ValidateAsIndexName(this string name)
		{
			// validate arguments
			if (name == null)
				throw new ArgumentNullException("name", "Index names can not be null");

			// index names should be between 3 and 16 characters
			if (name.Length < 3)
				throw new ArgumentOutOfRangeException("name", name, "Index names must be at least 3 characters");
			if (name.Length > 16)
				throw new ArgumentOutOfRangeException("name", name, "Index names must be at most 16 characters");

			// validate characters
			var invalidChar = name.FirstOrDefault(c => !(Char.IsLetterOrDigit(c) || c == '-'));
			if (invalidChar != Char.MinValue)
				throw new ArgumentOutOfRangeException("name", name, "Index name should be between 3-16 characters and only letters, numbers and hyphens ('-') are allowed");
		}
		/// <summary>
		/// Normalizes the given field <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name which to normalize.</param>
		/// <returns>Returns the normalized field name.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
		public static string NormalizeFieldName(this string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name", "Field names can not be null");

			return name.ToLower();
		}
		/// <summary>
		/// Get the base name of the given <paramref name="field"/>.
		/// </summary>
		/// <param name="field">The name which to normalize.</param>
		/// <returns>Returns the base field name.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="field"/> is null.</exception>
		public static string GetFieldNameBase(this string field)
		{
			// validate arguments
			if (string.IsNullOrEmpty(field))
				throw new ArgumentNullException("field", "Field names can not be null");

			// check if there is a dot
			var dotIndex = field.IndexOf('.');
			return (dotIndex == -1 ? field : field.Substring(0, dotIndex)).NormalizeFieldName();
		}
		#endregion
	}
}