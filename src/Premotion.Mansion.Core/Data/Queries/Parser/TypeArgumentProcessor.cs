using System;
using System.Linq;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Parses the type parameter.
	/// </summary>
	public class TypeArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public TypeArgumentProcessor(ITypeService typeService) : base(100)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
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
			// get the type name is any
			string typeNames;
			if (!parameters.TryGetAndRemove(context, "type", out typeNames) && string.IsNullOrEmpty(typeNames))
				return;

			// parse the type names
			var types = typeNames.Split(',').Select(x => typeService.Load(context, x)).ToArray();
			if (types.Length == 0)
				return;

			// add the type hints to the query
			query.Add(types);
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}