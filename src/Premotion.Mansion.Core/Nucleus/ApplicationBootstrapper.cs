using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Bootstrappers configure the application before it is started.
	/// </summary>
	[Exported(typeof (ApplicationBootstrapper))]
	public abstract class ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// Constructs an application bootstrapper.
		/// </summary>
		/// <param name="weight">The <see cref="Weight"/> of the bootstrapper.</param>
		protected ApplicationBootstrapper(int weight)
		{
			Weight = weight;
		}
		#endregion
		#region Boostrap Methods
		/// <summary>
		/// Registers all the services used by the application in the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> in which to register the services used by the application.</param>
		public void Bootstrap(IConfigurableNucleus nucleus)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// invoke template method
			DoBoostrap(nucleus);
		}
		/// <summary>
		/// Registers all the services used by the application in the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> in which to register the services used by the application.</param>
		protected abstract void DoBoostrap(IConfigurableNucleus nucleus);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the weight of this bootstrapper. The weight determines the order in which the <see cref="Bootstrap"/> method is executed.
		/// </summary>
		/// <remarks>
		/// The higher the weight the later the <see cref="Bootstrap"/> method is invoked.
		/// </remarks>
		public int Weight { get; private set; }
		#endregion
	}
}