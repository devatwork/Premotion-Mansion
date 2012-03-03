using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus.Facilities.Dependencies;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Implements <see cref="ITagScriptService"/>.
	/// </summary>
	public class TagScriptService : ITagScriptService, IServiceWithDependencies
	{
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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="resources">The resources which to open.</param>
		/// <returns>Returns a marker which will close the scripts automatically.</returns>
		public IDisposable Open(MansionContext context, IEnumerable<IResource> resources)
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
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="resource">The resource wcich to parse as script.</param>
		/// <returns>Returns the parsed script.</returns>
		/// <exception cref="ParseScriptException">Thrown when an exception occurres while parsing the script.</exception>
		public ITagScript Parse(MansionContext context, IResource resource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// get the cache service
			var cacheKey = ResourceCacheKey.Create(resource);
			var cacheService = context.Nucleus.Get<ICachingService>(context);

			// open the script
			return new TagScript(cacheService.GetOrAdd(
				context,
				cacheKey,
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

			// read in the XML document
			var document = new XmlDocument();
			using (var openResource = resource.OpenForReading())
			using (var reader = new MansionScriptXmlTextReader(openResource.Reader))
				document.Load(reader);
			if (document.DocumentElement == null)
				throw new InvalidOperationException("The document root can not be null");

			return document.DocumentElement;
		}
		#endregion
		#region Resolve Methods
		/// <summary>
		/// Resolves an XML node to a script tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The node which to resolve.</param>
		/// <param name="resource">The resource to which the tag belongs.</param>
		/// <returns>Returns the resolved script tag.</returns>
		private static ScriptTag Resolve(MansionContext context, XmlNode node, IResource resource)
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
			var namingService = context.Nucleus.Get<ITypeDirectoryService>(context);
			var objectFactoryService = context.Nucleus.Get<IObjectFactoryService>(context);
			Type tagType;
			if (!namingService.TryLookupSingle<ScriptTag>(node.NamespaceURI, localName, out tagType))
				throw new ParseScriptException(string.Format("Could not find instance of tag '{0}' in namespace '{1}'. Tag is defined on line '{2}' in '{3}'", localName, node.NamespaceURI, lineNumber, resource.GetResourceIdentifier()));
			var tag = objectFactoryService.Create<ScriptTag>(tagType);

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
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parentNode">The parent node.</param>
		/// <param name="parentTag">The parent tag.</param>
		/// <param name="resource">The resource.</param>
		private static void ParseChilderen(MansionContext context, XmlNode parentNode, ScriptTag parentTag, IResource resource)
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
		#region Implementation of IServiceWithDependencies
		/// <summary>
		/// Gets the <see cref="DependencyModel"/> of this service.
		/// </summary>
		public DependencyModel Dependencies
		{
			get { return dependencies; }
		}
		#endregion
		#region Private Fields
		private static readonly DependencyModel dependencies = new DependencyModel().Add<ITypeDirectoryService>().Add<IObjectFactoryService>().Add<ICachingService>();
		#endregion
	}
}