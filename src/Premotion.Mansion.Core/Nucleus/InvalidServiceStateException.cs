using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Thrown when the service is not ready for use yet.
	/// </summary>
	public class InvalidServiceStateException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="InvalidServiceStateException"/>.
		/// </summary>
		/// <param name="contractType">The <see cref="System.Type"/> of service being requested.</param>
		public InvalidServiceStateException(Type contractType) : base(string.Format("Service of type '{0}' is not ready to use", contractType))
		{
			// validate arguments
			if (contractType == null)
				throw new ArgumentNullException("contractType");

			// set values
			ContractType = contractType;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="System.Type"/> of service being requested but could not be resolved.
		/// </summary>
		public Type ContractType { get; private set; }
		#endregion
	}
}