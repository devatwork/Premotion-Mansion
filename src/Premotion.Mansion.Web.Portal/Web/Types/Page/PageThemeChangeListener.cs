﻿using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;
using Premotion.Mansion.Web.Portal.Descriptors;

namespace Premotion.Mansion.Web.Portal.Web.Types.Page
{
	/// <summary>
	/// This listener manages the position of blocks when a page is changing theme.
	/// </summary>
	public class PageThemeChangeListener : NodeListener
	{
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The updated properties of the node.</param>
		protected override void DoBeforeUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			// get the variables
			string currentTheme;
			var hasCurrentTheme = record.TryGet(context, "theme", out currentTheme) && !string.IsNullOrEmpty(currentTheme);
			string newTheme;
			var hasNewTheme = properties.TryGet(context, "theme", out newTheme) && !string.IsNullOrEmpty(newTheme);

			// do nothing when the page does not have a theme yet
			if (!hasCurrentTheme)
				return;

			// retrieve the schema of the current theme
			var currentThemeSchema = ColumnSchema.GetSchema(context, currentTheme);

			// retrieve the blocks of this page
			var repository = context.Repository;
			var blockNodeset = repository.RetrieveNodeset(context, new PropertyBag
			                                                       {
			                                                       	{"baseType", "Block"},
			                                                       	{"parentSource", record}
			                                                       });

			// check if a new theme is selected
			if (hasNewTheme)
			{
				// retrieve the schema of the new theme
				var newThemeSchema = ColumnSchema.GetSchema(context, newTheme);

				// loop through the blocks to find obsolete ones
				foreach (var blockNode in blockNodeset.Nodes)
				{
					// get the column of this block
					var column = blockNode.Get<string>(context, "column");

					// check if this block lived in the old theme
					if (!currentThemeSchema.ContainsColumn(column))
						continue;

					// check if the column exists in the new theme as well
					if (newThemeSchema.ContainsColumn(column))
						continue;

					// block is obsolete delete it
					repository.DeleteNode(context, blockNode);
				}
			}
			else
			{
				// theme is removed, delete all the theme blocks
				foreach (var blockNode in blockNodeset.Nodes.Where(candidate => currentThemeSchema.ContainsColumn(candidate.Get<string>(context, "column"))))
					repository.DeleteNode(context, blockNode);
			}

			base.DoBeforeUpdate(context, record, properties);
		}
	}
}