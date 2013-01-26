using System;
using System.Collections.Generic;

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
		private static readonly IDictionary<string, CsvFormat> Formats = new Dictionary<string, CsvFormat>(StringComparer.OrdinalIgnoreCase) {
			{"DutchExcel", DutchExcel},
			{"EnglishExcel", EnglishExcel},
			{"Tab", Tab}
		};
		#endregion
		#region Methods
		/// <summary>
		/// Gets a <see cref="CsvFormat"/> by it's <paramref name="formatName"/>.
		/// </summary>
		/// <param name="formatName">The name of the format which to get.</param>
		/// <returns>Returns the <see cref="CsvFormat"/> matching the name.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="formatName"/> is null  or empty.</exception>
		/// <exception cref="InvalidOperationException">Thrown if no format with <paramref name="formatName"/> was found.</exception>
		public static CsvFormat GetByName(string formatName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(formatName))
				throw new ArgumentNullException("formatName");

			CsvFormat format;
			if (!Formats.TryGetValue(formatName, out format))
				throw new InvalidOperationException(string.Format("Could not find CSV format for name '{0}'", formatName));
			return format;
		}
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