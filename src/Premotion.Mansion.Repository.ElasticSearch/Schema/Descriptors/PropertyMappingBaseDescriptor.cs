using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Descriptors
{
	/// <summary>
	/// Represents the base <see cref="TypeDescriptor"/> for elastic search properties.
	/// </summary>
	public abstract class PropertyMappingBaseDescriptor : TypeDescriptor
	{
		#region Create Methods
		/// <summary>
		/// Creates a <see cref="PropertyMapping"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <returns>The created <see cref="PropertyMapping"/>.</returns>
		public PropertyMapping CreateMapping(IMansionContext context, IPropertyDefinition property)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (property == null)
				throw new ArgumentNullException("property");

			// create the mapping
			return DoCreateMapping(context, property);
		}
		/// <summary>
		/// Creates a <see cref="PropertyMapping"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <returns>The created <see cref="PropertyMapping"/>.</returns>
		protected abstract PropertyMapping DoCreateMapping(IMansionContext context, IPropertyDefinition property);
		#endregion
	}
}