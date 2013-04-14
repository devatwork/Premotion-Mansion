using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// The link service provides methods to modify the linkbase of <see cref="Record"/>s.
	/// </summary>
	public interface ILinkService
	{
		/// <summary>
		/// Creates a link between <paramref name="source"/> and <paramref name="target"/>. The link type is identified by <paramref name="name"/> and. Both reqcords must be saved after the link was created.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="IPropertyBag"/> from which to create the link.</param>
		/// <param name="target">The target <see cref="IPropertyBag"/> to which to create the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the operation would have resulted in an invalid <see cref="Linkbase"/>.</exception>
		void Link(IMansionContext context, IPropertyBag source, IPropertyBag target, string name, IPropertyBag properties);
		/// <summary>
		/// Removes a link between <paramref name="source"/> and <paramref name="target"/>. The link type is identified by <paramref name="name"/> and. Both reqcords must be saved after the link was removed.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="IPropertyBag"/> from which to remove the link.</param>
		/// <param name="target">The target <see cref="IPropertyBag"/> to which to remove the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the operation would have resulted in an invalid <see cref="Linkbase"/>.</exception>
		void Unlink(IMansionContext context, IPropertyBag source, IPropertyBag target, string name);
		/// <summary>
		/// Gets the <see cref="Linkbase"/> of the given <paramref name="source"/>. The <see cref="Linkbase"/> must not be modified.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="IPropertyBag"/> for which to get the <see cref="Linkbase"/>.</param>
		/// <returns>Retrns the <see cref="Linkbase"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the <paramref name="source"/> does not have a <see cref="Linkbase"/>.</exception>
		Linkbase GetLinkbase(IMansionContext context, IPropertyBag source);
	}
}