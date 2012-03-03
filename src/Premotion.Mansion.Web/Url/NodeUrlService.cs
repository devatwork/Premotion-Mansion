using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Dependencies;
using Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Descriptors;

namespace Premotion.Mansion.Web.Url
{
	/// <summary>
	/// Provides the default implementation for <see cref="INodeUrlService"/>.
	/// </summary>
	public class NodeUrlService : ManagedLifecycleService, IServiceWithDependencies, INodeUrlService
	{
		#region Implementation of INodeURLService
		/// <summary>
		/// Generates a URL for <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> for which to generate the URL.</param>
		/// <returns>Returns the generated <see cref="Uri"/>.</returns>
		public Uri Generate(MansionWebContext context, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");

			// get the node type
			var typeService = context.Nucleus.Get<ITypeService>(context);
			var nodeType = typeService.Load(context, node.Pointer.Type);

			// get the generator
			NodeUrlGenerator generator;
			if (!generators.TryGetValue(nodeType, out generator))
				throw new InvalidOperationException(string.Format("Could not find url generator for type {0}, does the type exist?", nodeType.Name));

			// construct the base url
			var uriBuilder = new UriBuilder(context.HttpContext.Request.ApplicationBaseUri);

			// generate the url
			generator.Generate(context, node, nodeType, uriBuilder);

			// return the generated uri
			return uriBuilder.Uri;
		}
		/// <summary>
		/// Parses the URL into query parameters.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="url">The <see cref="Uri"/> which to parse.</param>
		/// <param name="queryParameters">The query parameters extracted from <paramref name="url"/>.</param>
		/// <returns>Returns true when parameters could be extracted, otherwise false.</returns>
		public bool TryExtractQueryParameters(MansionContext context, Uri url, out IPropertyBag queryParameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (url == null)
				throw new ArgumentNullException("url");

			// create bag for parameters
			queryParameters = new PropertyBag();

			// get the last segment which is the document name
			var documentName = url.Segments[url.Segments.Length - 1];

			// get last and second last dot
			var lastDotPosition = documentName.LastIndexOf('.');
			var secondLastDotPosition = documentName.LastIndexOf('.', lastDotPosition - 1);
			if (lastDotPosition != -1 && secondLastDotPosition != -1)
				queryParameters.Set("id", documentName.Substring(secondLastDotPosition + 1, lastDotPosition - secondLastDotPosition - 1));

			return queryParameters.Count > 0;
		}
		#endregion
		#region Override of ManagedLifecycleService
		/// <summary>
		/// Invoked just before this service is used for the first time.
		/// </summary>
		/// <param name="context">The <see cref="NucleusContext"/>.</param>
		protected override void DoStart(INucleusAwareContext context)
		{
			// load all the types and get their url generators
			var typeService = context.Nucleus.Get<ITypeService>(context);
			foreach (var type in typeService.LoadAll(context))
			{
				// find the url generator for this type
				NodeUrlGeneratorDescriptor nodeUrlGeneratorDescriptor;
				if (!type.TryFindDescriptorInHierarchy(out nodeUrlGeneratorDescriptor))
					throw new InvalidOperationException(string.Format("Type {0} does not have an URL generator, please check type configuration", type.Name));

				// create the generator
				var generator = nodeUrlGeneratorDescriptor.CreateGenerator(context);

				// add to the map
				generators.Add(type, generator);
			}

			base.DoStart(context);
		}
		#endregion
		#region Implementation of IServiceWithDependencies
		/// <summary>
		/// Gets the <see cref="DependencyModel"/> of this service.
		/// </summary>
		DependencyModel IServiceWithDependencies.Dependencies
		{
			get { return dependencies; }
		}
		#endregion
		#region Private Fields
		private static readonly DependencyModel dependencies = new DependencyModel().Add<ITypeService>().Add<ITypeDirectoryService>().Add<IObjectFactoryService>();
		private readonly Dictionary<ITypeDefinition, NodeUrlGenerator> generators = new Dictionary<ITypeDefinition, NodeUrlGenerator>();
		#endregion
	}
}