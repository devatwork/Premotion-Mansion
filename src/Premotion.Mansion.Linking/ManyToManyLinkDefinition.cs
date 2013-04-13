using System.Collections.Generic;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Implements a many-to-many relation. Which allows any combiniation of <see cref="Link"/>s in the source and target <see cref="Linkbase"/>s.
	/// </summary>
	public class ManyToManyLinkDefinition : LinkDefinition
	{
		#region Constructors
		/// <summary>
		/// Constructs this link definition.
		/// </summary>
		/// <param name="name">The name of this link definition.</param>
		public ManyToManyLinkDefinition(string name) : base(name)
		{
		}
		#endregion
		#region Overrides of LinkDefinition
		/// <summary>
		/// Creates the <see cref="Link"/> from <paramref name="source"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Linkbase"/> from which to create the link.</param>
		/// <param name="target">The target <see cref="Linkbase"/> to which to create the link.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <exception cref="InvalidLinkException">Thrown if a the specified link would not be valid.</exception>
		protected override Link CreateToLink(IMansionContext context, Linkbase source, Linkbase target, IEnumerable<KeyValuePair<string, object>> properties)
		{
			// just create the link
			return CreateLink(source, target, properties);
		}
		/// <summary>
		/// Creates the <see cref="Link"/> from <paramref name="target"/> to <paramref name="source"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Linkbase"/> from which to create the link.</param>
		/// <param name="target">The target <see cref="Linkbase"/> to which to create the link.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <param name="sourceToTarget">The <see cref="Link"/> for which to create a bi-directional <see cref="Link"/> back.</param>
		/// <exception cref="InvalidLinkException">Thrown if a the specified link would not be valid.</exception>
		protected override Link CreateFromLink(IMansionContext context, Linkbase source, Linkbase target, IEnumerable<KeyValuePair<string, object>> properties, Link sourceToTarget)
		{
			// already did validation, just create the link
			return CreateLink(target, source, properties);
		}
		#endregion
	}
}