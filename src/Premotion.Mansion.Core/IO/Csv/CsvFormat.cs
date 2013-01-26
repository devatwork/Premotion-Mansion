namespace Premotion.Mansion.Core.IO.Csv
{
	/// <summary>
	/// Represents the format of an CSV document.
	/// </summary>
	public class CsvFormat
	{
		#region Constants
		/// <summary>
		/// Defines the CSV format used by the Dutch version of Excel.
		/// </summary>
		public static readonly CsvFormat DutchExcel = new CsvFormat {
			ColumnDelimitor = ";",
			RowDelimitor = "\r\n",
			TextQualifier = "\""
		};
		/// <summary>
		/// Defines the CSV format used by the English version of Excel.
		/// </summary>
		public static readonly CsvFormat EnglishExcel = new CsvFormat {
			ColumnDelimitor = ",",
			RowDelimitor = "\r\n",
			TextQualifier = "\""
		};
		/// <summary>
		/// Defines the CSV format of tab seperated documents.
		/// </summary>
		public static readonly CsvFormat Tab = new CsvFormat {
			ColumnDelimitor = "\t",
			RowDelimitor = "\r\n",
			TextQualifier = "\""
		};
		#endregion
		#region Properties
		/// <summary>
		/// Gets the column delimitor.
		/// </summary>
		public string ColumnDelimitor { get; set; }
		/// <summary>
		/// Gets the row delimitor.
		/// </summary>
		public string RowDelimitor { get; set; }
		/// <summary>
		/// Gets the text qualifier.
		/// </summary>
		public string TextQualifier { get; set; }
		#endregion
	}
}