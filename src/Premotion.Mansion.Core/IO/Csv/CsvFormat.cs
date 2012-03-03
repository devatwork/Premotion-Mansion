namespace Premotion.Mansion.Core.IO.Csv
{
	/// <summary>
	/// Represents the format of an CSV document.
	/// </summary>
	public class CsvFormat
	{
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