using System;
using Premotion.Mansion.Core.IO.Csv;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.Csv
{
	/// <summary>
	/// Base class for all render CSV rows <see cref="ScriptTag"/>s.
	/// </summary>
	public abstract class RenderCsvRowBaseTag : ScriptTag
	{
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			// get the XML output pipe
			var outputPipe = context.OutputPipe as CsvOutputPipe;
			if (outputPipe == null)
				throw new InvalidOperationException("No CSV output pipe found on thet stack. Open a CSV output pipe first.");
			DoExecute(context, outputPipe);
		}
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="outputPipe">The <see cref="CsvOutputPipe"/> to which to write.</param>
		protected abstract void DoExecute(MansionContext context, CsvOutputPipe outputPipe);
	}
}