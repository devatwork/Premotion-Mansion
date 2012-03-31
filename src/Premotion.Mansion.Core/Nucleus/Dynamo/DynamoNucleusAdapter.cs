﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dynamo.Ioc;

namespace Premotion.Mansion.Core.Nucleus.Dynamo
{
	/// <summary>
	/// Implements the <see cref="INucleus"/> using the Dynamo <see cref="Container"/>.
	/// </summary>
	public class DynamoNucleusAdapter : NucleusBase
	{
		#region Constructors
		/// <summary>
		/// Constructs a new Dynamo Nucleus adapter.
		/// </summary>
		public DynamoNucleusAdapter()
		{
			// create a new container
			container = new Container(Lifetime.Container);

			// register the nucleus interface in the container
			container.RegisterInstance(typeof (INucleus), this);
			container.RegisterInstance(typeof (IConfigurableNucleus), this);
		}
		#endregion
		#region Implementation of INucleus
		/// <summary>
		/// Resolves all the components which implement the <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <returns>Returns an <see cref="IEnumerable{TContract}"/> containing all the matching components.</returns>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected override IEnumerable<TContract> DoResolve<TContract>()
		{
			return container.TryResolveAll<TContract>();
		}
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="instance">The instance of <typeparamref name="TContract"/> when found, otherwise null.</param>
		/// <returns>Returns true when a component could be resolved otherwise false.</returns>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected override bool DoTryResolveSingle<TContract>(out TContract instance)
		{
			// let the container resolve the instance
			instance = container.TryResolve<TContract>();

			// check if a result was found
			return instance != null;
		}
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="name">The strong name of the compoment which to resolve.</param>
		/// <param name="instance">The instance of <typeparamref name="TContract"/> when found, otherwise null.</param>
		/// <returns>Returns true when a component could be resolved otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown when t<paramref name="name"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected override bool DoTryResolveSingle<TContract>(string name, out TContract instance)
		{
			// let the container resolve the instance
			instance = container.TryResolve<TContract>(name);

			// check if a result was found
			return instance != null;
		}
		/// <summary>
		/// Registers a <paramref name="instanceFactory"/> for component with contract <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/> of the component which to register.</typeparam>
		/// <param name="instanceFactory">The instance factory for the component.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="instanceFactory"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected override void DoRegister<TContract>(Expression<Func<INucleus, TContract>> instanceFactory)
		{
			// create the instance epxression
			var instanceFactoryExpression = AssembleFactoryExpression(instanceFactory);

			// register the factory
			container.Register(instanceFactoryExpression);
		}
		/// <summary>
		/// Registers a <paramref name="instanceFactory"/> for component with contract <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/> of the component which to register.</typeparam>
		/// <param name="name">The name of the component.</param>
		/// <param name="instanceFactory">The instance factory for the component.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="name"/> or <paramref name="instanceFactory"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">Thrown when the Nucleus is already disposed.</exception>
		protected override void DoRegister<TContract>(string name, Expression<Func<INucleus, TContract>> instanceFactory)
		{
			// create the instance epxression
			var instanceFactoryExpression = AssembleFactoryExpression(instanceFactory);

			// register the factory
			container.Register(name, instanceFactoryExpression).WithLifetime(Lifetime.Transient());
		}
		/// <summary>
		/// Optimizes the underlying IoC container for performance. The container is now considered read-only.
		/// </summary>
		protected override void DoOptimize()
		{
			container.Compile();
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			if (!disposeManagedResources)
				return;

			// dispose the container
			container.Dispose();
		}
		#endregion
		#region Assemble Methods
		/// <summary>
		/// Assembles an registerable expression from the <paramref name="instanceFactory"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/> of the component which to register.</typeparam>
		/// <param name="instanceFactory">The instance factory for the component.</param>
		/// <returns>Returns the registerable instance expression.</returns>
		private static Expression<Func<IResolver, TContract>> AssembleFactoryExpression<TContract>(Expression<Func<INucleus, TContract>> instanceFactory)
		{
			// find the resolve method
			var resolverType = typeof (IResolver);
			var nucleusType = typeof (INucleus);
			var resolveMethodInfo = resolverType.GetMethod("Resolve", new[] {typeof (Type)});

			// build the expression
			var resolverParameterExpression = Expression.Parameter(resolverType, "resolver");
			var resolveNucleusExpression = Expression.Call(resolverParameterExpression, resolveMethodInfo, Expression.Constant(nucleusType, typeof (Type)));
			var invokeInstanceFactoryExpression = Expression.Invoke(instanceFactory, Expression.Convert(resolveNucleusExpression, nucleusType));
			var finalExpression = Expression.Lambda<Func<IResolver, TContract>>(invokeInstanceFactoryExpression, resolverParameterExpression);
			return finalExpression;
		}
		#endregion
		#region Private Fields
		private readonly Container container;
		#endregion
	}
}