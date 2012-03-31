using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Descriptors;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Defines extension methods for several types.
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		public static Nodeset Retrieve(this IRepository repository, IMansionContext context, IPropertyBag arguments)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		public static Node RetrieveSingle(this IRepository repository, IMansionContext context, IPropertyBag arguments)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the root node when found.</returns>
		/// <exception cref="InvalidOperationException">Thrown when root node could not be found in the repository.</exception>
		public static Node RetrieveRootNode(this IRepository repository, IMansionContext context)
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
		#region INucleus Extensions
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		/// <returns>Returns the resolved contract type.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="nucleus"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown the <typeparamref name="TContract"/> instance could not be resolved.</exception>
		public static TContract ResolveSingle<TContract>(this INucleus nucleus) where TContract : class
		{
			//  validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// resolve the object to an instance or throw an exception
			TContract result;
			if (!nucleus.TryResolveSingle(out result))
				throw new InvalidOperationException(string.Format("Missing a dependency of type '{0}'", typeof (TContract)));
			return result;
		}
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		/// <param name="namespaceUri">The namespace in which the component lives.</param>
		/// <param name="name">The name of the component.</param>
		/// <returns>Returns the resolved contract type.</returns>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="nucleus"/>, <paramref name="namespaceUri"/> or <paramref name="name"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown the <typeparamref name="TContract"/> instance could not be resolved.</exception>
		public static TContract ResolveSingle<TContract>(this INucleus nucleus, string namespaceUri, string name) where TContract : class
		{
			//  validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");
			if (string.IsNullOrEmpty(namespaceUri))
				throw new ArgumentNullException("namespaceUri");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// assemble the name
			var strongName = StrongNameGenerator.Generate(namespaceUri, name);

			// resolve the object to an instance or throw an exception
			TContract result;
			if (!nucleus.TryResolveSingle(strongName, out result))
				throw new InvalidOperationException(string.Format("Missing a dependency of type '{0}' with namespace '{1}' and name '{2}'", typeof (TContract), namespaceUri, name));
			return result;
		}
		#endregion
		#region ITypeDefinition Extensions
		/// <summary>
		/// Tries to find the <typeparamref name="TDescriptor"/> in the reverse type hierarchy of <paramref name="typeDefinition"/>.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of <see cref="IDescriptor"/>.</typeparam>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> for which to get the <paramref name="descriptor"/>.</param>
		/// <param name="descriptor">The instance of <typeparamref name="TDescriptor"/>.</param>
		/// <returns>Returns true when found, otherwise false.</returns>
		public static bool TryFindDescriptorInHierarchy<TDescriptor>(this ITypeDefinition typeDefinition, out TDescriptor descriptor) where TDescriptor : class, IDescriptor
		{
			// validate arguments
			if (typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");

			// loop through all the types in the hierarchy
			foreach (var type in typeDefinition.HierarchyReverse)
			{
				if (type.TryGetDescriptor(out descriptor))
					return true;
			}

			// descriptor not found
			descriptor = default(TDescriptor);
			return false;
		}
		/// <summary>
		/// Tries to find the <typeparamref name="TDescriptor"/> in the reverse type hierarchy of <paramref name="typeDefinition"/>.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of <see cref="IDescriptor"/>.</typeparam>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> for which to get the <paramref name="descriptor"/>.</param>
		/// <param name="predicate">The <see cref="Predicate{TDescriptor}"/> which can filter the returned descriptor.</param>
		/// <param name="descriptor">The instance of <typeparamref name="TDescriptor"/>.</param>
		/// <returns>Returns true when found, otherwise false.</returns>
		public static bool TryFindDescriptorInHierarchy<TDescriptor>(this ITypeDefinition typeDefinition, Predicate<TDescriptor> predicate, out TDescriptor descriptor) where TDescriptor : class, IDescriptor
		{
			// validate arguments
			if (typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			// loop through all the types in the hierarchy
			foreach (var type in typeDefinition.HierarchyReverse)
			{
				if (type.TryGetDescriptor(out descriptor) && predicate(descriptor))
					return true;
			}

			// descriptor not found
			descriptor = default(TDescriptor);
			return false;
		}
		#endregion
		#region String Extensions
		/// <summary>
		/// Checks whether the <paramref name="input"/> is a number.
		/// </summary>
		/// <param name="input">The input which to check.</param>
		/// <returns>Returns true when the <paramref name="input"/> is a number otherwise false.</returns>
		public static bool IsNumber(this string input)
		{
			// validate arguments
			return !string.IsNullOrEmpty(input) && input.All(Char.IsDigit);
		}
		#endregion
		#region IMansionContext Extensions
		/// <summary>
		/// Gets the unwrapped <see cref="IRepository"/> which is stripped from all decorators.
		/// </summary>
		/// <returns>Returns the unwrapped <see cref="IRepository"/>.</returns>
		public static IRepository GetUnwrappedRepository(this IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the current repository
			var repository = context.Repository;

			// unwrap the decorators
			while (repository is IRepositoryDecorator)
			{
				// unwrap it
				repository = ((IRepositoryDecorator) repository).DecoratedRepository;
			}

			return repository;
		}
		#endregion
		#region Type Extensions
		/// <summary>
		/// Creates an instance of type <paramref name="type"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/>.</typeparam>
		/// <param name="type">The <see cref="Type"/> from which to construct an instance.</param>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		/// <returns>Returns the created object.</returns>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="type"/> or <paramref name="nucleus"/> is null.</exception>
		public static TContract CreateInstance<TContract>(this Type type, INucleus nucleus) where TContract : class
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// select the factory function
			var nucleusParameterExpression = Expression.Parameter(typeof (INucleus), "nucleus");
			var factory = ObjectFactoryCache.GetOrAdd(type, key => Expression.Lambda<Func<INucleus, object>>(Expression.Invoke(CreateInstanceFactory<TContract>(key), nucleusParameterExpression), nucleusParameterExpression).Compile());

			// create the instance and return it
			return (TContract) factory(nucleus);
		}
		/// <summary>
		/// Creates an instance factory for the <paramref name="type"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/>.</typeparam>
		/// <param name="type">The <see cref="Type"/> for which to factory will be created.</param>
		/// <returns>Returns the created factory.</returns>
		public static Expression<Func<INucleus, TContract>> CreateInstanceFactory<TContract>(this Type type) where TContract : class
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");

			// make sure the type actually implements the contract
			if (!typeof (TContract).IsAssignableFrom(type))
				throw new InvalidOperationException(string.Format("Type '{0}' does not implement contract type '{1}'", type, typeof (TContract)));

			// get the constructor
			var constructorMethodInfos = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			if (constructorMethodInfos.Length != 1)
				throw new InvalidOperationException(string.Format("Could not find an unambiguous public constructor on type '{0}'", type));
			var constructorMethodInfo = constructorMethodInfos[0];
			var constructorParameterInfos = constructorMethodInfo.GetParameters();

			// create the expression list
			var bodyExpressions = new List<Expression>();

			// construct the nucleus param
			var nucleusType = typeof (INucleus);
			var tryResolveSingleMethodInfo = nucleusType.GetMethods().Single(candidate => "TryResolveSingle".Equals(candidate.Name) && candidate.GetParameters().Length == 1);
			if (tryResolveSingleMethodInfo == null)
				throw new InvalidOperationException(string.Format("Could not find mehtod TryResolveSingle with one parameter on type '{0}'", nucleusType));
			var nucleusParameterExpression = Expression.Parameter(nucleusType, "nucleus");

			// construct the parameters for the constructor
			var parameterTypes = constructorParameterInfos.Select(parameter => Expression.Parameter(parameter.ParameterType, parameter.Name)).ToList();
			bodyExpressions.AddRange(constructorParameterInfos.Select((parameterInfo, index) =>
			                                                          {
			                                                          	// get the type
			                                                          	var injectedType = parameterInfo.ParameterType;

			                                                          	// define the out parameter
			                                                          	var outParameter = Expression.Variable(injectedType, "out");

			                                                          	// bake the method call
			                                                          	var tryResolveCallExpression = Expression.Call(nucleusParameterExpression, tryResolveSingleMethodInfo.MakeGenericMethod(injectedType), outParameter);

			                                                          	// throw if the type could not be found
			                                                          	var newDependencyNotFoundExceptionExpression = Expression.New(typeof (InvalidOperationException).GetConstructor(new[] {typeof (string)}), new[] {Expression.Constant(string.Format("Could not resolve injected type '{0}' on type '{1}' make sure it is registered properly", injectedType, type))});
			                                                          	var checkResolveResultExpression = Expression.IfThen(Expression.Not(tryResolveCallExpression), Expression.Throw(newDependencyNotFoundExceptionExpression));

			                                                          	// set the value
			                                                          	var assignResultExpression = Expression.Assign(parameterTypes[index], outParameter);

			                                                          	// return the block
			                                                          	return Expression.Block(new[] {outParameter}, new Expression[] {checkResolveResultExpression, assignResultExpression});
			                                                          }));

			// bake the constructor call
			bodyExpressions.Add(Expression.New(constructorMethodInfo, parameterTypes));

			// bake the method call using expressions
			return Expression.Lambda<Func<INucleus, TContract>>(Expression.Block(parameterTypes, bodyExpressions), nucleusParameterExpression);
		}
		#endregion
		#region Private Fields
		private static readonly ConcurrentDictionary<Type, Func<INucleus, object>> ObjectFactoryCache = new ConcurrentDictionary<Type, Func<INucleus, object>>();
		#endregion
	}
}