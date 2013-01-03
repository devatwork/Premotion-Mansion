using System;
using System.Configuration;
using System.Linq;
using System.Net;
using Premotion.Mansion.Repository.ElasticSearch.Responses;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

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
			client.AddHandler("application/json", deserializer);
			client.AddHandler("text/json", deserializer);
			client.Timeout = 20*60*1000;
			ServicePointManager.Expect100Continue = true; 
         ServicePointManager.UseNagleAlgorithm = false; 
		}
		#endregion
		#region Http Methods
		/// <summary>
		/// Executes a HEAD request on the given <paramref name="resource"/>.
		/// </summary>
		/// <param name="resource">The resource on which to execute the request.</param>
		/// <param name="validHttpStatusCodes">Specifies which <see cref="HttpStatusCode"/>s are valid. Leave null for 200 - OK.</param>
		/// <returns>Returns the <see cref="IRestResponse"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="resource"/> is null.</exception>
		/// <exception cref="ConnectionException">Thrown if the request did not result in the expected <paramref name="validHttpStatusCodes"/>.</exception>
		public IRestResponse Head(string resource, HttpStatusCode[] validHttpStatusCodes = null)
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");

			return Execute(resource, request => { request.Method = Method.HEAD; }, validHttpStatusCodes);
		}
		/// <summary>
		/// Executes a PUT request on the given <paramref name="resource"/>.
		/// </summary>
		/// <param name="resource">The resource on which to execute the request.</param>
		/// <param name="obj">The object which to add to the body of the request.</param>
		/// <param name="validHttpStatusCodes">Specifies which <see cref="HttpStatusCode"/>s are valid. Leave null for 200 - OK.</param>
		/// <returns>Returns the <see cref="IRestResponse"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="resource"/> is null.</exception>
		/// <exception cref="ConnectionException">Thrown if the request did not result in the expected <paramref name="validHttpStatusCodes"/>.</exception>
		public IRestResponse Put(string resource, object obj, HttpStatusCode[] validHttpStatusCodes = null)
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");
			if (obj == null)
				throw new ArgumentNullException("obj");

			// execute the request
			return Execute(resource, request =>
			                         {
			                         	request.Method = Method.PUT;
			                         	request.AddBody(obj);
			                         }, validHttpStatusCodes);
		}
		/// <summary>
		/// Executes a POST request on the given <paramref name="resource"/>.
		/// </summary>
		/// <param name="resource">The resource on which to execute the request.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="resource"/> is null.</exception>
		/// <exception cref="ConnectionException">Thrown if the request did not result in the expected result.</exception>
		public void Post(string resource)
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");

			// execute the request
			Execute(resource, request => { request.Method = Method.POST; });
		}
		/// <summary>
		/// Executes a POST request on the given <paramref name="resource"/>.
		/// </summary>
		/// <typeparam name="TResponse">The <see cref="BaseResponse"/> type.</typeparam>
		/// <param name="resource">The resource on which to execute the request.</param>
		/// <param name="obj">The object which to add to the body of the request.</param>
		/// <param name="validHttpStatusCodes">Specifies which <see cref="HttpStatusCode"/>s are valid. Leave null for 200 - OK.</param>
		/// <returns>Returns the <typeparamref name="TResponse"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="resource"/> is null.</exception>
		/// <exception cref="ConnectionException">Thrown if the request did not result in the expected <paramref name="validHttpStatusCodes"/>.</exception>
		public TResponse Post<TResponse>(string resource, object obj, HttpStatusCode[] validHttpStatusCodes = null) where TResponse : BaseResponse, new()
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");
			if (obj == null)
				throw new ArgumentNullException("obj");

			// execute the request
			return Execute<TResponse>(resource, request =>
			                                    {
			                                    	request.Method = Method.POST;
			                                    	request.AddBody(obj);
			                                    }, validHttpStatusCodes).Data;
		}
		/// <summary>
		/// Executes a DELETE request on the given <paramref name="resource"/>.
		/// </summary>
		/// <param name="resource">The resource on which to execute the request.</param>
		/// <param name="validHttpStatusCodes">Specifies which <see cref="HttpStatusCode"/>s are valid. Leave null for 200 - OK.</param>
		/// <returns>Returns the <see cref="IRestResponse"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="resource"/> is null.</exception>
		/// <exception cref="ConnectionException">Thrown if the request did not result in the expected <paramref name="validHttpStatusCodes"/>.</exception>
		public IRestResponse Delete(string resource, HttpStatusCode[] validHttpStatusCodes = null)
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");

			return Execute(resource, request => { request.Method = Method.DELETE; }, validHttpStatusCodes);
		}
		/// <summary>
		/// Executes a request on ElasticSearch.
		/// </summary>
		/// <param name="resource">The resource on which to execute the request.</param>
		/// <param name="requestConfigurator">Action which configurates the <see cref="IRestRequest"/>.</param>
		/// <param name="validHttpStatusCodes">Defines the valid status codes.</param>
		/// <returns>Returns the <see cref="IRestResponse"/> of the request.</returns>
		/// <exception cref="ConnectionException">Thrown if the request did not result in the expected <paramref name="validHttpStatusCodes"/>.</exception>
		private IRestResponse Execute(string resource, Action<IRestRequest> requestConfigurator, HttpStatusCode[] validHttpStatusCodes = null)
		{
			// create the request
			var request = new RestRequest(resource)
			              {
			              	RequestFormat = DataFormat.Json,
			              	JsonSerializer = serializer
			              };

			requestConfigurator(request);

			// execute the request
			var response = client.Execute(request);

			// error handling
			if (response.ErrorException != null)
				throw new ConnectionException("Error while parsing the response", response.ErrorException, request, response);
			if ((validHttpStatusCodes == null && response.StatusCode != HttpStatusCode.OK) || (validHttpStatusCodes != null && validHttpStatusCodes.All(candidate => candidate != response.StatusCode)))
				throw new ConnectionException("Invalid ElasticSearch request", request, response);

			// return the response
			return response;
		}
		/// <summary>
		/// Executes a request on ElasticSearch.
		/// </summary>
		/// <typeparam name="TResponse">The <see cref="BaseResponse"/> type.</typeparam>
		/// <param name="resource">The resource on which to execute the request.</param>
		/// <param name="requestConfigurator">Action which configurates the <see cref="IRestRequest"/>.</param>
		/// <param name="validHttpStatusCodes">Defines the valid status codes.</param>
		/// <returns>Returns the <see cref="IRestResponse"/> of the request.</returns>
		/// <exception cref="ConnectionException">Thrown if the request did not result in the expected <paramref name="validHttpStatusCodes"/>.</exception>
		private IRestResponse<TResponse> Execute<TResponse>(string resource, Action<IRestRequest> requestConfigurator, HttpStatusCode[] validHttpStatusCodes = null) where TResponse : BaseResponse, new()
		{
			// create the request
			var request = new RestRequest(resource)
			              {
			              	RequestFormat = DataFormat.Json,
			              	JsonSerializer = serializer,
			              };

			requestConfigurator(request);

			// execute the request
			var response = client.Execute<TResponse>(request);

			// error handling
			if (response.ErrorException != null)
				throw new ConnectionException("Error while parsing the response", response.ErrorException, request, response);
			if ((validHttpStatusCodes == null && response.StatusCode != HttpStatusCode.OK) || (validHttpStatusCodes != null && validHttpStatusCodes.All(candidate => candidate != response.StatusCode)))
				throw new ConnectionException("Invalid ElasticSearch request", request, response);

			// return the response
			return response;
		}
		#endregion
		#region Private Fields
		private readonly RestClient client;
		private readonly IDeserializer deserializer = new ElasticSearchDeserializer();
		private readonly ISerializer serializer = new ElasticSearchJsonSerializer();
		#endregion
	}
}