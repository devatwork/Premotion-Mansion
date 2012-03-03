using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Implements the nucleus <see cref="ContextExtension"/>.
	/// </summary>
	public class NucleusContext : ContextExtension, INucleusAwareContext
	{
		#region Constructors
		/// <summary>
		/// Constructs a context extesion.
		/// </summary>
		/// <param name="originalContext">The original <see cref="IContext"/> being extended.</param>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		public NucleusContext(IContext originalContext, INucleus nucleus) : base(originalContext)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// set values
			Nucleus = nucleus;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="INucleus"/> of the current <see cref="IContext"/>.
		/// </summary>
		public INucleus Nucleus { get; private set; }
		#endregion
	}
}