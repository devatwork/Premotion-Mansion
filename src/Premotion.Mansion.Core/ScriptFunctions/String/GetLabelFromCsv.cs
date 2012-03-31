using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Returns the first argument which is not empty.
	/// </summary>
	[ScriptFunction("GetLabelFromCsv")]
	public class GetLabelFromCsv : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="csv"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string csv, string value)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (csv == null)
				throw new ArgumentNullException("csv");
			if (value == null)
				throw new ArgumentNullException("value");

			// split the csv
			var entries = csv.Split(';');

			// find the proper entry
			var entry = entries.Where(x => x.StartsWith(value + ",", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
			if (string.IsNullOrEmpty(entry))
				return string.Empty;

			// split the entry
			var entryParts = entry.Split(',');
			if (entryParts.Length != 2)
				throw new InvalidOperationException(string.Format("CSV value '{0}' should have two parts separted by a comma", entry));

			// return the value
			return entryParts[1];
		}
	}
}