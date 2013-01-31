using System;
using System.Collections.Generic;
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
		#region Http Methods
		/// <summary>
		/// Executes a HEAD request on the given <paramref name="resource"/>.
		/// </summary>
		/// <param name="resource">The resource on which to execute the request.</param>
		/// <param name="validHttpStatusCodes">Specifies which <see cref="HttpStatusCode"/>s are valid. Leave null for 200 - OK.</param>
		/// <returns>Returns the <see cref="IRestResponse"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="resource"/> is null.</exception>
		/// <exception cref="ConnectionException">Thrown if the request did not result in the expected <paramref name="validHttpStatusCodes"/>.</exception>
		public IRestResponse Head(string resource, IEnumerable<HttpStatusCode> validHttpStatusCodes = null)
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
		public IRestResponse Put(string resource, object obj, IEnumerable<HttpStatusCode> validHttpStatusCodes = null)
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");
			if (obj == null)
				throw new ArgumentNullException("obj");

			// execute the request
			return Execute(resource, request => {
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
		public TResponse Post<TResponse>(string resource, object obj, IEnumerable<HttpStatusCode> validHttpStatusCodes = null) where TResponse : BaseResponse, new()
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");
			if (obj == null)
				throw new ArgumentNullException("obj");

			// execute the request
			return Execute<TResponse>(resource, request => {
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
		public IRestResponse Delete(string resource, IEnumerable<HttpStatusCode> validHttpStatusCodes = null)
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
		private IRestResponse Execute(string resource, Action<IRestRequest> requestConfigurator, IEnumerable<HttpStatusCode> validHttpStatusCodes = null)
		{
			// create the request
			var request = new RestRequest(resource) {
				RequestFormat = DataFormat.Json,
				JsonSerializer = Serializer
			};

			requestConfigurator(request);

			// execute the request
			var response = Client.Execute(request);

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
		private IRestResponse<TResponse> Execute<TResponse>(string resource, Action<IRestRequest> requestConfigurator, IEnumerable<HttpStatusCode> validHttpStatusCodes = null) where TResponse : BaseResponse, new()
		{
			// create the request
			var request = new RestRequest(resource) {
				RequestFormat = DataFormat.Json,
				JsonSerializer = Serializer,
			};

			requestConfigurator(request);

			// execute the request
			var response = Client.Execute<TResponse>(request);

			// error handling
			if (response.ErrorException != null)
				throw new ConnectionException("Error while parsing the response", response.ErrorException, request, response);
			if ((validHttpStatusCodes == null && response.StatusCode != HttpStatusCode.OK) || (validHttpStatusCodes != null && validHttpStatusCodes.All(candidate => candidate != response.StatusCode)))
				throw new ConnectionException("Invalid ElasticSearch request", request, response);

			// return the response
			return response;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="RestClient"/>.
		/// </summary>
		private RestClient Client
		{
			get
			{
				var client = clientFactory.Value;
				if (client == null)
					throw new InvalidOperationException("ElasticSearch is disabled, please check configuration");
				return client;
			}
		}
		#endregion
		#region Private Fields
		private static readonly IDeserializer Deserializer = new ElasticSearchDeserializer();
		private static readonly ISerializer Serializer = new ElasticSearchJsonSerializer();
		private readonly Lazy<RestClient> clientFactory = new Lazy<RestClient>(() => {
			// check if elastic search is disabled
			if (!Configuration.IsEnabled)
				return null;

			// create the client
			var client = new RestClient(Configuration.ConnectionString);
			client.ClearHandlers();
			client.AddHandler("*", Deserializer);
			client.Timeout = 20*60*1000;
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.UseNagleAlgorithm = false;
			ServicePointManager.MaxServicePointIdleTime = 3600000;
			ServicePointManager.SetTcpKeepAlive(true, 30000, 3000);
			return client;
		});
		#endregion
	}
}