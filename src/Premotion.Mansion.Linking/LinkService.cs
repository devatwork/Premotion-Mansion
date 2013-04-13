using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Linking.Descriptors;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Provides an implemenation of the <see cref="ILinkService"/>.
	/// </summary>
	public class LinkService : ILinkService
	{
		#region Constants
		private const string LinkbaseDataKey = "link:linkbasedata";
		#endregion
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
		/// <exception cref="InvalidLinkException">Thrown if the operation would have resulted in an invalid <see cref="Linkbase"/>.</exception>
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

			// create the link
			UpdateLinkbase(context, source, target, (sourceLinkbase, targetLinkbase) => sourceLinkbase.Link(context, targetLinkbase, name, properties));
		}
		/// <summary>
		/// Removes a link between <paramref name="source"/> and <paramref name="target"/>. The link type is identified by <paramref name="name"/> and. Both reqcords must be saved after the link was removed.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Record"/> from which to remove the link.</param>
		/// <param name="target">The target <see cref="Record"/> to which to remove the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the operation would have resulted in an invalid <see cref="Linkbase"/>.</exception>
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

			// destroy the link
			UpdateLinkbase(context, source, target, (sourceLinkbase, targetLinkbase) => sourceLinkbase.Unlink(context, targetLinkbase, name));
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Allows an update of both the <paramref name="source"/> and <paramref name="target"/> <see cref="Linkbase"/>s using <paramref name="func"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Record"/>.</param>
		/// <param name="target">The target <see cref="Record"/>.</param>
		/// <param name="func">The action which to perform on the <see cref="Linkbase"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the operation would have resulted in an invalid <see cref="Linkbase"/>.</exception>
		private void UpdateLinkbase(IMansionContext context, Record source, Record target, Action<Linkbase, Linkbase> func)
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
		/// Loads the <see cref="Linkbase"/> from <paramref name="record"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		/// <returns></returns>
		private Linkbase LoadLinkbase(IMansionContext context, Record record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");

			// check if the type has got a linkbase
			var type = typeService.Load(context, record.Type);
			LinkbaseDescriptor linkbaseDescriptor;
			if (!type.TryFindDescriptorInHierarchy(out linkbaseDescriptor))
				throw new InvalidLinkException(string.Format("Type '{0}' does not have a link base", record.Type));

			// get the definition of the linkbase
			var definition = linkbaseDescriptor.GetDefinition(context);

			// load the data
			LinkbaseData data;
			if (!record.TryGet(context, LinkbaseDataKey, out data))
				data = LinkbaseData.Create(context, record);

			// create the linkbase
			return new Linkbase(definition, data);
		}
		/// <summary>
		/// Saves the <paramref name="linkbase"/> into <paramref name="record"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="linkbase"></param>
		/// <param name="record"></param>
		private static void SaveLinkbase(IMansionContext context, Linkbase linkbase, IPropertyBag record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (linkbase == null)
				throw new ArgumentNullException("linkbase");
			if (record == null)
				throw new ArgumentNullException("record");

			// set the link base data
			record.Set(LinkbaseDataKey, linkbase.Data);
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}