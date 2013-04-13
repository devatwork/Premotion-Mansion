using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Provides an implemenation of the <see cref="ILinkService"/>.
	/// </summary>
	public class LinkService : ILinkService
	{
		#region Constructors
		/// <summary>
		/// Constructs the link service.
		/// </summary>
		/// <param name="typeService"></param>
		public LinkService(ITypeService typeService)
		{
			//  validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
		}
		#endregion
		#region ILinkService Members
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
		public void Link(IMansionContext context, Record source, Record target, string name, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (target == null)
				throw new ArgumentNullException("target");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// get the link bases
			var sourceLinkbase = source.GetLinkbase(context, typeService);
			var targetLinkbase = source.GetLinkbase(context, typeService);

			// create the link
			sourceLinkbase.Link(context, targetLinkbase, name, properties);
		}
		/// <summary>
		/// Removes a link between <paramref name="source"/> and <paramref name="target"/>. The link type is identified by <paramref name="name"/> and. Both reqcords must be saved after the link was removed.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Record"/> from which to remove the link.</param>
		/// <param name="target">The target <see cref="Record"/> to which to remove the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		public void Unlink(IMansionContext context, Record source, Record target, string name)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (target == null)
				throw new ArgumentNullException("target");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// get the link bases
			var sourceLinkbase = source.GetLinkbase(context, typeService);
			var targetLinkbase = source.GetLinkbase(context, typeService);

			// destroy the link
			sourceLinkbase.Unlink(context, targetLinkbase, name);
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}