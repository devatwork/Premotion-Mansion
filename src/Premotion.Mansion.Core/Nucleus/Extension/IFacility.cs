using System;

namespace Premotion.Mansion.Core.Nucleus.Extension
{
	/// <summary>
	/// Base inteface for all nucleus facilities. Facilities are extensions to the nucleus.
	/// </summary>
	public interface IFacility : IDisposable
	{
		#region Activation Methods
		/// <summary>
		/// Activates this facility in the <paramref name="nucleus"/>. This event it typically used to register listeners to nucleus events.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="nucleus">The <see cref="IExtendedNucleus"/> in which this facility is activated.</param>
		void Activate(IContext context, IExtendedNucleus nucleus);
		#endregion
	}
}