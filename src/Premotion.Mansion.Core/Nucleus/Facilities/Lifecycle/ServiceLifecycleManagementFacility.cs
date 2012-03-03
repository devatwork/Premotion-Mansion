using System.Collections.Concurrent;
using Premotion.Mansion.Core.Nucleus.Extension;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle
{
	/// <summary>
	/// This facility manages the lifecycle of services.
	/// </summary>
	public class ServiceLifecycleManagementFacility : FacilityBase
	{
		#region Overrides of FacilityBase
		/// <summary>
		/// Activates this facility in the <paramref name="nucleus"/>. This event it typically used to register listeners to nucleus events.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="nucleus">The <see cref="IExtendedNucleus"/> in which this facility is activated.</param>
		protected override void DoActivate(IContext context, IExtendedNucleus nucleus)
		{
			// check new services for IManagedLifecycleService
			nucleus.ServiceRegistered += (ctx, model) =>
			                             {
			                             	// check if the service does not implement the IManagedLifecycleService
			                             	var lifetimeManagedService = model.GetInstance<IService>() as IManagedLifecycleService;
			                             	if (lifetimeManagedService == null)
			                             		return;

			                             	// add to the disposable list
			                             	disposableServices.Add(lifetimeManagedService);
			                             };

			// check starting services for IManagedLifecycleService
			nucleus.ServiceActivated += (ctx, model) =>
			                            {
			                            	// check if the service does not implement the IManagedLifecycleService
			                            	var lifetimeManagedService = model.GetInstance<IService>() as IManagedLifecycleService;
			                            	if (lifetimeManagedService == null)
			                            		return;

			                            	// add to the disposable list
			                            	lifetimeManagedService.Start(ctx);
			                            };
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
			// check for managed resource disposal
			if (!disposeManagedResources)
				return;

			// dispose all services
			foreach (var disposableService in disposableServices)
				disposableService.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly BlockingCollection<IManagedLifecycleService> disposableServices = new BlockingCollection<IManagedLifecycleService>();
		#endregion
	}
}