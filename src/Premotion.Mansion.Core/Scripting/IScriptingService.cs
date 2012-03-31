using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Scripting
{
	/// <summary>
	/// Represents the script service.
	/// </summary>
	public interface IScriptingService<out TScript> where TScript : IScript
	{
		#region Parse Methods
		/// <summary>
		/// Parses a script from the specified resource.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resource">The resource wcich to parse as script.</param>
		/// <returns>Returns the parsed script.</returns>
		/// <exception cref="ParseScriptException">Thrown when an exception occurres while parsing the script.</exception>
		TScript Parse(IMansionContext context, IResource resource);
		#endregion
	}
}