using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Repository.SqlServer.ScriptTags
{
	/// <summary>
	/// Verifies and fixes the integrity of the SQL server repository.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "verifyRepositoryIntegrity")]
	public class VerifyRepositoryIntegrityTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public VerifyRepositoryIntegrityTag(SqlServerRepository repository)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");

			// set values
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
			// get the fix flag
			var fix = GetAttribute(context, "fix", false);

			// create the report dataspace
			var report = new PropertyBag();

			// verify
			report.Set("result", VerifyIntegrity(context, fix));

			// push the report to the stack
			using (context.Stack.Push("Report", report))
				ExecuteChildTags(context);
		}
		/// <summary>
		/// Verifies the integrity of the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="fix">Flag indicating whether the repository will be fixed immediately.</param>
		/// <returns>Returns a report.</returns>
		private string VerifyIntegrity(IMansionContext context, bool fix)
		{
			// create the report builder
			var reportBuilder = new StringBuilder();

			// first make sure the root node exists
			VerifyRootNodeExists(context, fix, reportBuilder);

			// then make sure there are not missing nodes
			VerifyMissingParents(context, fix, reportBuilder);

			// fix the depth of the nodes
			for (var depth = 1; depth <= 20; depth++)
				VerifyNodesAtDepth(context, fix, reportBuilder, depth);

			// return the report
			reportBuilder.AppendLine("Finished!");
			return reportBuilder.ToString();
		}
		/// <summary>
		/// Verifies the root node exists.
		/// </summary>
		private void VerifyRootNodeExists(IMansionContext context, bool fix, StringBuilder reportBuilder)
		{
			// try to retrieve the root node
			var rootNode = repository.RetrieveSingleNode(context, new Query()
				                                                      .Add(new IsPropertyEqualSpecification("id", 1))
				                                                      .Add(StatusSpecification.Is(NodeStatus.Any))
				                                                      .Add(AllowedRolesSpecification.Any())
				                                                      .Add(new StorageOnlyQueryComponent())
				);
			if (rootNode != null)
				reportBuilder.AppendLine("Succes: Root node was found");
			else
			{
				if (fix)
				{
					repository.ExecuteWithoutTransaction(context, @"-- Insert root node
	SET IDENTITY_INSERT [dbo].[Nodes] ON;
	INSERT INTO [dbo].[Nodes] ([id],[name],[type],[depth]) VALUES ('1', 'Root', 'root', '1');
	SET IDENTITY_INSERT [dbo].[Nodes] OFF;");
					reportBuilder.AppendLine("Succes: created root node");
				}
				else
					reportBuilder.AppendLine("Error: Root node was not found");
			}
		}
		/// <summary>
		/// Checks whether there are nodes who don't have a parent.
		/// </summary>
		private void VerifyMissingParents(IMansionContext context, bool fix, StringBuilder reportBuilder)
		{
			// build the sql
			const string checkIntegritySql = "SELECT DISTINCT	[Nodes].[parentPath]," +
			                                 "						[Nodes].[parentStructure]," +
			                                 "						[Nodes].[parentPointer]," +
			                                 "						[Nodes].[parentId]" +
			                                 "FROM [Nodes]" +
			                                 "WHERE" +
			                                 "		[Nodes].[parentId] > 1" +
			                                 "	AND" +
			                                 "		[Nodes].[parentId] NOT IN ( SELECT [id] FROM [Nodes] )";

			// retrieve the results
			var dataset = repository.RetrieveDataset(context, checkIntegritySql);

			// report the number of missing parents
			reportBuilder.AppendLine(string.Format("{0}: Found {1} of missing parents", dataset.RowCount == 0 ? "Succes" : "Error", dataset.RowCount));

			// loop over all the records with missing parents
			foreach (var row in dataset.Rows)
			{
				// Split
				var ids = Convert.ToString(row.Get<string>(context, "parentPointer")).Split(new[] {NodePointer.PointerSeparator}, StringSplitOptions.RemoveEmptyEntries);
				var names = Convert.ToString(row.Get<string>(context, "parentPath")).Split(new[] {NodePointer.PathSeparator}, StringSplitOptions.RemoveEmptyEntries);
				var types = Convert.ToString(row.Get<string>(context, "parentStructure")).Split(new[] {NodePointer.StructureSeparator}, StringSplitOptions.RemoveEmptyEntries);

				// Fix
				if (!fix)
				{
					// Report message
					reportBuilder.AppendLine(string.Format("Error: Found missing node: path={0}, structure={1}, pointer={2}", string.Join(NodePointer.PathSeparator, names), string.Join(NodePointer.StructureSeparator, types), string.Join(NodePointer.PointerSeparator, ids)));
				}
				else
				{
					// Loop over all parents from depth = 1, depth = 2, etc.
					string id, name, type, parentId;
					for (var depth = 1; depth <= ids.Length; depth++)
					{
						// Check depth
						if (depth < ids.Length)
						{
							// Get id from pointer
							id = ids[depth - 1];
						}
						else
						{
							// Get parent id ( pointer may be incorrect )
							id = row.Get<string>(context, "parentId");
						}

						// Get name, type
						name = names[depth - 1];
						type = types[depth - 1];

						// Check node
						var node = repository.RetrieveSingleNode(context, new Query()
							                                                  .Add(new IsPropertyEqualSpecification("id", id))
							                                                  .Add(StatusSpecification.Is(NodeStatus.Any))
							                                                  .Add(AllowedRolesSpecification.Any())
							                                                  .Add(new StorageOnlyQueryComponent())
							);
						if (node == null)
						{
							// Get parent id
							parentId = depth == 1 ? "1" : ids[depth - 2];

							// Get parent node
							var parentNode = repository.RetrieveSingleNode(context, new Query()
								                                                        .Add(new IsPropertyEqualSpecification("id", parentId))
								                                                        .Add(StatusSpecification.Is(NodeStatus.Any))
								                                                        .Add(AllowedRolesSpecification.Any())
								                                                        .Add(new StorageOnlyQueryComponent())
								);

							// Set properties
							var newProperties = new PropertyBag {
								{"name", name},
								{"type", type},
								{"id", id},
								{"_allowIdenityInsert", true}
							};

							// Re-insert node
							repository.CreateNode(context, parentNode, newProperties);

							// Report message
							reportBuilder.AppendLine(string.Format("Succes: Fixed missing node: name={0}, type={1}, id={2}", name, type, id));
						}
					}
				}
			}
		}
		/// <summary>
		/// Verifies the relation integrity of the nodes at the given depth.
		/// </summary>
		private void VerifyNodesAtDepth(IMansionContext context, bool fix, StringBuilder reportBuilder, int depth)
		{
			// Find nodes at depth > 2 that are not correct
			var checkIntegritySql = @"	SELECT Nodes.id, 
												Nodes.name,
												Nodes.parentPath, 
												Nodes.parentStructure, 
												Nodes.parentPointer, 
												Nodes.depth, 
												Nodes.parentId,
												Parents.parentPath AS p_parentPath, 
												Parents.parentStructure AS p_parentStructure, 
												Parents.parentPointer AS p_parentPointer, 
												Parents.depth AS p_depth, 
												Parents.id AS p_id, 
												Parents.name AS p_name,             
												Parents.type AS p_type    
										FROM Nodes
										INNER JOIN NODES AS Parents
										ON Nodes.parentId = Parents.id
										WHERE Parents.depth = {0} AND 
										( 
												( ISNULL( Nodes.parentPath, '' ) <> Parents.parentPath + ISNULL( Parents.name, '' ) + '{1}' )
											OR ( ISNULL( Nodes.parentStructure, '' ) <> Parents.parentStructure + ISNULL( Parents.type, '' ) + '{2}' )
											OR ( ISNULL( Nodes.parentPointer, '' ) <> Parents.parentPointer + CAST( Parents.id AS varchar ) + '{3}' )
											OR ( Nodes.depth <> Parents.depth + 1 )
										)";
			checkIntegritySql = checkIntegritySql.Format(depth - 1, NodePointer.PathSeparator, NodePointer.StructureSeparator, NodePointer.PointerSeparator);

			// retrieve the results
			var dataset = repository.RetrieveDataset(context, checkIntegritySql);

			// report the number of missing parents
			reportBuilder.AppendLine(string.Format("{0}: Found {1} incorrect nodes on depth {2}", dataset.RowCount == 0 ? "Succes" : "Error", dataset.RowCount, depth));

			// loop over all the records with missing parents
			foreach (var row in dataset.Rows)
			{
				if (!fix)
				{
					// Report message
					reportBuilder.AppendLine(string.Format("Error: Found incorrect node: name={0}, type={1}, id={2}", row.Get<string>(context, "name"), row.Get<string>(context, "type"), row.Get<string>(context, "id")));
				}
				else
				{
					// retrieve the node
					var node = repository.RetrieveSingleNode(context, new Query()
						                                                  .Add(new IsPropertyEqualSpecification("id", row.Get<string>(context, "id")))
						                                                  .Add(StatusSpecification.Is(NodeStatus.Any))
						                                                  .Add(AllowedRolesSpecification.Any())
						                                                  .Add(new StorageOnlyQueryComponent())
						);

					// calculate the proper values
					var parentPath = row.Get<string>(context, "p_parentPath") + row.Get<string>(context, "p_name") + NodePointer.PathSeparator;
					var parentStructure = row.Get<string>(context, "p_parentStructure") + row.Get<string>(context, "p_type").ToLower() + NodePointer.StructureSeparator;
					var parentPointer = row.Get<string>(context, "p_parentPointer") + row.Get<string>(context, "p_id") + NodePointer.PointerSeparator;
					var parentDepth = row.Get<int>(context, "p_depth");

					// update the node
					repository.UpdateNode(context, node, new PropertyBag {
						{"parentPath", parentPath},
						{"parentStructure", parentStructure},
						{"parentPointer", parentPointer},
						{"depth", parentDepth + 1},
						{"_allowRelationPropertiesUpdate", true}
					});

					// Report message
					reportBuilder.AppendLine(string.Format("Succes: Fixed incorrect node: name={0}, type={1}, id={2}", row.Get<string>(context, "name"), row.Get<string>(context, "type"), row.Get<string>(context, "id")));
				}
			}
		}
		#endregion
		#region Private Fields
		private readonly SqlServerRepository repository;
		#endregion
	}
}