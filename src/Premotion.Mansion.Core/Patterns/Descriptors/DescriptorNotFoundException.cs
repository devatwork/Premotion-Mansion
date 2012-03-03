using System;

namespace Premotion.Mansion.Core.Patterns.Descriptors
{
	/// <summary>
	/// This exception is thrown when a <see cref="IDescriptee"/> does not contain a descriptor of type <typeparam name="TDescriptor" />
	/// </summary>
	public class DescriptorNotFoundException<TDescriptor> : ApplicationException where TDescriptor : IDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="descriptee"></param>
		public DescriptorNotFoundException(IDescriptee descriptee)
		{
			// validate arguments
			if (descriptee == null)
				throw new ArgumentNullException("descriptee");

			// format the message
			message = string.Format("Could not find descriptor of type '{0}' on '{1}'", typeof (TDescriptor), descriptee.GetType());
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