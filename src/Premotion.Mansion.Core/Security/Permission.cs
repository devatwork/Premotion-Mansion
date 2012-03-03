namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Represents a permission which links a <see cref="Role"/> to an <see cref="ProtectedOperation"/>.
	/// </summary>
	public class Permission
	{
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return (Granted ? "grants" : "denies") + " permission on " + Operation + " with priority" + Priority;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether permission is granted or denied.
		/// </summary>
		public bool Granted { get; set; }
		/// <summary>
		/// Gets the priority of this permission. <see cref="Permission"/>s with higher <see cref="Priority"/> will overrule those with lower <see cref="Priority"/>.
		/// </summary>
		public int Priority { get; set; }
		/// <summary>
		/// Gets the <see cref="ProtectedOperation"/> to wich this <see cref="Permission"/> is <see cref="Granted"/> or denied.
		/// </summary>
		public ProtectedOperation Operation { get; set; }
		#endregion
	}
}