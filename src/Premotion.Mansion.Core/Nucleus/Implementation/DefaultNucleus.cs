using System;
using System.Collections.Concurrent;
using System.Linq;
using Premotion.Mansion.Core.Nucleus.Configuration;
using Premotion.Mansion.Core.Nucleus.Extension;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Nucleus.Implementation
{
	/// <summary>
	/// Provides the default implementation for <see cref="INucleus"/>, <see cref="IConfigurableNucleus"/> and <see cref="IExtendedNucleus"/>.
	/// </summary>
	public class DefaultNucleus : DisposableBase, IExtendedNucleus, IOwnedNuclues
	{
		#region Implementation of INucleus
		/// <summary>
		/// Gets a <see cref="IService"/> of type TService.
		/// </summary>
		/// <typeparam name="TContract">The type of <see cref="IService"/> which to get.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <returns>Returns the instance of the service.</returns>
		/// <exception cref="AmbiguousServiceFoundException">Thrown when TService did not resolve to a service instance.</exception>
		/// <exception cref="InvalidServiceStateException">Thrown when TService resolved to mulitple service instances.</exception>
		/// <exception cref="NoServiceFoundException">Thrown when TService resolved to a service which is not ready for use.</exception>
		public TContract Get<TContract>(IContext context) where TContract : IService
		{
			return (TContract) Get(context, typeof (TContract));
		}
		/// <summary>
		/// Gets a <see cref="IService"/> of type TService.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="contract">The contract of the service, must inherit from <see cref="IService"/>.</param>
		/// <returns>Returns the instance of the service.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="contract"/> does not inherit from <see cref="IService"/>.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="contract"/> is null.</exception>
		/// <exception cref="AmbiguousServiceFoundException">Thrown when TService did not resolve to a service instance.</exception>
		/// <exception cref="InvalidServiceStateException">Thrown when TService resolved to mulitple service instances.</exception>
		/// <exception cref="NoServiceFoundException">Thrown when TService resolved to a service which is not ready for use.</exception>
		public IService Get(IContext context, Type contract)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (contract == null)
				throw new ArgumentNullException("contract");
			if (!typeof (IService).IsAssignableFrom(contract))
				throw new ArgumentException(string.Format("The contract '{0}' does not inherit from IService", contract), "contract");
			CheckDisposed();

			// find the models implementating the requested service
			var models = serviceModels.Where(candidate => contract.IsAssignableFrom(candidate.ContractType)).ToArray();

			// check the number of models is out of range
			if (models.Length == 0)
				throw new NoServiceFoundException(contract);
			if (models.Length > 1)
				throw new AmbiguousServiceFoundException(contract, models.Select(x => x.ImplementationType));

			// check if the model is not initialized
			// TODO: add thread safety here
			var model = models[0];
			if (!model.Activated)
			{
				// set the started flag
				RaiseServiceActivating(context, model);
				model.Activated = true;
				RaiseServiceActivated(context, model);
			}

			// return the service instance
			return model.GetInstance<IService>();
		}
		#endregion
		#region Implementation of IConfigurableNucleus
		/// <summary>
		/// Augments this <see cref="INucleus"/> with a new <paramref name="facility"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="facility">The <see cref="IFacility"/>.</param>
		public void Augment(IContext context, IFacility facility)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (facility == null)
				throw new ArgumentNullException("facility");
			CheckDisposed();

			// add the facility
			facilities.Add(facility);

			// activate this facility
			facility.Activate(context, this);
		}
		/// <summary>
		/// Registers the <paramref name="instance"/> to this <see cref="INucleus"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of <see cref="IService"/> which to register.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="instance">The instance of <see cref="IService"/> which to register.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is null.</exception>
		public void Register<TContract>(IContext context, TContract instance) where TContract : class, IService
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (instance == null)
				throw new ArgumentNullException("instance");
			CheckDisposed();

			// make sure the instance actually implements the contract
			if (!typeof (TContract).IsAssignableFrom(instance.GetType()))
				throw new InvalidOperationException(string.Format("Service implementation '{0}' does not implement contract '{1}'.", instance.GetType(), typeof (TContract)));

			// create the model of this service
			var model = new ServiceModel(typeof (TContract), instance);

			// register the model
			RaiseServiceRegistering(context, model);
			serviceModels.Add(model);
			RaiseServiceRegistered(context, model);
		}
		#endregion
		#region Implementation of IExtendedNucleus
		/// <summary>
		/// Fired just before a service is registered in this <see cref="INucleus"/>.
		/// </summary>
		public event ServiceDelegate ServiceRegistering;
		/// <summary>
		/// Fired just after a service is registered in this <see cref="INucleus"/>.
		/// </summary>
		public event ServiceDelegate ServiceRegistered;
		/// <summary>
		/// Fired just before a service is started by this <see cref="INucleus"/>.
		/// </summary>
		public event ServiceDelegate ServiceActivating;
		/// <summary>
		/// Fired just after a service is started by this <see cref="INucleus"/>.
		/// </summary>
		public event ServiceDelegate ServiceActivated;
		/// <summary>
		/// Raises the <see cref="ServiceRegistering"/> event.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/> in which this nucleus is running.</param>
		/// <param name="model">The <see cref="IServiceModel"/> triggering the event.</param>
		private void RaiseServiceRegistering(IContext context, IServiceModel model)
		{
			var handlers = ServiceRegistering;
			if (handlers != null)
				handlers(GetNucleusContext(context), model);
		}
		/// <summary>
		/// Raises the <see cref="ServiceRegistered"/> event.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/> in which this nucleus is running.</param>
		/// <param name="model">The <see cref="IServiceModel"/> triggering the event.</param>
		private void RaiseServiceRegistered(IContext context, IServiceModel model)
		{
			var handlers = ServiceRegistered;
			if (handlers != null)
				handlers(GetNucleusContext(context), model);
		}
		/// <summary>
		/// Raises the <see cref="ServiceActivating"/> event.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/> in which this nucleus is running.</param>
		/// <param name="model">The <see cref="IServiceModel"/> triggering the event.</param>
		private void RaiseServiceActivating(IContext context, IServiceModel model)
		{
			var handlers = ServiceActivating;
			if (handlers != null)
				handlers(GetNucleusContext(context), model);
		}
		/// <summary>
		/// Raises the <see cref="ServiceActivated"/> event.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/> in which this nucleus is running.</param>
		/// <param name="model">The <see cref="IServiceModel"/> triggering the event.</param>
		private void RaiseServiceActivated(IContext context, IServiceModel model)
		{
			var handlers = ServiceActivated;
			if (handlers != null)
				handlers(GetNucleusContext(context), model);
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Gets the nucleus context.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <returns>Returns <see cref="NucleusContext"/>.</returns>
		private NucleusContext GetNucleusContext(IContext context)
		{
			return context.Extend(ctx => new NucleusContext(ctx, this));
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
			// check for managed resource
			if (!disposeManagedResources)
				return;

			// dispose all the facilities
			foreach (var facility in facilities)
				facility.Dispose();
		}
		#endregion
		#region Private Field
		private readonly BlockingCollection<IFacility> facilities = new BlockingCollection<IFacility>();
		private readonly BlockingCollection<ServiceModel> serviceModels = new BlockingCollection<ServiceModel>();
		#endregion
	}
}