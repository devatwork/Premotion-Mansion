using System;
using System.Configuration;
using System.Net;
using Premotion.Mansion.Core;
using RestSharp;

namespace Premotion.Mansion.Repository.ElasticSearch.Connection
{
	/// <summary>
	/// Implements the ElasticSearch connection manager.
	/// </summary>
	public class ConnectionManager
	{
		#region Constructors
		/// <summary>
		/// Constructs an ElasticSearch connection manager.
		/// </summary>
		public ConnectionManager()
		{
			// get the connection string
			var appSettings = ConfigurationManager.AppSettings;
			var connectionString = appSettings[Constants.ConnectionStringApplicationSettingKey];
			if (string.IsNullOrEmpty(connectionString))
				throw new InvalidOperationException("No valid value found in application settings for key " + Constants.ConnectionStringApplicationSettingKey);

			// create the client
			client = new RestClient(connectionString);
		}
		#endregion
		#region Private Fields
		private readonly RestClient client;
		#endregion
		#region Http Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="resource"></param>
		/// <param name="obj"></param>
		/// <exception cref="ConnectionException">Thrown if the request did not result in 200 - OK.</exception>
		public void Put(IMansionContext context, string resource, object obj)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resource == null)
				throw new ArgumentNullException("resource");
			if (obj == null)
				throw new ArgumentNullException("obj");

			// execute the request
			Execute(resource, request =>
			                  {
			                  	request.Method = Method.PUT;
			                  	request.AddBody(obj);
			                  });
		}
		/// <summary>
		/// Executes a request on ElasticSearch.
		/// </summary>
		/// <param name="resource">The resource on which to execute the request.</param>
		/// <param name="requestConfigurator">Action which configurates the <see cref="IRestRequest"/>.</param>
		/// <returns>Returns the <see cref="IRestResponse"/> of the request.</returns>
		/// <exception cref="ConnectionException">Thrown if the request did not result in 200 - OK.</exception>
		private IRestResponse Execute(string resource, Action<IRestRequest> requestConfigurator)
		{
			// create the request
			var request = new RestRequest(resource)
			              {
			              	RequestFormat = DataFormat.Json
			              };
			requestConfigurator(request);

			// execute the request
			var response = client.Execute(request);

			// error handling
			if (response.StatusCode != HttpStatusCode.OK)
				throw new ConnectionException("Invalid ElasticSearch request", request, response);

			// return the response
			return response;
		}
		#endregion
	}
}