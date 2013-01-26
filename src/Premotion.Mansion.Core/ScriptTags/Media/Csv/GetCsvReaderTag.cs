using Premotion.Mansion.Core.IO.Csv;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.Csv
{
	/// <summary>
	/// Gets a CSV reader to read CSV data from the underlying pipe.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "getCsvReader")]
	public class GetCsvReaderTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the input pipe
			var inputPipe = context.InputPipe;

			// parse the format
			var formatName = GetRequiredAttribute<string>(context, "format");
			var format = CsvFormat.GetByName(formatName);

			// create the reader
			var reader = new CsvReader(inputPipe, format);

			// push the reader to the stack
			using (context.ReaderStack.Push(reader))
				ExecuteChildTags(context);
		}
		#endregion
	}
}