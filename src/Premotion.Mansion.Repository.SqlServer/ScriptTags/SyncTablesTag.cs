﻿using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.ScriptTags
{
	/// <summary>
	/// Syncs the nodes table to other tables.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "syncTables")]
	public class SyncTablesTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeService"></param>
		/// <param name="parser"> </param>
		/// <param name="repository"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public SyncTablesTag(ITypeService typeService, IQueryParser parser, SqlServerRepository repository)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");
			if (parser == null)
				throw new ArgumentNullException("parser");
			if (repository == null)
				throw new ArgumentNullException("repository");

			// set values
			this.typeService = typeService;
			this.parser = parser;
			this.repository = repository;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// perform the buld load operation
			repository.BulkOperation(bulkContext => {
				// loop over all the types
				foreach (var type in typeService.LoadAll(context))
				{
					// get the schema for this type
					var schema = Resolver.ResolveTypeOnly(context, type);

					// get the additional tables of this type, if the type has only the root table ignore it
					var tableList = schema.Tables.ToList();
					if (tableList.Count == 0)
						continue;

					// get the node entries for this type, ignore types with zero nodes
					var query = parser.Parse(context, new PropertyBag {
						{"baseType", type.Name},
						{"status", "any"},
						{"bypassAuthorization", true},
						{StorageOnlyQueryComponent.PropertyKey, true}
					});
					var recordSet = repository.Retrieve(context, query);
					if (recordSet.RowCount == 0)
						continue;

					// loop through all the tables except the root table
					var records = recordSet.Records.ToList();
					foreach (var table in tableList.Where(candidate => !(candidate is RootTable)))
					{
						// sync this table
						table.ToSyncStatement(context, bulkContext, records);
					}
				}
			});
		}
		#endregion
		#region Private Fields
		private readonly IQueryParser parser;
		private readonly SqlServerRepository repository;
		private readonly ITypeService typeService;
		#endregion
	}
}