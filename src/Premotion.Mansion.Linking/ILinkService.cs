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
		/// <param name="source">The source <see cref="Record"/> from which to create the link.</param>
		/// <param name="target">The target <see cref="Record"/> to which to create the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the specified link could not be created.</exception>
		void Link(IMansionContext context, Record source, Record target, string name, IPropertyBag properties);
		/// <summary>
		/// Removes a link between <paramref name="source"/> and <paramref name="target"/>. The link type is identified by <paramref name="name"/> and. Both reqcords must be saved after the link was removed.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Record"/> from which to remove the link.</param>
		/// <param name="target">The target <see cref="Record"/> to which to remove the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		void Unlink(IMansionContext context, Record source, Record target, string name);
	}
}