using System;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Marks any type as exported indicating.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class ExportedAttribute : Attribute
	{
		#region Constructors
		/// <summary>
		/// Constructs an exported attribute for the given <paramref name="contractType"/>.
		/// </summary>
		/// <param name="contractType">The <see cref="Type"/> of the contract.</param>
		public ExportedAttribute(Type contractType)
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
		/// Gets the contract type of this export type.
		/// </summary>
		public Type ContractType { get; private set; }
		#endregion
	}
}