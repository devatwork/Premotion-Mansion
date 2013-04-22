using System;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.KnowledgeOrganization
{
	/// <summary>
	/// Registers all the services used by knowledge organization applications.
	/// </summary>
	public class KnowledgeOrganizationApplicationBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public KnowledgeOrganizationApplicationBootstrapper() : base(50)
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
		}
		#endregion
	}
}