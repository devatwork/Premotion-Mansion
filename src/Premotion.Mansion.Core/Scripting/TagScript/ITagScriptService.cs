using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Defines the interface for <see cref="ITagScript"/> services.
	/// </summary>
	public interface ITagScriptService : IScriptingService<ITagScript>
	{
		#region Open Methods
		/// <summary>
		/// Opens a series of scripts.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resources">The resources which to open.</param>
		/// <returns>Returns a marker which will close the scripts automatically.</returns>
		IDisposable Open(IMansionContext context, IEnumerable<IResource> resources);
		#endregion
	}
}