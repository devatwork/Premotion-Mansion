using System;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Enumerates all the states a node can have.
	/// </summary>
	[Flags]
	public enum NodeStatus
	{
		/// <summary>
		/// This node is not approved yet and therefore can not be published.
		/// </summary>
		Draft = 0x1,
		/// <summary>
		/// This node is approved and the publication date is in the future.
		/// </summary>
		Staged = 0x2,
		/// <summary>
		/// This node is published.
		/// </summary>
		Published = 0x4,
		/// <summary>
		/// This nodes expiration date has passed.
		/// </summary>
		Expired = 0x8,
		/// <summary>
		/// This nodes is archived.
		/// </summary>
		Archived = 0x10,
		/// <summary>
		/// Any status.
		/// </summary>
		Any = Draft | Staged | Published | Expired | Archived
	}
}