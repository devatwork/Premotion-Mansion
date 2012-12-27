using System;
using Premotion.Mansion.Core;
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
			// open the repository
			using (RepositoryUtil.Open(context))
			{
				// check if the root node exists
				var rootNode = context.Repository.RetrieveRootNode(context);
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