using System;

namespace Premotion.Mansion.Core.IO.Csv
{
	/// <summary>
	/// Represents the format of an CSV document being outputted.
	/// </summary>
	public class CsvOutputFormat : CsvFormat
	{
		#region Properties
		/// <summary>
		/// Flag indicating whether there is a document start delemitor.
		/// </summary>
		public bool HasDocumentStartDelimitor
		{
			get { return !string.IsNullOrEmpty(DocumentStartDelimitor); }
		}
		/// <summary>
		/// Gets the document start delemitor.
		/// </summary>
		public string DocumentStartDelimitor { get; set; }
		/// <summary>
		/// Flag indicating whether there is a document end delemitor.
		/// </summary>
		public bool HasDocumentEndDelimitor
		{
			get { return !string.IsNullOrEmpty(DocumentEndDelimitor); }
		}
		/// <summary>
		/// Gets the document end delemitor.
		/// </summary>
		public string DocumentEndDelimitor { get; set; }
		/// <summary>
		/// Flag indicating whether to include the headers in the document being written. Default is true.
		/// </summary>
		public bool IncludeColumnHeaders { get; set; }
		/// <summary>
		/// Gets the headers of the column.
		/// </summary>
		public string[] ColumnHeaders { get; set; }
		/// <summary>
		/// Gets the properties displayed in the columns.
		/// </summary>
		public string[] ColumnProperties { get; set; }
		#endregion
		#region Enumeration Methods
		/// <summary>
		/// Gets a <see cref="CsvOutputFormat"/> identified by it's <paramref name="formatName"/>.
		/// </summary>
		/// <param name="formatName">The name of the format which to get.</param>
		/// <returns>Returns the <see cref="CsvOutputFormat"/>.</returns>
		/// <exception cref="InvalidOperationException">Thrown when no format could be found with the <paramref name="formatName"/>.</exception>
		public static CsvOutputFormat GetFormat(string formatName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(formatName))
				throw new ArgumentNullException("formatName");

			// no format found matching the name
			throw new InvalidOperationException(string.Format("Could not find a CSV output format with name '{0}'", formatName));
		}
		#endregion
	}
}