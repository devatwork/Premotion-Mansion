using System;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Represents an executable tag script.
	/// </summary>
	public interface ITagScript : IScript, IDisposable
	{
		#region Initialize Methods
		/// <summary>
		/// Initializes this script.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		void Initialize(IMansionContext context);
		#endregion
	}
}