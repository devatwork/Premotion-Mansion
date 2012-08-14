using System;
using System.Data;
using System.Data.SqlClient;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Repository.SqlServer.Schemas2;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Base class for all commands.
	/// </summary>
	public class QueryCommandContext : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// Construct this query command context.
		/// </summary>
		/// <param name="schema">The <see cref="Schema"/>.</param>
		/// <param name="command">The <see cref="SqlCommand"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public QueryCommandContext(Schema schema, SqlCommand command)
		{
			// validate arguments
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (command == null)
				throw new ArgumentNullException("command");

			//  set the values
			Schema = schema;
			Command = command;
			Command.CommandType = CommandType.Text;
			QueryBuilder = new SqlStringBuilder(Schema.RootTable);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="Schema"/> of this command.
		/// </summary>
		public Schema Schema { get; private set; }
		/// <summary>
		/// Gets the <see cref="SqlStringBuilder"/> of this command.
		/// </summary>
		public SqlStringBuilder QueryBuilder { get; private set; }
		/// <summary>
		/// Gets the <see cref="SqlCommand"/> of this command.
		/// </summary>
		public SqlCommand Command { get; private set; }
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
	}
}