using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Registers all the services used by linking applications.
	/// </summary>
	public class LinkingApplicationBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public LinkingApplicationBootstrapper() : base(50)
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
			nucleus.Register<ILinkService>(resolver => new LinkService());
		}
		#endregion
	}
}