using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Thrown when no service was found of a particular type.
	/// </summary>
	public class NoServiceFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="NoServiceFoundException"/>.
		/// </summary>
		/// <param name="contractType">The <see cref="System.Type"/> of service being requested.</param>
		public NoServiceFoundException(Type contractType) : base(string.Format("Could not find service of type '{0}'", contractType))
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