using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;
using Premotion.Mansion.Linking;

namespace Premotion.Mansion.KnowledgeOrganization.Web.Types.ThesaurusTerm
{
	/// <summary>
	/// This listener manages the synonym relation of this thesaurus term.
	/// </summary>
	public class ThesaurusTermPreferredTermChangeListener : NodeListener
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="linkService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ThesaurusTermPreferredTermChangeListener(ILinkService linkService)
		{
			// validate arguments
			if (linkService == null)
				throw new ArgumentNullException("linkService");

			// set the value
			this.linkService = linkService;
		}
		#endregion
		#region Overrides of NodeListener
		/// <summary>
		/// This method is called just after a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The properties from which the <paramref name="record"/> was constructed.</param>
		protected override void DoAfterCreate(IMansionContext context, Record record, IPropertyBag properties)
		{
			base.DoAfterCreate(context, record, properties);

			// check if a preferred term was specified
			Guid newPreferredTermGuid;
			if (!record.TryGet(context, "preferredTermGuid", out newPreferredTermGuid) || newPreferredTermGuid == Guid.Empty)
				return;

			// retrieve the preferredTermNode
			var newPreferredTermNode = context.Repository.RetrieveSingleNode(context, newPreferredTermGuid);
			if (newPreferredTermNode == null)
				return;

			// create the new link
			var linkProperties = new PropertyBag();
			LinkHelper.CopyLinkEndsProperties(context, newPreferredTermNode, record, linkProperties);
			linkService.Link(context, newPreferredTermNode, record, Constants.SynonymLinkName, linkProperties);

			// store the record
			context.Repository.Update(context, record, properties);

			// update the target node as well
			var newPreferredTermProperties = new PropertyBag();
			LinkHelper.CopyLinkbase(context, newPreferredTermNode, newPreferredTermProperties);
			context.Repository.UpdateNode(context, newPreferredTermNode, newPreferredTermProperties);
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The updated properties of the node.</param>
		protected override void DoBeforeUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			base.DoBeforeUpdate(context, record, properties);

			// check if the preferred term has changed
			Guid newPreferredTermGuid;
			if (!properties.TryGet(context, "preferredTermGuid", out newPreferredTermGuid))
				return;

			// delete the old link if it exists
			Guid currentPreferredTermGuid;
			if (record.TryGet(context, "preferredTermGuid", out currentPreferredTermGuid) && currentPreferredTermGuid != Guid.Empty)
			{
				// retrieve the old record
				var currentPreferredTermNode = context.Repository.RetrieveSingleNode(context, currentPreferredTermGuid);

				// remove the link, if the target was found
				if (currentPreferredTermNode != null)
					linkService.Unlink(context, currentPreferredTermNode, record, Constants.SynonymLinkName);
			}

			// check if there is no new synonym
			if (newPreferredTermGuid == Guid.Empty)
				return;

			// retrieve the preferredTermNode
			var newPreferredTermNode = context.Repository.RetrieveSingleNode(context, newPreferredTermGuid);
			if (newPreferredTermNode == null)
				return;

			// create the new link
			var linkProperties = new PropertyBag();
			LinkHelper.CopyLinkEndsProperties(context, newPreferredTermNode, record, linkProperties);
			linkService.Link(context, newPreferredTermNode, record, Constants.SynonymLinkName, linkProperties);

			// copy the linkbase to the properties to its gets stored
			LinkHelper.CopyLinkbase(context, record, properties);

			// update the target node as well
			var newPreferredTermProperties = new PropertyBag();
			LinkHelper.CopyLinkbase(context, newPreferredTermNode, newPreferredTermProperties);
			context.Repository.UpdateNode(context, newPreferredTermNode, newPreferredTermProperties);
		}
		#endregion
		#region Private Fields
		private readonly ILinkService linkService;
		#endregion
	}
}