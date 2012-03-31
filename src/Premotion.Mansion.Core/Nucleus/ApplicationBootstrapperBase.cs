using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Bootstrappers configure the application before it is started.
	/// </summary>
	public abstract class ApplicationBootstrapperBase
	{
		#region Initialize Methods
		/// <summary>
		/// Initializes the application using the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> from which to configure the application.</param>
		public void Initialize(IConfigurableNucleus nucleus)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// invoke template method
			DoInitialize(nucleus);
		}
		/// <summary>
		/// Initializes the application using the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> from which to configure the application.</param>
		protected abstract void DoInitialize(IConfigurableNucleus nucleus);
		#endregion
	}
}