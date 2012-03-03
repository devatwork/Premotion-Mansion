using System;
using System.Xml;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;

namespace Premotion.Mansion.Core.Patterns.Descriptors
{
	/// <summary>
	/// Factory class for XML descriptors.
	/// </summary>
	/// <typeparam name="TDescriptor">The type of descriptor.</typeparam>
	public static class XmlDescriptorFactory<TDescriptor> where TDescriptor : class, IDescriptor
	{
		#region Factory Methods
		/// <summary>
		/// Creates an descriptor from an XML element..
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="element">The XML element from which to create the descriptor.</param>
		/// <returns>Returns the created desciptor.</returns>
		public static IDescriptor Create(IContext context, string element)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(element))
				throw new ArgumentNullException("element");

			// create an XML element
			var doc = new XmlDocument();
			doc.LoadXml(element);

			// parse the node
			return Create(context, doc.DocumentElement);
		}
		/// <summary>
		/// Creates an descriptor from an XML element.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="element">The XML element from which to create the descriptor.</param>
		/// <param name="additionalConstructorParameters">Additional constructor parameters passed to the constructor of the descriptor.</param>
		/// <returns>Returns the created desciptor.</returns>
		public static TDescriptor Create(IContext context, XmlNode element, params object[] additionalConstructorParameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (element == null)
				throw new ArgumentNullException("element");
			if (additionalConstructorParameters == null)
				throw new ArgumentNullException("additionalConstructorParameters");

			// get the properties 
			var namespaceUri = element.NamespaceURI;
			var name = element.LocalName;
			var properties = new PropertyBag();
			foreach (XmlAttribute attribute in element.Attributes)
				properties.Set(attribute.LocalName, attribute.Value);

			// assemble the constructor parameters
			var constructorParameters = new object[3 + additionalConstructorParameters.Length];
			constructorParameters[0] = namespaceUri;
			constructorParameters[1] = name;
			constructorParameters[2] = properties;
			Array.Copy(additionalConstructorParameters, 0, constructorParameters, 3, additionalConstructorParameters.Length);

			// get the descriptor type
			Type descriptor;
			var namingService = context.Cast<INucleusAwareContext>().Nucleus.Get<ITypeDirectoryService>(context);
			var objectFactoryService = context.Cast<INucleusAwareContext>().Nucleus.Get<IObjectFactoryService>(context);
			if (!namingService.TryLookupSingle<TDescriptor>(namespaceUri, name, out descriptor))
				throw new InvalidOperationException(string.Format("Could not find descriptor class with name '{0}' in namespace '{1}'", name, namespaceUri));
			return objectFactoryService.Create<TDescriptor>(descriptor, constructorParameters);
		}
		#endregion
	}
}