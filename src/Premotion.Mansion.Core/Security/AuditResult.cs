using System.Collections.Generic;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Represents the result of an security audit.
	/// </summary>
	public class AuditResult
	{
		#region Properties
		/// <summary>
		/// Gets the flag indicating whether the <see cref="User"/> has permission to the <see cref="Operation"/>.
		/// </summary>
		public bool Granted { get; set; }
		/// <summary>
		/// Gets the <see cref="Permission"/>s checked.
		/// </summary>
		public List<Permission> Permissions { get; set; }
		/// <summary>
		/// Gets the <see cref="ProtectedOperation"/> against which permission was checked.
		/// </summary>
		public ProtectedOperation Operation { get; set; }
		/// <summary>
		/// Gets the <see cref="User"/> against for who the permission were checked.
		/// </summary>
		public User User { get; set; }
		#endregion
	}
}