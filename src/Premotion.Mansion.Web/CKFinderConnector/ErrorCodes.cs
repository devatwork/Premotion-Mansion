namespace Premotion.Mansion.Web.CKFinderConnector
{
	/// <summary>
	/// 
	/// </summary>
	public enum ErrorCodes
	{
		/// <summary>
		/// 
		/// </summary>
		None = 0,
		/// <summary>
		/// 
		/// </summary>
		CustomError = 1,
		/// <summary>
		/// 
		/// </summary>
		InvalidCommand = 10,
		/// <summary>
		/// 
		/// </summary>
		TypeNotSpecified = 11,
		/// <summary>
		/// 
		/// </summary>
		InvalidType = 12,
		/// <summary>
		/// 
		/// </summary>
		InvalidName = 102,
		/// <summary>
		/// 
		/// </summary>
		Unauthorized = 103,
		/// <summary>
		/// 
		/// </summary>
		AccessDenied = 104,
		/// <summary>
		/// 
		/// </summary>
		InvalidExtension = 105,
		/// <summary>
		/// 
		/// </summary>
		InvalidRequest = 109,
		/// <summary>
		/// 
		/// </summary>
		Unknown = 110,
		/// <summary>
		/// 
		/// </summary>
		AlreadyExist = 115,
		/// <summary>
		/// 
		/// </summary>
		FolderNotFound = 116,
		/// <summary>
		/// 
		/// </summary>
		FileNotFound = 117,
		/// <summary>
		/// 
		/// </summary>
		SourceAndTargetPathEqual = 118,
		/// <summary>
		/// 
		/// </summary>
		UploadedFileRenamed = 201,
		/// <summary>
		/// 
		/// </summary>
		UploadedInvalid = 202,
		/// <summary>
		/// 
		/// </summary>
		UploadedTooBig = 203,
		/// <summary>
		/// 
		/// </summary>
		UploadedCorrupt = 204,
		/// <summary>
		/// 
		/// </summary>
		UploadedNoTmpDir = 205,
		/// <summary>
		/// 
		/// </summary>
		UploadedWrongHtmlFile = 206,
		/// <summary>
		/// 
		/// </summary>
		UploadedInvalidNameRenamed = 207,
		/// <summary>
		/// 
		/// </summary>
		MoveFailed = 300,
		/// <summary>
		/// 
		/// </summary>
		CopyFailed = 301,
		/// <summary>
		/// 
		/// </summary>
		ConnectorDisabled = 500,
		/// <summary>
		/// 
		/// </summary>
		ThumbnailsDisabled = 501
	}
}