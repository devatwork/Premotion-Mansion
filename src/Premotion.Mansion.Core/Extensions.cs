using System;
using System.Linq;
using System.Xml.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Provides extensiosn used by the SoftTools integration project.
	/// </summary>
	public static class Extensions
	{
		#region Extensions of XContainer
		/// <summary>
		/// Gets the first (in document order) child element with the specified <paramref name="name"/>.
		/// </summary>
		/// <param name="element">The <see cref="XContainer"/> which to search.</param>
		/// <param name="name">The <see cref="XName"/> to match.</param>
		/// <returns>Returns the <see cref="XContainer"/> with the specified <paramref name="name"/>.</returns>
		/// <exception cref="InvalidOperationException">Thrown when an element with <paramref name="name"/> is not found in <paramref name="element"/>.</exception>
		public static XElement RequiredElement(this XContainer element, XName name)
		{
			// validate arguments
			if (element == null)
				throw new ArgumentNullException("element");
			if (name == null)
				throw new ArgumentNullException("name");

			// try to find the element
			var child = element.Element(name);
			if (child == null)
				throw new InvalidOperationException(string.Format("Expected element of name '{0}' in '{1}'", name, element));
			return child;
		}
		/// <summary>
		/// Checks whether <paramref name="element"/> has an element with the specified <paramref name="name"/>.
		/// </summary>
		/// <param name="element">The <see cref="XContainer"/> which to search.</param>
		/// <param name="name">The <see cref="XName"/> to match.</param>
		/// <returns>Returns true when the element is found otherwise false.</returns>
		public static bool HasElement(this XContainer element, XName name)
		{
			// validate arguments
			if (element == null)
				throw new ArgumentNullException("element");
			if (name == null)
				throw new ArgumentNullException("name");

			// try to find the element
			return element.Element(name) != null;
		}
		/// <summary>
		/// Appends the <paramref name="node"/> to the <paramref name="container"/>.
		/// </summary>
		/// <typeparam name="TNode">The type of <see cref="XNode"/> which will be added.</typeparam>
		/// <param name="container">The <see cref="XContainer"/> to which to append the <paramref name="node"/></param>
		/// <param name="node">The <see cref="XNode"/> which to append.</param>
		/// <returns>Returns the appended node.</returns>
		public static TNode Append<TNode>(this XContainer container, TNode node) where TNode : XNode
		{
			// validate arguments
			if (container == null)
				throw new ArgumentNullException("container");
			if (node == null)
				throw new ArgumentNullException("node");

			// add to the container
			container.Add(node);
			return node;
		}
		#endregion
		#region Extensions of XElement
		/// <summary>
		/// Gets attribute with the specified <paramref name="name"/>.
		/// </summary>
		/// <param name="element">The <see cref="XElement"/> which to search.</param>
		/// <param name="name">The <see cref="XName"/> to match.</param>
		/// <returns>Returns the <see cref="XElement"/> with the specified <paramref name="name"/>.</returns>
		/// <exception cref="InvalidOperationException">Thrown when an attribute with <paramref name="name"/> is not found in <paramref name="element"/>.</exception>
		public static XAttribute RequiredAttribute(this XElement element, XName name)
		{
			// validate arguments
			if (element == null)
				throw new ArgumentNullException("element");
			if (name == null)
				throw new ArgumentNullException("name");

			// try to find the element
			var attribute = element.Attribute(name);
			if (attribute == null)
				throw new InvalidOperationException(string.Format("Expected attribute of name '{0}' in '{1}'", name, element));
			return attribute;
		}
		#endregion
		#region Extensions of IRepository
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> on which to execute the query.</param>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		public static Nodeset Retrieve(this IRepository repository, MansionContext context, IPropertyBag arguments)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// parse the query
			var query = repository.ParseQuery(context, arguments);

			// execute the query
			return repository.Retrieve(context, query);
		}
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> on which to execute the query.</param>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		public static Node RetrieveSingle(this IRepository repository, MansionContext context, IPropertyBag arguments)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// parse the query
			var query = repository.ParseQuery(context, arguments);

			// execute the query
			return repository.RetrieveSingle(context, query);
		}
		/// <summary>
		/// Retrieves the root <see cref="Node"/>.
		/// </summary>
		/// <param name="repository">The <see cref="IRepository"/> from which to retrieve the node.</param>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the root node when found.</returns>
		/// <exception cref="InvalidOperationException">Thrown when root node could not be found in the repository.</exception>
		public static Node RetrieveRootNode(this IRepository repository, MansionContext context)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (context == null)
				throw new ArgumentNullException("context");

			// retrieve the node
			var rootNode = repository.RetrieveSingle(context, new PropertyBag
			                                                  {
			                                                  	{"id", 1}
			                                                  });
			if (rootNode == null)
				throw new InvalidOperationException("Could not find root node, please check repository");
			return rootNode;
		}
		#endregion
		#region Extensions of String
		/// <summary>
		/// Checks whether the <paramref name="input"/> is a number.
		/// </summary>
		/// <param name="input">The input which to check.</param>
		/// <returns>Returns true when the <paramref name="input"/> is a number otherwise false.</returns>
		public static bool IsNumber(this string input)
		{
			// validate arguments
			if (string.IsNullOrEmpty(input))
				return false;

			return input.All(Char.IsDigit);
		}
		#endregion
	}
}