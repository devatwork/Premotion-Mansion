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
			var applicationSettings = context.Stack.Peek<IPropertyBag>(ApplicationSettingsConstants.DataspaceName);
			var repositoryNamespace = applicationSettings.Get(context, ApplicationSettingsConstants.RepositoryNamespace, String.Empty);

			// if not repository is specified to not try to intialize
			if (String.IsNullOrEmpty(repositoryNamespace))
				return;

			// open the repository
			using (RepositoryUtil.Open(context, repositoryNamespace, applicationSettings))
			{
				// check if the root node exists
				var rootNode = context.Repository.RetrieveSingleNode(context, new Query().Add(new IsPropertyEqualSpecification("id", 1)));
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
				                                                                });
				var roleIndexNode = context.Repository.CreateNode(context, securityNode, new PropertyBag
				                                                                     {
				                                                                     	{"type", "RoleIndex"},
				                                                                     	{"name", "Roles"},
				                                                                     });
				var adminRoleNode = context.Repository.CreateNode(context, roleIndexNode, new PropertyBag
				                                                                      {
				                                                                      	{"type", "Role"},
				                                                                      	{"name", "Administrator"},
				                                                                      });
				var visitorRoleNode = context.Repository.CreateNode(context, roleIndexNode, new PropertyBag
				                                                                        {
				                                                                        	{"type", "Role"},
				                                                                        	{"name", "Visitor"},
				                                                                        });
				var userIndexNode = context.Repository.CreateNode(context, securityNode, new PropertyBag
				                                                                     {
				                                                                     	{"type", "UserIndex"},
				                                                                     	{"name", "Users"},
				                                                                     });
				var adminUserNode = context.Repository.CreateNode(context, userIndexNode, new PropertyBag
				                                                                      {
				                                                                      	{"type", "User"},
				                                                                      	{"name", "Administrator"},
				                                                                      	{"login", "admin@premotion.nl"},
				                                                                      	{"password", "admin"},
				                                                                      });
				context.Repository.CreateNode(context, userIndexNode, new PropertyBag
				                                                  {
				                                                  	{"type", "User"},
				                                                  	{"name", "Visitor"},
				                                                  	{"foreignId", "Anonymous"},
				                                                  	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                  });
				var userGroupIndexNode = context.Repository.CreateNode(context, securityNode, new PropertyBag
				                                                                          {
				                                                                          	{"type", "UserGroupIndex"},
				                                                                          	{"name", "Groups"},
				                                                                          	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                                          });
				var adminUserGroup = context.Repository.CreateNode(context, userGroupIndexNode, new PropertyBag
				                                                                            {
				                                                                            	{"type", "UserGroup"},
				                                                                            	{"name", "Administrators"},
				                                                                            	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                                            	{"assignedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                                            	{"userGuids", adminUserNode.PermanentId.ToString()},
				                                                                            });

				// update the root node
				context.Repository.UpdateNode(context, rootNode, new PropertyBag
				                                             {
				                                             	{"repositoryInitialized", true},
				                                             	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString() + "," + visitorRoleNode.PermanentId.ToString()},
				                                             });
				context.Repository.UpdateNode(context, securityNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.UpdateNode(context, roleIndexNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.UpdateNode(context, adminRoleNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.UpdateNode(context, visitorRoleNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.UpdateNode(context, userIndexNode, new PropertyBag {{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()}});
				context.Repository.UpdateNode(context, adminUserNode, new PropertyBag
				                                                  {
				                                                  	{"allowedRoleGuids", adminRoleNode.PermanentId.ToString()},
				                                                  	{"foreignId", adminUserNode.PermanentId.ToString()}
				                                                  });
				context.Repository.UpdateNode(context, adminUserGroup, new PropertyBag
				                                                   {
				                                                   	{"foreignId", adminUserGroup.PermanentId.ToString()}
				                                                   });

				// add cms permission to the admin user
				context.Nucleus.ResolveSingle<ISecurityModelService>().AssignPermission(context, new Role(adminRoleNode.PermanentId.ToString()), ProtectedOperation.Create(context, "Cms", "use"));
			}
		}
		#endregion
	}
}