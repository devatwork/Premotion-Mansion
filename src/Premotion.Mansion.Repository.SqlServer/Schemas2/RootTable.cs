namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a table into values of one specific type are stored.
	/// </summary>
	public class RootTable : Table
	{
		#region Constructors
		/// <summary>
		/// Constructs a root table.
		/// </summary>
		/// <param name="tableName">The name of the root table.</param>
		public RootTable(string tableName) : base(tableName)
		{
			// create the columns
			Add(new IdentityColumn(this));
			Add(new OrderColumn());
			Add(new ExtendedPropertiesColumn());
		}
		#endregion
	}
}