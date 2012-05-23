namespace Premotion.Mansion.Web.Social.Facebook
{
	/// <summary>
	/// Represents an Facebook error.
	/// </summary>
	public class Error
	{
		#region Properties
		/// <summary>
		/// Gets the error message.
		/// </summary>
		public string Message { get; set; }
		/// <summary>
		/// Gets the error type.
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// Gets the error code.
		/// </summary>
		public int Code { get; set; }
		#endregion
	}
}