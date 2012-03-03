using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Url;

namespace Premotion.Mansion.Web.Descriptors
{
	/// <summary>
	/// Describes the <see cref="NodeUrlGenerator"/> for this <see cref="ITypeDefinition"/>.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "nodeUrlGenerator")]
	public class NodeUrlGeneratorDescriptor : TypeDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public NodeUrlGeneratorDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Create Methods
		/// <summary>
		/// Creates a <see cref="NodeUrlGenerator"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the created <see cref="NodeUrlGenerator"/>.</returns>
		public NodeUrlGenerator CreateGenerator(INucleusAwareContext context)
		{
			// get the type
			var generatorType = Properties.Get<Type>(context, "type");

			// create the instance
			var objectFactoryService = context.Nucleus.Get<IObjectFactoryService>(context);
			return objectFactoryService.Create<NodeUrlGenerator>(generatorType);
		}
		#endregion
	}
}