using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Urls;

namespace Premotion.Mansion.Web.Descriptors
{
	/// <summary>
	/// Describes the <see cref="NodeUrlGenerator"/> for this <see cref="ITypeDefinition"/>.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "nodeUrlGenerator")]
	public class NodeUrlGeneratorDescriptor : TypeDescriptor
	{
		#region Create Methods
		/// <summary>
		/// Creates a <see cref="NodeUrlGenerator"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the created <see cref="NodeUrlGenerator"/>.</returns>
		public NodeUrlGenerator CreateGenerator(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the type
			var generatorType = Properties.Get<Type>(context, "type");

			// create the instance
			return generatorType.CreateInstance<NodeUrlGenerator>(context.Nucleus);
		}
		#endregion
	}
}