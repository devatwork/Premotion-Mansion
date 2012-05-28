using System;

namespace Premotion.Mansion.Web.Social
{
	/// <summary>
	/// A normalized model representing an ID of an object for a specific social service provider.
	/// </summary>
	public class SocialId
	{
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="SocialId"/>.
		/// </summary>
		/// <param name="id">The social service provider ID of the identified object.</param>
		/// <param name="socialProviderId">The ID of the social service provider.</param>
		public SocialId(string id, string socialProviderId)
		{
			// validate arguments
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");
			if (string.IsNullOrEmpty(socialProviderId))
				throw new ArgumentNullException("socialProviderId");

			// set values
			this.id = id;
			this.socialProviderId = socialProviderId;
		}
		#endregion
		#region Properties
		/// <summary>
		/// The social service provider ID of the identified object.
		/// </summary>
		public string Id
		{
			get { return id; }
		}
		/// <summary>
		/// The ID of the social service provider.
		/// </summary>
		public string SocialProviderId
		{
			get { return socialProviderId; }
		}
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return id + "@" + socialProviderId;
		}
		#endregion
		#region Private Fields
		private readonly string id;
		private readonly string socialProviderId;
		#endregion
	}
}