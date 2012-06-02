using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Initializers start the application before it is able to process anything. The initializers are invoked after the <see cref="ApplicationBootstrapper"/>s and should be used to initialize the application.
	/// </summary>
	[Exported(typeof (ApplicationInitializer))]
	public abstract class ApplicationInitializer
	{
		#region Constructors
		/// <summary>
		/// Constructs an application initializer.
		/// </summary>
		/// <param name="weight">The <see cref="Weight"/> of the bootstrapper.</param>
		protected ApplicationInitializer(int weight)
		{
			Weight = weight;
		}
		#endregion
		#region Initialize Methods
		/// <summary>
		/// Initializes the application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public void Initialize(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// invoke template method
			DoInitialize(context);
		}
		/// <summary>
		/// Initializes the application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected abstract void DoInitialize(IMansionContext context);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the weight of this initializer. The weight determines the order in which the <see cref="Initialize"/> method is executed.
		/// </summary>
		/// <remarks>
		/// The higher the weight the later the <see cref="Initialize"/> method is invoked.
		/// </remarks>
		public int Weight { get; private set; }
		#endregion
	}
}