using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Premotion.Mansion.Core.IO.Windows
{
	/// <summary>
	/// Contains windows specific extensions.
	/// </summary>
	public static class Extensions
	{
		#region Extensions of DirectoryInfo
		/// <summary>
		/// Checks whether <paramref name="info"/> is writable by the current user.
		/// </summary>
		/// <param name="info">The <see cref="DirectoryInfo"/>.</param>
		/// <returns>Returns true whent the directory is writeable, otherwise false.</returns>
		public static bool IsWriteable(this DirectoryInfo info)
		{
			// validate arguments
			if (info == null)
				throw new ArgumentNullException("info");

			// check if the entry exists
			if (!info.Exists)
				return false;

			// Get the access rules of the specified files (user groups and user names that have access to the file)
			var rules = info.GetAccessControl().GetAccessRules(true, true, typeof (SecurityIdentifier));

			// Get the identity of the current user and the groups that the user is in.
			var identity = WindowsIdentity.GetCurrent();
			if (identity == null)
				throw new InvalidOperationException("Could not get windows identity");
			var user = identity.User;
			if (user == null)
				throw new InvalidOperationException("Could not get user from identity");
			var sidCurrentUser = user.Value;
			var groups = identity.Groups;
			if (groups == null)
				throw new InvalidOperationException("Could not get groups from identity");

			// Check if writing to the file is explicitly denied for this user or a group the user is in.
			if (rules.OfType<FileSystemAccessRule>().Any(r => (groups.Contains(r.IdentityReference) || r.IdentityReference.Value == sidCurrentUser) && r.AccessControlType == AccessControlType.Deny && (r.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData))
				return false;

			// Check if writing is allowed
			return rules.OfType<FileSystemAccessRule>().Any(r => (groups.Contains(r.IdentityReference) || r.IdentityReference.Value == sidCurrentUser) && r.AccessControlType == AccessControlType.Allow && (r.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData);
		}
		#endregion
	}
}