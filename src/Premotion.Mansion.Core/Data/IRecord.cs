using System;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Represents a record.
	/// </summary>
	public interface IRecord : IPropertyBag
	{
		#region Methods
		/// <summary>
		/// Initializes this record.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		void Initialize(IMansionContext context);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of this record.
		/// </summary>
		int Id { get; }
		/// <summary>
		/// Gets the type name of this record.
		/// </summary>
		string Type { get; }
		#endregion
	}
}