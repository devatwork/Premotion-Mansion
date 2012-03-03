using System;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Dependencies
{
	/// <summary>
	/// Thrown when a service dependency was not satisfied.
	/// </summary>
	public class MissingDependencyException : ServiceActivationException
	{
		#region Constructors
		/// <summary>
		/// Constructs the exception.
		/// </summary>
		/// <param name="implementationType">The service implementation requiring the dependency.</param>
		/// <param name="dependentServiceContract">The missing service contract dependency.</param>
		public MissingDependencyException(Type implementationType, Type dependentServiceContract)
		{
			// validate arguments
			if (implementationType == null)
				throw new ArgumentNullException("implementationType");
			if (dependentServiceContract == null)
				throw new ArgumentNullException("dependentServiceContract");

			// format the message
			message = string.Format("Failed to start implementation '{0}' due to missing dependency: '{1}'", implementationType, dependentServiceContract);
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