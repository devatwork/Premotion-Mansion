using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Linking.Descriptors;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Provides an implemenation of the <see cref="ILinkService"/>.
	/// </summary>
	public class LinkService : ILinkService
	{
		#region ILinkService Members
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
		public void Link(IMansionContext context, IPropertyBag source, IPropertyBag target, string name, IPropertyBag properties)
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

			// create the link
			UpdateLinkbase(context, source, target, (sourceLinkbase, targetLinkbase) => sourceLinkbase.Link(context, targetLinkbase, name, properties));
		}
		/// <summary>
		/// Removes a link between <paramref name="source"/> and <paramref name="target"/>. The link type is identified by <paramref name="name"/> and. Both reqcords must be saved after the link was removed.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="IPropertyBag"/> from which to remove the link.</param>
		/// <param name="target">The target <see cref="IPropertyBag"/> to which to remove the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the operation would have resulted in an invalid <see cref="Linkbase"/>.</exception>
		public void Unlink(IMansionContext context, IPropertyBag source, IPropertyBag target, string name)
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

			// destroy the link
			UpdateLinkbase(context, source, target, (sourceLinkbase, targetLinkbase) => sourceLinkbase.Unlink(context, targetLinkbase, name));
		}
		/// <summary>
		/// Gets the <see cref="Linkbase"/> of the given <paramref name="source"/>. The <see cref="Linkbase"/> must not be modified.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="IPropertyBag"/> for which to get the <see cref="Linkbase"/>.</param>
		/// <returns>Retrns the <see cref="Linkbase"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the <paramref name="source"/> does not have a <see cref="Linkbase"/>.</exception>
		public Linkbase GetLinkbase(IMansionContext context, IPropertyBag source)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");

			// load and return the linkbase
			return LoadLinkbase(context, source);
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Allows an update of both the <paramref name="source"/> and <paramref name="target"/> <see cref="Linkbase"/>s using <paramref name="func"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="IPropertyBag"/>.</param>
		/// <param name="target">The target <see cref="IPropertyBag"/>.</param>
		/// <param name="func">The action which to perform on the <see cref="Linkbase"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the operation would have resulted in an invalid <see cref="Linkbase"/>.</exception>
		private void UpdateLinkbase(IMansionContext context, IPropertyBag source, IPropertyBag target, Action<Linkbase, Linkbase> func)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (target == null)
				throw new ArgumentNullException("target");
			if (func == null)
				throw new ArgumentNullException("func");

			// load the source and target linkbases
			var sourceLinkbase = LoadLinkbase(context, source);
			var targetLinkbase = LoadLinkbase(context, target);

			// invoke the func
			func(sourceLinkbase, targetLinkbase);

			// store the source and target linkbase data
			SaveLinkbase(context, sourceLinkbase, source);
			SaveLinkbase(context, targetLinkbase, target);
		}
		/// <summary>
		/// Loads the <see cref="Linkbase"/> from <paramref name="source"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		private Linkbase LoadLinkbase(IMansionContext context, IPropertyBag source)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");

			// check if the type has got a linkbase
			ITypeDefinition type;
			if (!source.TryGet(context, "type", out type))
				throw new InvalidLinkException("Source does not contain type information and therfore can not contain a linkbase");
			LinkbaseDescriptor linkbaseDescriptor;
			if (!type.TryFindDescriptorInHierarchy(out linkbaseDescriptor))
				throw new InvalidLinkException(string.Format("Type '{0}' does not have a link base", type.Name));

			// get the definition of the linkbase
			var definition = linkbaseDescriptor.GetDefinition(context);

			// load the data
			LinkbaseData data;
			if (!source.TryGet(context, Constants.LinkbaseDataKey, out data))
				data = LinkbaseData.Create(context, source);

			// create the linkbase
			return new Linkbase(definition, data);
		}
		/// <summary>
		/// Saves the <paramref name="linkbase"/> into <paramref name="source"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="linkbase"></param>
		/// <param name="source"></param>
		private static void SaveLinkbase(IMansionContext context, Linkbase linkbase, IPropertyBag source)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (linkbase == null)
				throw new ArgumentNullException("linkbase");
			if (source == null)
				throw new ArgumentNullException("source");

			// set the link base data
			source.Set(Constants.LinkbaseDataKey, linkbase.Data);
		}
		#endregion
	}
}