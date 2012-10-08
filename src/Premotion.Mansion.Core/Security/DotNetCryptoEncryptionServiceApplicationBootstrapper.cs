using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Registers the <see cref="DotNetCryptoEncryptionService"/>.
	/// </summary>
	public class DotNetCryptoEncryptionServiceApplicationBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary></summary>
		public DotNetCryptoEncryptionServiceApplicationBootstrapper() : base(50)
		{
		}
		#endregion
		#region Overrides of ApplicationBootstrapper
		/// <summary>
		/// Registers all the services used by the application in the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> in which to register the services used by the application.</param>
		protected override void DoBoostrap(IConfigurableNucleus nucleus)
		{
			nucleus.Register<IEncryptionService>(resolver => DotNetCryptoEncryptionService.Create());
		}
		#endregion
	}
}