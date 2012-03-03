using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.IO.JSon
{
	/// <summary>
	/// Represents the JSON output pipe.
	/// </summary>
	public class JsonOutputPipe : DisposableBase, IOutputPipe
	{
		#region Constructors
		/// <summary>
		/// Constructs the XML output pipe.
		/// </summary>
		/// <param name="outputPipe">The target output pipe which to write to.</param>
		public JsonOutputPipe(IOutputPipe outputPipe)
		{
			// validate arguments
			if (outputPipe == null)
				throw new ArgumentNullException("outputPipe");

			// set values
			this.outputPipe = outputPipe;

			// create the reader
			jsonWriter = new JsonTextWriter(outputPipe.Writer)
			             {
			             	CloseOutput = false,
			             	Formatting = Formatting.None
			             };
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
			get
			{
				CheckDisposed();
				return outputPipe.Writer;
			}
		}
		/// <summary>
		/// Gets the underlying stream of this pipe. Use with caution.
		/// </summary>
		public Stream RawStream
		{
			get { return outputPipe.RawStream; }
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

			// close the reader
			jsonWriter.Flush();
			jsonWriter.Close();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the XML writer.
		/// </summary>
		public JsonTextWriter JsonWriter
		{
			get
			{
				CheckDisposed();
				return jsonWriter;
			}
		}
		#endregion
		#region Private Fields
		private readonly JsonTextWriter jsonWriter;
		private readonly IOutputPipe outputPipe;
		#endregion
	}
}