using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Descriptors
{
	/// <summary>
	/// Represents the <see cref="TypeDescriptor"/> for elastic search types.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "typeMapping")]
	public class TypeMappingDescriptor : TypeDescriptor
	{
		#region Update Methods
		/// <summary>
		/// Updates the given <paramref name="typeMapping"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeMapping">The <see cref="TypeMapping"/>.</param>
		/// <returns>The created <see cref="PropertyMapping"/>.</returns>
		public void UpdateMapping(IMansionContext context, TypeMapping typeMapping)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (typeMapping == null)
				throw new ArgumentNullException("typeMapping");

			throw new NotImplementedException();
		}
		#endregion
	}
}