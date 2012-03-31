using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
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
		/// <exception cref="ArgumentNullException"></exception>
		public SyncTablesTag(ITypeService typeService)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the SQL Server repo
			var repository = context.GetUnwrappedRepository() as SqlServerRepository;
			if (repository == null)
				throw new InvalidOperationException("The top-most repository is not a SQL server repository");

			// perform the buld load operation
			repository.BulkOperation(bulkContext =>
			                         {
			                         	// loop over all the types
			                         	foreach (var type in typeService.LoadAll(context))
			                         	{
			                         		// get the schema for this type
			                         		var typeSchema = SchemaProvider.Resolve(context, type);

			                         		// get the additional tables of this type, if the type has only the root table ignore it
			                         		var tableList = typeSchema.OwnedTables.ToList();
			                         		if (tableList.Count == 0)
			                         			continue;

			                         		// get the node entries for this type, ignore types with zero nodes
			                         		var nodeset = repository.Retrieve(context, repository.ParseQuery(context, new PropertyBag
			                         		                                                                          {
			                         		                                                                          	{"baseType", type.Name}
			                         		                                                                          }));
			                         		if (nodeset.RowCount == 0)
			                         			continue;

			                         		// loop through all the tables except the root table
			                         		foreach (var table in tableList)
			                         		{
			                         			// sync this table
			                         			table.ToSyncStatement(context, bulkContext, nodeset.Nodes.ToList());
			                         		}
			                         	}
			                         });
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}