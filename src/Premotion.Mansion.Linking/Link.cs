using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Represents a link between two <see cref="Linkbase"/>s.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class Link
	{
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
			return linkbase.Id.Equals(TargetLinkbaseId);
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
			return Name.Equals(definition.Name, StringComparison.OrdinalIgnoreCase);
		}
		#endregion
		#region Properties Fields
		/// <summary>
		/// The name of this link.
		/// </summary>
		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; }
		/// <summary>
		/// Indicates the direction of link. If true then the link was created from the source to the target, otherwise the link was created from the target to the source.
		/// </summary>
		[JsonProperty("originating", Required = Required.Default)]
		public bool Originating { get; set; }
		/// <summary>
		/// The properties of this link.
		/// </summary>
		[JsonProperty("properties", Required = Required.Always)]
		public PropertyBag Properties { get; set; }
		/// <summary>
		/// The source linkbase ID.
		/// </summary>
		[JsonProperty("sourceLinkbaseId", Required = Required.Always)]
		public string SourceLinkbaseId { get; set; }
		/// <summary>
		/// The target linkbase ID.
		/// </summary>
		[JsonProperty("targetLinkbaseId", Required = Required.Always)]
		public string TargetLinkbaseId { get; set; }
		#endregion
	}
}