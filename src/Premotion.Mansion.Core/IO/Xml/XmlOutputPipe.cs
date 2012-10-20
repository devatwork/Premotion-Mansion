using System;
using System.IO;
using System.Text;
using System.Xml;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.IO.Xml
{
	/// <summary>
	/// Represents the XML output pipe.
	/// </summary>
	public class XmlOutputPipe : DisposableBase, IOutputPipe
	{
		#region Constructors
		/// <summary>
		/// Constructs the XML output pipe.
		/// </summary>
		/// <param name="outputPipe">The target output pipe which to write to.</param>
		public XmlOutputPipe(IOutputPipe outputPipe)
		{
			// validate arguments
			if (outputPipe == null)
				throw new ArgumentNullException("outputPipe");

			// set values
			this.outputPipe = outputPipe;

			// create the reader
			xmlWriter = XmlWriter.Create(outputPipe.Writer, new XmlWriterSettings
			                                                {
			                                                	CloseOutput = false,
																				NamespaceHandling = NamespaceHandling.OmitDuplicates
			                                                });

			// create the namespace manager
			NamespaceManager = new XmlNamespaceManager(new NameTable());
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
			xmlWriter.Flush();
			xmlWriter.Close();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the XML writer.
		/// </summary>
		public XmlWriter XmlWriter
		{
			get
			{
				CheckDisposed();
				return xmlWriter;
			}
		}
		/// <summary>
		/// Gets or sets the default namespace.
		/// </summary>
		public XmlNamespaceManager NamespaceManager { get; set; }
		#endregion
		#region Private Fields
		private readonly IOutputPipe outputPipe;
		private readonly XmlWriter xmlWriter;
		#endregion
	}
}