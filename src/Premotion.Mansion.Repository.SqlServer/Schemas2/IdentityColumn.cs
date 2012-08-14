namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents an identity column
	/// </summary>
	public class IdentityColumn : Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public IdentityColumn() : base("id")
		{
		}
		#endregion
	}
}