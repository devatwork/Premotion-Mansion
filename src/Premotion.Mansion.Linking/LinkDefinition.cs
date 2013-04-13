using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Represents the definition of a link.
	/// </summary>
	public abstract class LinkDefinition
	{
		#region Constructors
		/// <summary>
		/// Constructs this link definition.
		/// </summary>
		/// <param name="name">The name of this link definition.</param>
		protected LinkDefinition(string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set values
			this.name = name;
		}
		#endregion
		#region Link Methods
		/// <summary>
		/// Creates the <see cref="Link"/> endpoints which represents a link between the <paramref name="source"/> and <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Linkbase"/> from which to create the link.</param>
		/// <param name="target">The target <see cref="Linkbase"/> to which to create the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <param name="sourceToTarget">The created source-to-target <see cref="Link"/>.</param>
		/// <param name="targetToSource">The created target-to-source <see cref="Link"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if a the specified link would not be valid.</exception>
		public static void Create(IMansionContext context, Linkbase source, Linkbase target, string name, IPropertyBag properties, out Link sourceToTarget, out Link targetToSource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (target == null)
				throw new ArgumentNullException("target");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// get the link definition instances
			var sourceLinkDefinition = source.Definition.GetLinkDefinition(name);
			var targetLinkDefinition = target.Definition.GetLinkDefinition(name);

			// create the link endpoint
			sourceToTarget = sourceLinkDefinition.CreateToLink(context, source, target, properties);
			targetToSource = targetLinkDefinition.CreateFromLink(context, source, target, properties, sourceToTarget);
		}
		/// <summary>
		/// Creates the <see cref="Link"/> from <paramref name="source"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Linkbase"/> from which to create the link.</param>
		/// <param name="target">The target <see cref="Linkbase"/> to which to create the link.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <exception cref="InvalidLinkException">Thrown if a the specified link would not be valid.</exception>
		protected abstract Link CreateToLink(IMansionContext context, Linkbase source, Linkbase target, IEnumerable<KeyValuePair<string, object>> properties);
		/// <summary>
		/// Creates the <see cref="Link"/> from <paramref name="target"/> to <paramref name="source"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Linkbase"/> from which to create the link.</param>
		/// <param name="target">The target <see cref="Linkbase"/> to which to create the link.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <param name="sourceToTarget">The <see cref="Link"/> for which to create a bi-directional <see cref="Link"/> back.</param>
		/// <exception cref="InvalidLinkException">Thrown if a the specified link would not be valid.</exception>
		protected abstract Link CreateFromLink(IMansionContext context, Linkbase source, Linkbase target, IEnumerable<KeyValuePair<string, object>> properties, Link sourceToTarget);
		/// <summary>
		/// Creates a <see cref="Link"/>.
		/// </summary>
		/// <param name="source">The source <see cref="Linkbase"/> from which to create the link.</param>
		/// <param name="target">The target <see cref="Linkbase"/> to which to create the link.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <returns>Returns the created <see cref="Link"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		protected Link CreateLink(Linkbase source, Linkbase target, IEnumerable<KeyValuePair<string, object>> properties)
		{
			// validate arguments
			if (source == null)
				throw new ArgumentNullException("source");
			if (target == null)
				throw new ArgumentNullException("target");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// create the link
			return new Link {
				Name = name,
				Properties = new PropertyBag(properties),
				SourceLinkbaseId = source.Id,
				TargetLinkbaseId = target.Id
			};
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this link definition.
		/// </summary>
		public string Name
		{
			get { return name; }
		}
		#endregion
		#region Private Fields
		private readonly string name;
		#endregion
	}
}