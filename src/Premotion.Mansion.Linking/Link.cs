using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Represents a link between two <see cref="Linkbase"/>s.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class Link
	{
		#region Constructors
		/// <summary>
		/// Constructs a Link.
		/// </summary>
		/// <param name="name">The name of the link.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <param name="sourceLinkbaseId">The ID of the linkbase from which this link was made.</param>
		/// <param name="targetLinkbaseId">The IDNof the linkbase to which this link was made.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public Link(string name, IPropertyBag properties, string sourceLinkbaseId, string targetLinkbaseId)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (properties == null)
				throw new ArgumentNullException("properties");
			if (string.IsNullOrEmpty(sourceLinkbaseId))
				throw new ArgumentNullException("sourceLinkbaseId");
			if (string.IsNullOrEmpty(targetLinkbaseId))
				throw new ArgumentNullException("targetLinkbaseId");

			// set values
			this.name = name;
			this.properties = properties;
			this.sourceLinkbaseId = sourceLinkbaseId;
			this.targetLinkbaseId = targetLinkbaseId;
		}
		#endregion
		#region Boolean Methods
		/// <summary>
		/// Checks whether this Link is a to the given <paramref name="linkbase"/>.
		/// </summary>
		/// <param name="linkbase">The target <see cref="Linkbase"/>.</param>
		/// <returns>Returns true if this Link links to the <paramref name="linkbase"/>, otherwise false.</returns>
		public bool IsTo(Linkbase linkbase)
		{
			// validate arguments
			if (linkbase == null)
				throw new ArgumentNullException("linkbase");
			return linkbase.Id.Equals(targetLinkbaseId);
		}
		/// <summary>
		/// Checks if this Link is an instance of <paramref name="definition"/>.
		/// </summary>
		/// <param name="definition">The <see cref="LinkDefinition"/>.</param>
		/// <returns>Returns true if this Link is an instance of <paramref name="definition"/>, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public bool IsInstanceOf(LinkDefinition definition)
		{
			//  validate arguments
			if (definition == null)
				throw new ArgumentNullException("definition");
			return name.Equals(definition.Name, StringComparison.OrdinalIgnoreCase);
		}
		#endregion
		#region Private Fields
		[JsonProperty("name")]
		private readonly string name;
		[JsonProperty("properties")]
		private readonly IPropertyBag properties;
		[JsonProperty("sourceLinkbaseId")]
		private readonly string sourceLinkbaseId;
		[JsonProperty("targetLinkbaseId")]
		private readonly string targetLinkbaseId;
		#endregion
	}
}