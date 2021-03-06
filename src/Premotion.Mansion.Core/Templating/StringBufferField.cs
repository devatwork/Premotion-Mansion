﻿using System.Collections.Generic;
using System.Text;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Represets a buffered field.
	/// </summary>
	public class StringBufferField : IField
	{
		#region Append Methods
		/// <summary>
		/// Appends content to this field.
		/// </summary>
		/// <param name="content">The content which to append.</param>
		public void Append(string content)
		{
			// validate argument
			if (content == null)
				return;

			// append to buffer
			buffer.Append(content);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the content of this field.
		/// </summary>
		public string Content
		{
			get { return buffer.ToString(); }
		}
		/// <summary>
		/// Gets a collection of rendered section ids.
		/// </summary>
		public ICollection<string> RenderedSections
		{
			get { return renderedSections; }
		}
		#endregion
		#region Private Fields
		private readonly StringBuilder buffer = new StringBuilder();
		private readonly List<string> renderedSections = new List<string>();
		#endregion
	}
}