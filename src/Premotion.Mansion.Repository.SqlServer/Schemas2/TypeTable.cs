namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a table into values of one specific type are stored.
	/// </summary>
	public class TypeTable : Table
	{
		#region Constructors
		/// <summary>
		/// Constructs this table with the given <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of this table.</param>
		public TypeTable(string name) : base(name)
		{
			// create the columns
			Add(new JoinColumn());
		}
		#endregion
	}
}