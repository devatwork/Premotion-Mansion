using System;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Represents an extension of the context.
	/// </summary>
	public abstract class MansionContextExtension : MansionContextDecorator
	{
		#region Constructors
		/// <summary>
		/// Constructs the mansion context decorator.
		/// </summary>
		/// <param name="decoratedContext">The <see cref="IMansionContext"/> being decorated.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="decoratedContext"/> is null.</exception>
		protected MansionContextExtension(IMansionContext decoratedContext) : base(decoratedContext)
		{
		}
		#endregion
	}
}