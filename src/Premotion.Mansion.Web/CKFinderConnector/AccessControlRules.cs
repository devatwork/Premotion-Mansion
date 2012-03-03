using System;

namespace Premotion.Mansion.Web.CKFinderConnector
{
	/// <summary>
	/// Enumerates the access control rules.
	/// </summary>
	[Flags]
	public enum AccessControlRules
	{
		/// <summary></summary>
		FolderView = 1,
		/// <summary></summary>
		FolderCreate = 2,
		/// <summary></summary>
		FolderRename = 4,
		/// <summary></summary>
		FolderDelete = 8,
		/// <summary></summary>
		FileView = 16,
		/// <summary></summary>
		FileUpload = 32,
		/// <summary></summary>
		FileRename = 64,
		/// <summary></summary>
		FileDelete = 128
	}
}