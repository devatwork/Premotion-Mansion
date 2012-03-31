using System;
using Premotion.Mansion.Core.IO.Csv;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.Csv
{
	/// <summary>
	/// Renders a CSV document to the outpipe.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderCSVDocument")]
	public class RenderCsvDocumentTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// check if a default format is used or a custom one is specified
			var formatName = GetAttribute<string>(context, "format");
			CsvOutputFormat format;
			if (!string.IsNullOrEmpty(formatName))
			{
				// get the format
				format = CsvOutputFormat.GetFormat(formatName);
			}
			else
			{
				// create a custom format
				format = new CsvOutputFormat
				         {
				         	ColumnDelimitor = GetAttribute<string>(context, "columnDelimitor"),
				         	ColumnProperties = GetAttribute<string>(context, "columnProperties").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries),
				         	ColumnHeaders = GetAttribute(context, "columnHeaders", GetAttribute<string>(context, "columnProperties")).Split(new[] {','}),
				         	DocumentEndDelimitor = GetAttribute<string>(context, "documentEndDelimitor"),
				         	DocumentStartDelimitor = GetAttribute<string>(context, "documentStartDelimitor"),
				         	IncludeColumnHeaders = GetAttribute<bool>(context, "includeColumnHeaders"),
				         	RowDelimitor = GetAttribute<string>(context, "rowDelimitor"),
				         	TextQualifier = GetAttribute<string>(context, "textQualifier")
				         };
			}

			// check if the format is sane
			if (format.ColumnProperties.Length != format.ColumnHeaders.Length)
				throw new InvalidOperationException("Unbalanced header with properties");

			// create the XML pipe and push it to the stack
			using (var pipe = new CsvOutputPipe(context.OutputPipe, format))
			using (context.OutputPipeStack.Push(pipe))
			{
				// execute the children
				ExecuteChildTags(context);
			}
		}
	}
}