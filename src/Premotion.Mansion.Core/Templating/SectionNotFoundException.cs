using System;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Thrown when a section could not be found.
	/// </summary>
	public class SectionNotFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="context"></param>
		public SectionNotFoundException(string name, MansionContext context)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (context == null)
				throw new ArgumentNullException("context");

			// format the message
			var buffer = new StringBuilder();
			buffer.AppendLine(string.Format("Could not find section with name '{0}' in templates:", name));
			foreach (var path in context.TemplateStack.Select(template => template.Path.Paths.Last()))
				buffer.AppendLine("- " + path);
			message = buffer.ToString();
		}
		#endregion
		#region Overrides of Exception
		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <returns>
		/// The error message that explains the reason for the exception, or an empty string("").
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override string Message
		{
			get { return message; }
		}
		#endregion
		#region Private Fields
		private readonly string message;
		#endregion
	}
}