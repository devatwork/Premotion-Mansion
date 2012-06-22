using System;
using System.Data.SqlClient;
using System.Text;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Implement extensions for SQL server types.
	/// </summary>
	public static class Extensions
	{
		#region SqlCommand Extensions
		/// <summary>
		/// Adds a parameter to this command and returns it's name.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <returns>Returns the name of the added parameter.</returns>
		public static string AddParameter(this SqlCommand command, object value)
		{
			// validate arguments
			if (command == null)
				throw new ArgumentNullException("command");

			return command.Parameters.AddWithValue(command.Parameters.Count.ToString(), value).ParameterName;
		}
		/// <summary>
		/// Adds a parameter to this command and returns it's name.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <returns>Returns the name of the added parameter.</returns>
		public static void AddParameter(this SqlCommand command, object value, string parameterName)
		{
			// validate arguments
			if (command == null)
				throw new ArgumentNullException("command");
			if (string.IsNullOrEmpty(parameterName))
				throw new ArgumentNullException("parameterName");

			command.Parameters.AddWithValue(parameterName, value);
		}
		#endregion
		#region StringBuilder Extensions
		/// <summary>
		/// Trims the string builder for IN statement.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static string Trim(this StringBuilder builder)
		{
			// validate arguments
			if (builder == null)
				throw new ArgumentNullException("builder");

			// trim
			return builder.ToString().Trim(' ', ',');
		}
		#endregion
	}
}