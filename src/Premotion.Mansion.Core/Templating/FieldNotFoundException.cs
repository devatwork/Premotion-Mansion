using System;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Thrown when a field is not found in a particular section.
	/// </summary>
	public class FieldNotFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="context"></param>
		public FieldNotFoundException(string name, IMansionContext context)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (context == null)
				throw new ArgumentNullException("context");

			// format the message
			var buffer = new StringBuilder();
			buffer.AppendLine(string.Format("Could not find field with name '{0}' in sections:", name));
			foreach (var section in context.ActiveSectionStack.Select(activeSection => activeSection.Section))
				buffer.AppendLine(string.Format("- {0} ({1})", section.GetName(context), section.Template.Path.Paths.First()));
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