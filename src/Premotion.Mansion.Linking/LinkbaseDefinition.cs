using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Contains the definition of a <see cref="Linkbase"/>.
	/// </summary>
	public class LinkbaseDefinition
	{
		#region Constructors
		/// <summary>
		/// Constructs a link definition.
		/// </summary>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <param name="linkDefinitions">The <see cref="LinkDefinition"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public LinkbaseDefinition(ITypeDefinition type, IEnumerable<LinkDefinition> linkDefinitions)
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");
			if (linkDefinitions == null)
				throw new ArgumentNullException("linkDefinitions");

			// set the values
			this.type = type;
			this.linkDefinitions = linkDefinitions.ToDictionary(x => x.Name);
		}
		#endregion
		#region Link Definition Methods
		/// <summary>
		/// Gets the <see cref="LinkDefinition"/> identified by it's <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of the link type.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null or empty.</exception>
		/// <exception cref="InvalidLinkException">Thrown if no <see cref="LinkDefinition"/> could be found in this <see cref="LinkbaseDefinition"/>.</exception>
		public LinkDefinition GetLinkDefinition(string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// try to get the definition
			LinkDefinition link;
			if (!linkDefinitions.TryGetValue(name, out link))
				throw new InvalidLinkException(string.Format("Type '{0}' does not have a definition named '{1}'", type.Name, name));
			return link;
		}
		#endregion
		#region Private Fields
		private readonly IDictionary<string, LinkDefinition> linkDefinitions;
		private readonly ITypeDefinition type;
		#endregion
	}
}