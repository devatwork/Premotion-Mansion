using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.IO.Csv
{
	/// <summary>
	/// Implements the <see cref="IPropertyBagReader"/> for reading CSVs.
	/// </summary>
	public class CsvReader : DisposableBase, IPropertyBagReader
	{
		#region Constructors
		/// <summary>
		/// Constructs a CSV row reader from the given <paramref name="input"/>.
		/// </summary>
		/// <param name="input">The <see cref="IInputPipe"/> from which to read.</param>
		/// <param name="format">The <see cref="CsvFormat"/>.</param>
		/// <param name="firstLineIsHeader">Flag indicating whether the first line read contains the header. Defaults to true.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the arguments is null.</exception>
		public CsvReader(IInputPipe input, CsvFormat format, bool firstLineIsHeader = true)
		{
			// validate arguments
			if (input == null)
				throw new ArgumentNullException("input");
			if (format == null)
				throw new ArgumentNullException("format");

			// set values
			this.input = input;
			this.format = format;
			this.firstLineIsHeader = firstLineIsHeader;
		}
		#endregion
		#region IPropertyBagReader Members
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<IPropertyBag> GetEnumerator()
		{
			// make sure this object is not disposed yet
			CheckDisposed();

			// create a buffer
			var buffer = new StringBuilder();

			// read until end of line or end of file
			var inTextQualifier = false;
			var firstLineRead = false;
			var row = new PropertyBag();
			var cellIndex = 0;
			var headerList = new List<string>();
			while (true)
			{
				// read the current character
				var currentCharacter = input.Reader.Read();

				// check for end of line
				if (currentCharacter == -1)
					yield break;

				// append the character
				buffer.Append((char) currentCharacter);

				// check for text qualifier
				if (buffer.EndsWith(format.TextQualifier))
				{
					// eat the text qualifier
					buffer.Length -= format.TextQualifier.Length;

					// toggle the flag
					inTextQualifier = !inTextQualifier;
				}
				else if (inTextQualifier)
					continue;

				// check for column delimitor
				if (buffer.EndsWith(format.ColumnDelimitor))
				{
					// eat the column delimitor
					buffer.Length -= format.ColumnDelimitor.Length;

					// get the read value
					var value = buffer.ToString();

					// if the first line is read, store the value in the row property bag
					if (firstLineRead)
						row.Set(headerList[cellIndex], value);
					else
					{
						// if the first line contains the header, the value contains a column name, otherwise store the value in the row property bag
						if (firstLineIsHeader)
							headerList.Add(value);
						else
						{
							// add a header to the list and store the value
							headerList.Add(cellIndex.ToString(CultureInfo.InvariantCulture));
							row.Set(headerList[cellIndex], value);
						}
					}

					// reset the buffer
					buffer.Length = 0;

					// increment the cell index
					cellIndex++;

					// keep reading
					continue;
				}

				// check for row delimitor
				if (buffer.EndsWith(format.RowDelimitor))
				{
					// eat the row delimitor
					buffer.Length -= format.RowDelimitor.Length;

					// get the read value
					var value = buffer.ToString();
					buffer.Length = 0;
					cellIndex = 0;

					// set the value
					if (!firstLineRead)
					{
						firstLineRead = true;
						// if the first line contains the header, store the header value
						if (firstLineIsHeader)
						{
							headerList.Add(value);
							continue;
						}

						// add header based on cell index
						headerList.Add(cellIndex.ToString(CultureInfo.InvariantCulture));
					}

					// store the value
					row.Set(headerList[cellIndex], value);

					// return the result
					yield return row;

					// reset
					row = new PropertyBag();
				}
			}
		}
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			if (!disposeManagedResources)
				return;

			// dispose the input
			input.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly bool firstLineIsHeader;
		private readonly CsvFormat format;
		private readonly IInputPipe input;
		#endregion
	}
}