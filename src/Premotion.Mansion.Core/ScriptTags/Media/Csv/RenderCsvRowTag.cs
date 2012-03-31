using Premotion.Mansion.Core.IO.Csv;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.Csv
{
	/// <summary>
	/// Renders values as a CSV row to the output.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderCsvRow")]
	public class RenderCsvRowTag : RenderCsvRowBaseTag
	{
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="outputPipe">The <see cref="CsvOutputPipe"/> to which to write.</param>
		protected override void DoExecute(IMansionContext context, CsvOutputPipe outputPipe)
		{
			// get the attributes
			var values = GetRequiredAttribute<string>(context, "values");

			// write the row
			outputPipe.Write(values.Split(new[] {','}));
		}
	}
}