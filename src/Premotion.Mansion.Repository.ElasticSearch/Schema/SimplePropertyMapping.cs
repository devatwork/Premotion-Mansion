using System;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Maps a simple property.
	/// </summary>
	public class SimplePropertyMapping : PropertyMapping
	{
		#region Constructors
		/// <summary>
		/// Constructs the property mapping with the given <paramref name="property"/>.
		/// </summary>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public SimplePropertyMapping(IPropertyDefinition property) : base(property)
		{
		}
		#endregion
	}
}