using System;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle
{
	/// <summary>
	/// The lifecycle of <see cref="IService"/>s implementing this inteface is managed by the <see cref="ServiceLifecycleManagementFacility"/>.
	/// </summary>
	public interface IManagedLifecycleService : IDisposable, IService
	{
		#region Start Methods
		/// <summary>
		/// Invoked just before this service is used for the first time.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		void Start(INucleusAwareContext context);
		#endregion
	}
}