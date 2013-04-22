using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;

namespace Premotion.Mansion.KnowledgeOrganization.Web.Types.KnowledgeOrganizationThesaurusTerm
{
	/// <summary>
	/// This listener manages the synonym relation of this thesaurus term.
	/// </summary>
	public class ThesaurusTermPreferredTermChangeListener : NodeListener
	{
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

			// store the guid in the synonymGuids field
			context.Repository.UpdateNode(context, newPreferredTermNode, new PropertyBag {
				{"synonymGuids", newPreferredTermNode.Get(context, "synonymGuids", string.Empty).AppendNeedle(record.Get<string>(context, "guid"))}
			});
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
			var recordGuidString = record.Get<string>(context, "guid");
			Guid currentPreferredTermGuid;
			if (record.TryGet(context, "preferredTermGuid", out currentPreferredTermGuid) && currentPreferredTermGuid != Guid.Empty)
			{
				// retrieve the old record
				var currentPreferredTermNode = context.Repository.RetrieveSingleNode(context, currentPreferredTermGuid);

				// remove the link, if the target was found
				if (currentPreferredTermNode != null)
				{
					// store the guid in the synonymGuids field
					context.Repository.UpdateNode(context, currentPreferredTermNode, new PropertyBag {
						{"synonymGuids", currentPreferredTermNode.Get(context, "synonymGuids", string.Empty).RemoveNeedle(recordGuidString)}
					});
				}
			}

			// check if there is no new synonym
			if (newPreferredTermGuid == Guid.Empty)
				return;

			// retrieve the preferredTermNode
			var newPreferredTermNode = context.Repository.RetrieveSingleNode(context, newPreferredTermGuid);
			if (newPreferredTermNode == null)
				return;

			// store the guid in the synonymGuids field
			context.Repository.UpdateNode(context, newPreferredTermNode, new PropertyBag {
				{"synonymGuids", newPreferredTermNode.Get(context, "synonymGuids", string.Empty).AppendNeedle(recordGuidString)}
			});
		}
		#endregion
	}
}