using Premotion.Mansion.Core.IO.Csv;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.Csv
{
	/// <summary>
	/// Renders a dataspace as a CSV row to the output.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderDataspaceToCsvRow")]
	public class RenderDataspaceToCsvRowTag : RenderCsvRowBaseTag
	{
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="outputPipe">The <see cref="CsvOutputPipe"/> to which to write.</param>
		protected override void DoExecute(IMansionContext context, CsvOutputPipe outputPipe)
		{
			// get the attributes
			var dataspace = GetRequiredAttribute<IPropertyBag>(context, "source");

			// write the row
			outputPipe.Write(context, dataspace);
		}
	}
}