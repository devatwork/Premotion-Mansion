using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Implements <see cref="ITagScriptService"/>.
	/// </summary>
	public class TagScriptService : ITagScriptService
	{
		#region Constructors
		/// <summary>
		/// Constructs the tag script service.
		/// </summary>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="cachingService"/> is null.</exception>
		public TagScriptService(ICachingService cachingService)
		{
			// validate arguments
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// set values
			this.cachingService = cachingService;
		}
		#endregion
		#region Nested type: CachedTagScript
		/// <summary>
		/// Implements <see cref="CachedObject{TObject}"/> for <see cref="TagScript"/>.
		/// </summary>
		private class CachedTagScript : CachedObject<ScriptTag>
		{
			#region Constructors
			/// <summary>
			/// Constructs a new cached object.
			/// </summary>
			/// <param name="obj">The object which to cache.</param>
			public CachedTagScript(ScriptTag obj) : base(obj)
			{
				Priority = Priority.NotRemovable;
			}
			#endregion
		}
		#endregion
		#region Nested type: MansionScriptXmlTextReader
		/// <summary>
		/// Implements the <see cref="XmlTextReader"/> for mansion tag script to extract line numbers.
		/// </summary>
		private class MansionScriptXmlTextReader : XmlTextReader
		{
			#region Constructors
			/// <summary>
			/// Constructs a mansion tag script XmlTextReader.
			/// </summary>
			/// <param name="reader"></param>
			public MansionScriptXmlTextReader(TextReader reader) : base(reader)
			{
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets the local name of the current node.
			/// </summary>
			/// <returns>
			/// The name of the current node with the prefix removed. For example, LocalName is book for the element &lt;bk:book&gt;.For node types that do not have a name (like Text, Comment, and so on), this property returns String.Empty.
			/// </returns>
			public override string LocalName
			{
				get
				{
					// check for type
					return NodeType == XmlNodeType.Element ? base.LocalName + ',' + LineNumber : base.LocalName;
				}
			}
			#endregion
		}
		#endregion
		#region Implementation of IScriptService<out ITagScript>
		/// <summary>
		/// Opens a series of scripts.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resources">The resources which to open.</param>
		/// <returns>Returns a marker which will close the scripts automatically.</returns>
		public IDisposable Open(IMansionContext context, IEnumerable<IResource> resources)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resources == null)
				throw new ArgumentNullException("resources");

			// create a stack group key
			var groupKey = new AutoStackKeyGroup();

			// loop through all the scripts
			foreach (var resource in resources)
			{
				// parse the scripts
				var script = Parse(context, resource);

				// initialize the tag script
				script.Initialize(context);

				// push the script to the stack
				groupKey.Push(context.ScriptStack.Push(script, x => x.Dispose()));
			}

			return groupKey;
		}
		/// <summary>
		/// Parses a script from the specified resource.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resource">The resource wcich to parse as script.</param>
		/// <returns>Returns the parsed script.</returns>
		/// <exception cref="ParseScriptException">Thrown when an exception occurres while parsing the script.</exception>
		public ITagScript Parse(IMansionContext context, IResource resource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// open the script
			return new TagScript(cachingService.GetOrAdd(
				context,
				ResourceCacheKey.Create(resource),
				() =>
				{
					try
					{
						// load the script
						var rootNode = LoadScript(resource);

						// resolve the document root tag
						var rootTag = Resolve(context, rootNode, resource);

						// parse the rest of the script
						ParseChilderen(context, rootNode, rootTag, resource);

						// return the parsed script cache object
						return new CachedTagScript(rootTag);
					}
					catch (Exception ex)
					{
						throw new ParseScriptException(resource, ex);
					}
				}));
		}
		#endregion
		#region Load Methods
		/// <summary>
		/// Loads the XML node from the resource.
		/// </summary>
		/// <param name="resource">The resource from which to load.</param>
		/// <returns>Returns the loaded XML node.</returns>
		private static XmlNode LoadScript(IResource resource)
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");

			try
			{
				// read in the XML document
				var document = new XmlDocument();
				using (var openResource = resource.OpenForReading())
				using (var reader = new MansionScriptXmlTextReader(openResource.Reader))
					document.Load(reader);
				if (document.DocumentElement == null)
					throw new InvalidOperationException("The document root can not be null");

				return document.DocumentElement;
			}
			catch (Exception ex)
			{
				throw new Exception(String.Format("Failed to load script {0}", resource.GetResourceIdentifier()), ex);
			}
		}
		#endregion
		#region Resolve Methods
		/// <summary>
		/// Resolves an XML node to a script tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which to resolve.</param>
		/// <param name="resource">The resource to which the tag belongs.</param>
		/// <returns>Returns the resolved script tag.</returns>
		private static ScriptTag Resolve(IMansionContext context, XmlNode node, IResource resource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// get the name of the tag, the additional info is added by the MansionScriptXmlTextReader
			var parts = node.LocalName.Split(',');
			var localName = parts[0];
			var lineNumber = int.Parse(parts[1]);

			// create the tag instance
			var tag = context.Nucleus.ResolveSingle<ScriptTag>(node.NamespaceURI, localName);

			// set tag info
			tag.Info.Name = localName;
			tag.Info.Namespace = node.NamespaceURI;
			tag.Info.LineNumber = lineNumber;
			tag.Info.Resource = resource;

			// initialize the tag
			tag.Initialize(context, node);

			// return the resolved tag
			return tag;
		}
		#endregion
		#region Parse Methods
		/// <summary>
		/// Parses the childeren of <paramref name="parentNode"/> and adds them as <see cref="ScriptTag"/>s to <paramref name="parentTag"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parentNode">The parent node.</param>
		/// <param name="parentTag">The parent tag.</param>
		/// <param name="resource">The resource.</param>
		private static void ParseChilderen(IMansionContext context, XmlNode parentNode, ScriptTag parentTag, IResource resource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parentNode == null)
				throw new ArgumentNullException("parentNode");
			if (parentTag == null)
				throw new ArgumentNullException("parentTag");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// loop through all the child nodes
			foreach (XmlNode childNode in parentNode.ChildNodes)
			{
				// filter on elements only
				if (childNode.NodeType != XmlNodeType.Element)
					continue;

				// get an instance of this child tag
				var childTag = Resolve(context, childNode, resource);

				// add the new child to the parent
				parentTag.Add(context, childTag);

				// check if this child tag has children of it's own
				if (childNode.HasChildNodes)
					ParseChilderen(context, childNode, childTag, resource);
			}
		}
		#endregion
		#region Private Fields
		private readonly ICachingService cachingService;
		#endregion
	}
}