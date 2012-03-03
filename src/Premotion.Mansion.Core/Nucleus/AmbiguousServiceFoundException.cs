using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Thrown when an ambiguous service was found.
	/// </summary>
	public class AmbiguousServiceFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="AmbiguousServiceFoundException"/>.
		/// </summary>
		/// <param name="contractType">The <see cref="System.Type"/> of service being requested.</param>
		/// <param name="implementationTypes">The types.</param>
		public AmbiguousServiceFoundException(Type contractType, IEnumerable<Type> implementationTypes) : base(string.Format("Service of type '{0}' resolved to multiple instances", contractType))
		{
			// validate arguments
			if (contractType == null)
				throw new ArgumentNullException("contractType");
			if (implementationTypes == null)
				throw new ArgumentNullException("implementationTypes");

			// set values
			ContractType = contractType;
			ImplementationTypes = implementationTypes.ToArray();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="System.Type"/> of service being requested but could not be resolved.
		/// </summary>
		public Type ContractType { get; private set; }
		/// <summary>
		/// Gets the types.
		/// </summary>
		public Type[] ImplementationTypes { get; private set; }
		#endregion
	}
}