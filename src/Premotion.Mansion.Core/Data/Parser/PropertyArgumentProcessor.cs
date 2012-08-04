using System;
using System.Linq;
using Premotion.Mansion.Core.Data.Specifications;

namespace Premotion.Mansion.Core.Data.Parser
{
	/// <summary>
	/// Parses the remaining parameters into property specifications.
	/// </summary>
	public class PropertyArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public PropertyArgumentProcessor() : base(10)
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
			// loop through all the remaining properties
			foreach (var propertyName in parameters.Names.ToArray())
			{
				// guard against empty properties
				string valueString;
				if (!parameters.TryGetAndRemove(context, propertyName, out valueString) || string.IsNullOrWhiteSpace(valueString))
					continue;

				// parse the parameters, ignoring properties without values
				object[] values = (valueString).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
				if (values.Length == 0)
					continue;

				// turn it into a specification
				query.Add(SpecificationFactory.IsIn(propertyName, values));
			}
		}
		#endregion
	}
}