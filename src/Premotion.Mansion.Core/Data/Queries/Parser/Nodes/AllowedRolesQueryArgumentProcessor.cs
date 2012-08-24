using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;

namespace Premotion.Mansion.Core.Data.Queries.Parser.Nodes
{
	/// <summary>
	/// Implements the allow roles query argument processor.
	/// </summary>
	public class AllowedRolesQueryArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public AllowedRolesQueryArgumentProcessor() : base(300)
		{
		}
		#endregion
		#region Overrides of QueryArgumentProcessor
		/// <summary>
		/// Processes the <paramref name="parameters"/> and turn them into <paramref name="query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parameters">The parameters which to process.</param>
		/// <param name="query">The <see cref="Query"/> in which to set the parameters.</param>
		protected override void DoProcess(IMansionContext context, IPropertyBag parameters, Query query)
		{
			// if the authorization is bypassed do not add the security check
			// TODO: add validator logic to make sure the type supports authorization
			bool bypassAuthorization;
			if (parameters.TryGetAndRemove(context, "bypassAuthorization", out bypassAuthorization) && bypassAuthorization)
				query.Add(AllowedRolesSpecification.Any());
			else
				query.Add(AllowedRolesSpecification.UserRoles(context));
		}
		#endregion
	}
}