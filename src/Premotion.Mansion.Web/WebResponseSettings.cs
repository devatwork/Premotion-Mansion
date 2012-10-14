using System;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Defines the cache settings for a <see cref="WebResponse"/>
	/// </summary>
	public class WebResponseSettings
	{
		#region Constructors
		/// <summary></summary>
		public WebResponseSettings()
		{
			LastModified = DateTime.Now;
			OutputCacheEnabled = true;
		}
		#endregion
		#region Clone Methods
		/// <summary>
		/// Clones this <see cref="WebResponseSettings"/>.
		/// </summary>
		/// <returns>Returns the cloned instance.</returns>
		public WebResponseSettings Clone()
		{
			return new WebResponseSettings
			       {
			       	ETag = ETag,
			       	Expires = Expires,
			       	LastModified = LastModified,
			       	OutputCacheEnabled = OutputCacheEnabled
			       };
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the last modified date and time at which the cached information was creaded.
		/// </summary>
		public DateTime LastModified { get; set; }
		/// <summary>
		/// Gets or sets the absolute date and time at which cached information expires in the cache.
		/// </summary>
		public DateTime? Expires { get; set; }
		/// <summary>
		/// Gets/Set a flag indicating whether the output cache of this response is disabled.
		/// </summary>
		public bool OutputCacheEnabled { get; set; }
		/// <summary>
		/// Gets or sets the ETag of the current <see cref="WebResponse"/>.
		/// </summary>
		public string ETag { get; set; }
		#endregion
	}
}