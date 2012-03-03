using System;
using System.IO;
using System.Text;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.IO.Csv
{
	/// <summary>
	/// Implements the <see cref="IOutputPipe"/> for writing CSV files.
	/// </summary>
	public class CsvOutputPipe : DisposableBase, IOutputPipe
	{
		#region Constructors
		/// <summary>
		/// Constructs the XML output pipe.
		/// </summary>
		/// <param name="outputPipe">The target output pipe which to write to.</param>
		/// <param name="format">The <see cref="CsvOutputFormat"/> which to use.</param>
		public CsvOutputPipe(IOutputPipe outputPipe, CsvOutputFormat format)
		{
			// validate arguments
			if (outputPipe == null)
				throw new ArgumentNullException("outputPipe");
			if (format == null)
				throw new ArgumentNullException("format");

			// set values
			this.outputPipe = outputPipe;
			this.format = format;

			// write the document start delimitor when available
			if (format.HasDocumentStartDelimitor)
				outputPipe.Writer.Write(format.DocumentStartDelimitor);

			// write the column headers when needed
			if (format.IncludeColumnHeaders)
				Write(format.ColumnHeaders);
		}
		#endregion
		#region Write Methods
		/// <summary>
		/// Writes <paramref name="values"/> to the output.
		/// </summary>
		/// <param name="values">The values of the row which to write.</param>
		public void Write(string[] values)
		{
			// validate arguments
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Length != format.ColumnHeaders.Length)
				throw new ArgumentException(string.Format("Value array ({0}) is not the same size as the column headers array ({1})", values.Length, format.ColumnHeaders.Length), "values");
			CheckDisposed();

			// write out the values
			for (var index = 0; index < values.Length; index++)
			{
				// write out the current value when there is one
				var value = values[index];
				if (!string.IsNullOrEmpty(value))
				{
					outputPipe.Writer.Write(format.TextQualifier);
					outputPipe.Writer.Write(value);
					outputPipe.Writer.Write(format.TextQualifier);
				}

				// write out the column or row delimitor
				outputPipe.Writer.Write(index == (values.Length - 1) ? format.RowDelimitor : format.ColumnDelimitor);
			}
		}
		/// <summary>
		/// Writes <paramref name="values"/> to the output.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="values">The <see cref="IPropertyBag"/> containing the values which to write.</param>
		public void Write(IContext context, IPropertyBag values)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (values == null)
				throw new ArgumentNullException("values");
			CheckDisposed();

			// write out the values
			for (var index = 0; index < format.ColumnProperties.Length; index++)
			{
				// write out the current value when there is one
				string value;
				if (values.TryGet(context, format.ColumnProperties[index], out value))
				{
					outputPipe.Writer.Write(format.TextQualifier);
					outputPipe.Writer.Write(value);
					outputPipe.Writer.Write(format.TextQualifier);
				}

				// write out the column or row delimitor
				outputPipe.Writer.Write(index == (format.ColumnProperties.Length - 1) ? format.RowDelimitor : format.ColumnDelimitor);
			}
		}
		#endregion
		#region Implementation of IPipe
		/// <summary>
		/// Gets the encoding of this pipe.
		/// </summary>
		public Encoding Encoding
		{
			get
			{
				CheckDisposed();
				return outputPipe.Encoding;
			}
		}
		#endregion
		#region Implementation of IOutputPipe
		/// <summary>
		/// Gets the writer for this pipe.
		/// </summary>
		public TextWriter Writer
		{
			get { throw new NotSupportedException("Only the dedicated CSV write methods are supported"); }
		}
		/// <summary>
		/// Gets the underlying stream of this pipe. Use with caution.
		/// </summary>
		public Stream RawStream
		{
			get { throw new NotSupportedException("Only the dedicated CSV write methods are supported"); }
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
			// check for unmanaged disposal
			if (!disposeManagedResources)
				return;

			// write the document start delimitor when available
			if (format.HasDocumentEndDelimitor)
				outputPipe.Writer.Write(format.DocumentStartDelimitor);
		}
		#endregion
		#region Private Fields
		private readonly CsvOutputFormat format;
		private readonly IOutputPipe outputPipe;
		#endregion
	}
}