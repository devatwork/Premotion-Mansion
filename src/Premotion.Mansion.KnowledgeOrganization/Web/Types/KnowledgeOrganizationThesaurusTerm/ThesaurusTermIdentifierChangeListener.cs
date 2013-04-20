using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;

namespace Premotion.Mansion.KnowledgeOrganization.Web.Types.KnowledgeOrganizationThesaurusTerm
{
	/// <summary>
	/// This listener manages the name of this thesaurus term.
	/// </summary>
	public class ThesaurusTermIdentifierChangeListener : NodeListener
	{
		/// <summary>
		/// This method is called just before a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The new properties of the node.</param>
		protected override void DoBeforeCreate(IMansionContext context, IPropertyBag properties)
		{
			base.DoBeforeCreate(context, properties);

			// make sure the idenitifier is lowwercase
			var identifier = properties.Get<string>(context, "identifier").Trim().ToLower();
			properties.Set("identifier", identifier);

			// make sure there is a name
			var name = properties.Get(context, "name", identifier).Trim().ToLower();
			properties.Set("name", name);
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

			// check if the identifier was updated
			string identifier;
			if (!properties.TryGet(context, "identifier", out identifier))
				return;
			identifier = identifier.Trim().ToLower();
			properties.Set("identifier", identifier);

			// only update the name with the identifier if the previous name was also derived from the identifier
			if (record.Get(context, "name", string.Empty).Equals(record.Get(context, "identifier", string.Empty)))
			{
				var name = properties.Get(context, "name", identifier).Trim().ToLower();
				properties.Set("name", name);
			}
		}
	}
}