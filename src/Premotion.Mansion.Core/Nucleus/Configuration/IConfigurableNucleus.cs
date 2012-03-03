using System;
using Premotion.Mansion.Core.Nucleus.Extension;

namespace Premotion.Mansion.Core.Nucleus.Configuration
{
	/// <summary>
	/// Defines the interface for configurable <see cref="INucleus"/>es.
	/// </summary>
	public interface IConfigurableNucleus : INucleus
	{
		#region Register Methods
		/// <summary>
		/// Registers the <paramref name="instance"/> to this <see cref="INucleus"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of <see cref="IService"/> which to register.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="instance">The instance of <see cref="IService"/> which to register.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is null.</exception>
		void Register<TContract>(IContext context, TContract instance) where TContract : class, IService;
		/// <summary>
		/// Augments this <see cref="INucleus"/> with a new <paramref name="facility"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="facility">The <see cref="IFacility"/>.</param>
		void Augment(IContext context, IFacility facility);
		#endregion
	}
}