using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Descriptors;

namespace Premotion.Mansion.Web.Url
{
	/// <summary>
	/// Provides the default implementation for <see cref="INodeUrlService"/>.
	/// </summary>
	public class NodeUrlService : INodeUrlService
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nucleus"> </param>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public NodeUrlService(INucleus nucleus, ITypeService typeService)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
		}
		#endregion
		#region Implementation of INodeUrlService
		/// <summary>
		/// Generates a URL for <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <returns>Returns the generated <see cref="Uri"/>.</returns>
		public Uri Generate(IMansionWebContext context, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");

			// initialize the service
			Initialize(context);

			// get the node type
			var nodeType = typeService.Load(context, node.Pointer.Type);

			// get the generator
			NodeUrlGenerator generator;
			if (!generators.TryGetValue(nodeType, out generator))
				throw new InvalidOperationException(string.Format("Could not find url generator for type {0}, does the type exist?", nodeType.Name));

			// construct the base url
			var uriBuilder = new UriBuilder(context.ApplicationBaseUri);

			// generate the url
			generator.Generate(context, node, nodeType, uriBuilder);

			// return the generated uri
			return uriBuilder.Uri;
		}
		/// <summary>
		/// Parses the URL into query parameters.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="url">The <see cref="Uri"/> which to parse.</param>
		/// <param name="queryParameters">The query parameters extracted from <paramref name="url"/>.</param>
		/// <returns>Returns true when parameters could be extracted, otherwise false.</returns>
		public bool TryExtractQueryParameters(IMansionWebContext context, Uri url, out IPropertyBag queryParameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (url == null)
				throw new ArgumentNullException("url");

			// create bag for parameters
			queryParameters = new PropertyBag();

			// get the number of segments in the base uri
			var baseSegmentCount = context.ApplicationBaseUri.Segments.Length;

			// check for backoffice request
			if (context.IsBackoffice)
				baseSegmentCount += 1;

			// check if this is the base url
			if (url.Segments.Length <= baseSegmentCount)
				return false;

			// get the last segment which is the document name
			var candidateId = url.Segments[baseSegmentCount].Trim(Dispatcher.Constants.UrlPartTrimCharacters);

			// check if the candidate id is an actual number
			var isNumber = candidateId.IsNumber();

			// if it is a number, it is the page id
			if (isNumber)
				queryParameters.Set("id", candidateId);

			return isNumber;
		}
		#endregion
		#region Initialize Methods
		/// <summary>
		/// Initializes this service.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		private void Initialize(IMansionContext context)
		{
			// make thread safe
			if ((initializedState != 0) || (Interlocked.CompareExchange(ref initializedState, 1, 0) != 0))
				return;

			// create the url generators
			generators = typeService.LoadAll(context).Select(type =>
			                                                 {
			                                                 	// find the url generator for this type
			                                                 	NodeUrlGeneratorDescriptor nodeUrlGeneratorDescriptor;
			                                                 	if (!type.TryFindDescriptorInHierarchy(out nodeUrlGeneratorDescriptor))
			                                                 		throw new InvalidOperationException(string.Format("Type {0} does not have an URL generator, please check type configuration", type.Name));

			                                                 	// create the generator
			                                                 	return new
			                                                 	       {
			                                                 	       	Type = type,
			                                                 	       	Generator = nodeUrlGeneratorDescriptor.CreateGenerator(context)
			                                                 	       };
			                                                 }).ToDictionary(x => x.Type, x => x.Generator);
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		private IDictionary<ITypeDefinition, NodeUrlGenerator> generators;
		private int initializedState;
		#endregion
	}
}