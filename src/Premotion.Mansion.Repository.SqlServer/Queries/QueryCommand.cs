using System.Data.SqlClient;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Base class for all commands.
	/// </summary>
	public abstract class QueryCommand : DisposableBase
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="Schema"/> of this command.
		/// </summary>
		public Schema Schema { get; protected set; }
		/// <summary>
		/// Gets the <see cref="SqlStringBuilder"/> of this command.
		/// </summary>
		public SqlStringBuilder QueryBuilder { get; protected set; }
		/// <summary>
		/// Gets the <see cref="SqlCommand"/> of this command.
		/// </summary>
		public SqlCommand Command { get; protected set; }
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			if (!disposeManagedResources)
				return;

			// cleanup
			Command.Dispose();
		}
		#endregion
		// TODO: clean up this class
	}
}