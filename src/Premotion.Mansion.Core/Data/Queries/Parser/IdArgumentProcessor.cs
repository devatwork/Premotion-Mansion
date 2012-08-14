using System;
using Premotion.Mansion.Core.Data.Queries.Specifications;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Implements the <see cref="NodePointer"/> <see cref="QueryArgumentProcessor"/>.
	/// </summary>
	public class IdArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public IdArgumentProcessor() : base(200)
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
			var counter = 0;

			// check for id, guid or pointer
			int id;
			if (parameters.TryGetAndRemove(context, "id", out id))
			{
				query.Add(new IsPropertyEqualSpecification("id", id));
				counter++;
			}
			Guid guid;
			if (parameters.TryGetAndRemove(context, "guid", out guid))
			{
				query.Add(new IsPropertyEqualSpecification("guid", guid));
				counter++;
			}
			NodePointer pointer;
			if (parameters.TryGetAndRemove(context, "pointer", out pointer))
			{
				query.Add(new IsPropertyEqualSpecification("id", pointer.Id));
				counter++;
			}

			// check for ambigous parameters
			if (counter > 1)
				throw new InvalidOperationException("Detected an ambigious id parmeters. Remove either id, guid or pointer.");
		}
		#endregion
	}
}