using System;
using System.Linq;

namespace Premotion.Mansion.Repository.ElasticSearch
{
	/// <summary>
	/// Provides extension methods used by the ElasticSearch implementation.
	/// </summary>
	public static class Extensions
	{
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
			if (invalidChar != char.MinValue)
				throw new ArgumentOutOfRangeException("name", name, "Index name should be between 3-16 characters and only letters, numbers and hyphens ('-') are allowed");
		}
	}
}