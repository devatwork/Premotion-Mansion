using System;
using System.Text;
using RestSharp;

namespace Premotion.Mansion.Repository.ElasticSearch.Connection
{
	/// <summary>
	/// Represents an ElasticSearch connection exception.
	/// </summary>
	public class ConnectionException : Exception
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="request"></param>
		/// <param name="response"></param>
		public ConnectionException(string message, IRestRequest request, IRestResponse response) : base(message)
		{
			// loop over the request parameters
			var builder = new StringBuilder();
			foreach (var param in request.Parameters)
				builder.AppendFormat(" - {0} ** {1} ** {2}{3}", param.Name, param.Type, param.Value, Environment.NewLine);

			Details = string.Format("Message: {6}, {0}StatusCode: {1}, {0}Method: {2}, {0}Url: {3}, {0}{0}Request:{0} {4} {0}Response:{0}{5}", Environment.NewLine, response.StatusCode, request.Method, request.Resource, builder, response.Content, message);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the request details.
		/// </summary>
		public string Details { get; private set; }
		#endregion
		#region Overrides of Exception
		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <returns>
		/// The error message that explains the reason for the exception, or an empty string("").
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override string Message
		{
			get { return Details; }
		}
		#endregion
	}
}