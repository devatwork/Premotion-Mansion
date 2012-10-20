using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Web.Converters
{
	/// <summary>
	/// Implements <see cref="ComparerBase{TType}"/> for <see cref="Uri"/>.
	/// </summary>
	public class UrlComparer : ComparerBase<Url>
	{
		#region Overrides of ComparerBase<Url>
		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="left">The left-hand object which to compare.</param>
		/// <param name="right">The right-hand object which to compare.</param>
		/// <returns>Returns 0 when the objects are equal, greater than zero when <paramref name="left"/> is greater than <paramref name="right"/>, or less than zero when <paramref name="left"/> is smaller than <paramref name="right"/>.</returns>
		protected override int Compare(IMansionContext context, Url left, Url right)
		{
			return string.CompareOrdinal(left.ToString().ToLower(), right.ToString().ToLower());
		}
		#endregion
	}
}