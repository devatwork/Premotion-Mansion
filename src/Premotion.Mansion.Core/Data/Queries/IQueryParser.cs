using System;

namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Parses parameters into a <see cref="Query"/>.
	/// </summary>
	public interface IQueryParser
	{
		#region Parse Methods
		/// <summary>
		/// Parses the given <paramref name="parameters"/> into a <see cref="Query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parameters">The parameters which to parse.</param>
		/// <returns>Returns the <see cref="Query"/>.</returns>
		/// <exception cref="ArgumentNullException">Throw if <paramref name="context"/> or <paramref name="parameters"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if any of the parameters could not be parsed into a query.</exception>
		Query Parse(IMansionContext context, IPropertyBag parameters);
		#endregion
	}
}