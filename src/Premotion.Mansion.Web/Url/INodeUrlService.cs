using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Web.Url
{
	/// <summary>
	/// Generates and parses URLs for <see cref="Node"/>s.
	/// </summary>
	public interface INodeUrlService : IService
	{
		#region Generate Methods
		/// <summary>
		/// Generates a URL for <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <returns>Returns the generated <see cref="Uri"/>.</returns>
		Uri Generate(MansionWebContext context, Node node);
		#endregion
		#region Parse Methods
		/// <summary>
		/// Parses the URL into query parameters.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="url">The <see cref="Uri"/> which to parse.</param>
		/// <param name="queryParameters">The query parameters extracted from <paramref name="url"/>.</param>
		/// <returns>Returns true when parameters could be extracted, otherwise false.</returns>
		bool TryExtractQueryParameters(MansionContext context, Uri url, out IPropertyBag queryParameters);
		#endregion
	}
}