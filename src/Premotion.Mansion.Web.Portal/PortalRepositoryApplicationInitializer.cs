using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Web.Portal
{
	/// <summary>
	/// Initializes the current repository from the application if there is one. This initializer makes sure the common nodes exist in the repository.
	/// </summary>
	public class PortalRepositoryApplicationInitializer : ApplicationInitializer
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public PortalRepositoryApplicationInitializer() : base(35)
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
				var rootNode = context.Repository.RetrieveSingle(context, new PropertyBag
				                                                          {
				                                                          	{"id", 1},
				                                                          	{"bypassAuthorization", true}
				                                                          });
				if (rootNode == null)
					throw new InvalidOperationException("The root node was not found in the repository, please make sure it exists before initializing");

				// make sure the content index root node exists
				context.Repository.RetrieveContentIndexRootNode(context);

				// make sure the taxonomy node exists
				context.Repository.RetrieveTaxonomyNode(context);
			}
		}
		#endregion
	}
}