using System;

namespace Premotion.Mansion.Core.Conversion.Comparers
{
	/// <summary>
	/// Implements <see cref="ComparerBase{TType}"/> for <see cref="Uri"/>.
	/// </summary>
	public class UriComparer : ComparerBase<Uri>
	{
		#region Overrides of ComparerBase<Uri>
		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="left">The left-hand object which to compare.</param>
		/// <param name="right">The right-hand object which to compare.</param>
		/// <returns>Returns 0 when the objects are equal, greater than zero when <paramref name="left"/> is greater than <paramref name="right"/>, or less than zero when <paramref name="left"/> is smaller than <paramref name="right"/>.</returns>
		protected override int Compare(IMansionContext context, Uri left, Uri right)
		{
			return Uri.Compare(left, right, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);
		}
		#endregion
	}
}