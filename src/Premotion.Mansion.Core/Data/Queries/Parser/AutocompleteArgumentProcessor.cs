using System;
using System.Linq;
using Premotion.Mansion.Core.Data.Queries.Specifications;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Implements the autocomplete argument processor.
	/// </summary>
	public class AutocompleteArgumentProcessor : QueryArgumentProcessor
	{
		private const string Postfix = "_autocomplete";
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public AutocompleteArgumentProcessor() : base(100)
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
			// find autocomplete properties
			foreach (var propertyName in parameters.Names.Where(x => x.EndsWith(Postfix, StringComparison.OrdinalIgnoreCase)).ToArray())
			{
				// remove the parameter
				string value;
				if (!parameters.TryGetAndRemove(context, propertyName, out value))
					continue;

				// cut of the autocomplete postfix
				var propertyNameWithoutPostix = propertyName.Substring(0, propertyName.Length - Postfix.Length);

				// create the specification
				query.Add(new AutocompleteSpecification(propertyNameWithoutPostix, value));
			}
		}
		#endregion
	}
}