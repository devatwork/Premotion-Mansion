using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings
{
	/// <summary>
	/// Represents the base <see cref="TypeDescriptor"/> for elastic search properties.
	/// </summary>
	public abstract class PropertyMappingBaseDescriptor : TypeDescriptor
	{
		#region Add Mapping Methods
		/// <summary>
		/// Adds <see cref="PropertyMapping"/>s of <paramref name="property"/> to the given <paramref name="typeMapping"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/> of the property for which to add the <see cref="PropertyMapping"/>s.</param>
		/// <param name="typeMapping">The <see cref="TypeMapping"/> to which to add the new <see cref="PropertyMapping"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		public void AddMappingTo(IMansionContext context, IPropertyDefinition property, TypeMapping typeMapping)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (property == null)
				throw new ArgumentNullException("property");
			if (typeMapping == null)
				throw new ArgumentNullException("typeMapping");

			// invoke template method
			DoAddMappingTo(context, property, typeMapping);
		}
		/// <summary>
		/// Adds <see cref="PropertyMapping"/>s of <paramref name="property"/> to the given <paramref name="typeMapping"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/> of the property for which to add the <see cref="PropertyMapping"/>s.</param>
		/// <param name="typeMapping">The <see cref="TypeMapping"/> to which to add the new <see cref="PropertyMapping"/>s.</param>
		protected abstract void DoAddMappingTo(IMansionContext context, IPropertyDefinition property, TypeMapping typeMapping);
		#endregion
	}
}