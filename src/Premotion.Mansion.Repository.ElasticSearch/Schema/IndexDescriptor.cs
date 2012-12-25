using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Represents the <see cref="TypeDescriptor"/> for elastic search indices.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "index")]
	public class IndexDescriptor : TypeDescriptor
	{
		#region Create Methods
		/// <summary>
		/// Creates a <see cref="IndexDescriptor"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>The created <see cref="IndexDefinition"/>.</returns>
		public IndexDefinition CreateDefinition(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the required properties
			var name = Properties.Get<string>(context, "name");

			// create the definition
			var definition = new IndexDefinition(name);

			// map settings
			int numberOfShards;
			if (Properties.TryGet(context, "numberOfShards", out numberOfShards))
				definition.Settings.NumberOfShards = numberOfShards;
			int numberOfReplicas;
			if (Properties.TryGet(context, "numberOfReplicas", out numberOfReplicas))
				definition.Settings.NumberOfReplicas = numberOfReplicas;

			// return the created definition
			return definition;
		}
		#endregion
	}
}