using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Initializes the current repository from the application if there is one. This initializer makes sure the common nodes exist in the repository.
	/// </summary>
	public class RepositoryApplicationInitializer : ApplicationInitializer
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public RepositoryApplicationInitializer() : base(30)
		{
		}
		#endregion
		#region Overrides of ApplicationInitializer
		/// <summary>
		/// Initializes the application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoInitialize(IMansionContext context)
		{
			// open the repository
			using (RepositoryUtil.Open(context))
			{
				// check if the root node exists
				var rootNode = context.Repository.RetrieveSingleNode(context, new Query().Add(new IsPropertyEqualSpecification("id", 1)).Add(new StorageOnlyQueryComponent()));
				if (rootNode == null)
					throw new InvalidOperationException("The root node was not found in the repository, please make sure it exists before initializing");

				// if the repository is already intialized do not reinitialize it
				if (rootNode.Get(context, "repositoryInitialized", false))
					return;

				// create the default nodes
				var securityNode = context.Repository.CreateNode(context, rootNode, new PropertyBag
				                                                                    {
				                                                                    	{"type", "SecurityIndex"},
				                                                                    	{"name", "Security"},
				                                                                    	{"approved", true},
				                                                                    });
				var roleIndexNode = context.Repository.CreateNode(context, securityNode, new PropertyBag
				                                                                         {
				                                                                         	{"type", "RoleIndex"},
				                                                                         	{"name", "Roles"},
				                                                                         	{"approved", true},
				                                                                         });
				var adminRoleNode = context.Repository.CreateNode(context, roleIndexNode, new PropertyBag
				                                                                          {
				                                                                          	{"type", "Role"},
				                                                                          	{"name", "Administrator"},
				                                                                          	{"approved", true},
				                                                                          });
				var visitorRoleNode = context.Repository.CreateNode(context, roleIndexNode, new PropertyBag
				                                                                            {
				                                                                            	{"type", "Role"},
				                                                                            	{"name", "Visitor"},
				                                                                            	{"approved", true},
				                                                                            });
				var userIndexNode = context.Repository.CreateNode(context, securityNode, new PropertyBag
				                                                                         {
				                                                                         	{"type", "UserIndex"},
				                                                                         	{"name", "Users"},
				                                                                         	{"approved", true},
				                                                                         });
				var adminUserNode = context.Repository.CreateNode(context, userIndexNode, new PropertyBag
				                                                                          {
				                                                                          	{"type", "User"},
				                                                                          	{"name", "Administrator"},
				                                                                          	{"login", "admin@premotion.nl"},
				                                                                          	{"password", "admin"},
				                                                                          	{"approved", true},
				                                                                          });
				context.Repository.CreateNode(context, userIndexNode, new PropertyBag
				                                                      {
				                                                      	{"type", "User"},
				                                                      	{"name", "Visitor"},
				                                                      	{"key", "AnonymousUser"},
				                                                      	{"allowedRoleGuids", adminRoleNode.PermanentId},
				                                                      	{"approved", true},
				                                                      });
				var userGroupIndexNode = context.Repository.CreateNode(context, securityNode, new PropertyBag
				                                                                              {
				                                                                              	{"type", "UserGroupIndex"},
				                                                                              	{"name", "Groups"},
				                                                                              	{"allowedRoleGuids", adminRoleNode.PermanentId},
				                                                                              	{"approved", true},
				                                                                              });
				var adminUserGroup = context.Repository.CreateNode(context, userGroupIndexNode, new PropertyBag
				                                                                                {
				                                                                                	{"type", "UserGroup"},
				                                                                                	{"name", "Administrators"},
				                                                                                	{"allowedRoleGuids", adminRoleNode.PermanentId},
				                                                                                	{"assignedRoleGuids", adminRoleNode.PermanentId},
				                                                                                	{"userGuids", adminUserNode.PermanentId},
				                                                                                	{"approved", true},
				                                                                                });

				// update the root node
				context.Repository.UpdateNode(context, rootNode, new PropertyBag
				                                                 {
				                                                 	{"repositoryInitialized", true},
				                                                 	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString() + "," + visitorRoleNode.PermanentId.ToString()},
				                                                 });
				context.Repository.UpdateNode(context, securityNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId}});
				context.Repository.UpdateNode(context, roleIndexNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId}});
				context.Repository.UpdateNode(context, adminRoleNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId}});
				context.Repository.UpdateNode(context, visitorRoleNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId}});
				context.Repository.UpdateNode(context, userIndexNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId}});
				context.Repository.UpdateNode(context, adminUserNode, new PropertyBag
				                                                      {
				                                                      	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}
				                                                      });

				// add cms permission to the admin user
				context.Nucleus.ResolveSingle<ISecurityModelService>().AssignPermission(context, new Role(adminRoleNode.PermanentId), ProtectedOperation.Create(context, "Cms", "use"));
			}
		}
		#endregion
	}
}