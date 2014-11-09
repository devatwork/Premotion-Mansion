using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Scheduler
{
	/// <summary>
	/// Implements the <see cref="ApplicationBootstrapper"/> for the scheduler
	/// </summary>
	public class SchedulerpBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SchedulerpBootstrapper() : base(100)
		{
		}
		#endregion
		#region Overrides of ApplicationBootstrapper
		/// <summary>
		/// Registers all the services used by the application in the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> in which to register the services used by the application.</param>
		protected override void DoBootstrap(IConfigurableNucleus nucleus)
		{
			nucleus.Register<ISchedulerService>(resolver => new QuartzSchedulerService());
		}
		#endregion
	}
}