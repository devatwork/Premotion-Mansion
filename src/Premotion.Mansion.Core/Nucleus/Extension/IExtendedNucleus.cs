using Premotion.Mansion.Core.Nucleus.Configuration;

namespace Premotion.Mansion.Core.Nucleus.Extension
{
	/// <summary>
	/// Defines the interface for extendable <see cref="INucleus"/>es.
	/// </summary>
	public interface IExtendedNucleus : IConfigurableNucleus
	{
		#region Events
		/// <summary>
		/// Fired just before a service is registered in this <see cref="INucleus"/>.
		/// </summary>
		event ServiceDelegate ServiceRegistering;
		/// <summary>
		/// Fired just after a service is registered in this <see cref="INucleus"/>.
		/// </summary>
		event ServiceDelegate ServiceRegistered;
		/// <summary>
		/// Fired just before a service is started by this <see cref="INucleus"/>.
		/// </summary>
		event ServiceDelegate ServiceActivating;
		/// <summary>
		/// Fired just after a service is started by this <see cref="INucleus"/>.
		/// </summary>
		event ServiceDelegate ServiceActivated;
		#endregion
	}
	/// <summary>
	/// Represents a delegate which holds a <see cref="IServiceModel"/>.
	/// </summary>
	/// <param name="context">The <see cref="NucleusContext"/> in which the event was triggered.</param>
	/// <param name="model">The <see cref="IServiceModel"/> which triggered the event.</param>
	public delegate void ServiceDelegate(NucleusContext context, IServiceModel model);
}