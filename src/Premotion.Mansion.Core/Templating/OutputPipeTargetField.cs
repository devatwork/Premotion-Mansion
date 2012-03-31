using System;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// This field writes to the output to the active output pipe.
	/// </summary>
	public class OutputPipeTargetField : IField
	{
		#region Constructors
		/// <summary>
		/// Constructs the output pipe target field.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public OutputPipeTargetField(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// set values
			IOutputPipe output;
			if (!context.OutputPipeStack.TryPeek(out output))
				throw new InvalidOperationException("There is no open outputpipe to which to write.");
			outputPipe = output;
		}
		#endregion
		#region Append Methods
		/// <summary>
		/// Appends content to this field.
		/// </summary>
		/// <param name="content">The content which to append.</param>
		public void Append(string content)
		{
			// validate argument
			if (content == null)
				throw new ArgumentNullException("content");

			// write the content to the output pipe
			outputPipe.Writer.Write(content);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="IOutputPipe"/> to which this field is written.
		/// </summary>
		public IOutputPipe OutputPipe
		{
			get { return outputPipe; }
		}
		/// <summary>
		/// Gets the content of this field.
		/// </summary>
		public string Content
		{
			get { throw new NotSupportedException(); }
		}
		#endregion
		#region Private Fields
		private readonly IOutputPipe outputPipe;
		#endregion
	}
}