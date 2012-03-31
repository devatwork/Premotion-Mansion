using System;
using System.Xml;
using Premotion.Mansion.Core.Collections;

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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="element">The XML element from which to create the descriptor.</param>
		/// <returns>Returns the created desciptor.</returns>
		public static IDescriptor Create(IMansionContext context, string element)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="element">The XML element from which to create the descriptor.</param>
		/// <param name="preInitialize">Allows access to the descriptor before it is initialized.</param>
		/// <returns>Returns the created desciptor.</returns>
		public static TDescriptor Create(IMansionContext context, XmlNode element, Action<TDescriptor> preInitialize = null)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (element == null)
				throw new ArgumentNullException("element");

			// get the properties 
			var properties = new PropertyBag();
			if (element.Attributes != null)
			{
				foreach (XmlAttribute attribute in element.Attributes)
					properties.Set(attribute.LocalName, attribute.Value);
			}

			// create the descriptor instance
			var descriptor = context.Nucleus.ResolveSingle<TDescriptor>(element.NamespaceURI, element.LocalName);

			// set all the properties before initialization
			descriptor.Properties = properties;
			if (preInitialize != null)
				preInitialize(descriptor);

			// initialize the descriptor
			descriptor.Initialize(context);

			// get the descriptor type
			return descriptor;
		}
		#endregion
	}
}