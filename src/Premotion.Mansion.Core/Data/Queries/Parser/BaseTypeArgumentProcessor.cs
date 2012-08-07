using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Parses the base type parameter.
	/// </summary>
	public class BaseTypeArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public BaseTypeArgumentProcessor(ITypeService typeService) : base(100)
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
			string baseTypeNames;
			if (!parameters.TryGetAndRemove(context, "baseType", out baseTypeNames) && string.IsNullOrEmpty(baseTypeNames))
				return;

			// parse the type names
			var types = baseTypeNames.Split(',').Select(x => typeService.Load(context, x)).ToArray();
			if (types.Length == 0)
				return;

			// add the type hints to the query
			query.Add(types.SelectMany(baseType =>
			                           {
			                           	var list = new List<ITypeDefinition>(new[] {baseType});
			                           	list.AddRange(baseType.GetInheritingTypes(context));
			                           	return list;
			                           }));
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}