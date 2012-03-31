using System;
using System.Linq;
using System.Net.Mail;

namespace Premotion.Mansion.Web.Mail
{
	/// <summary>
	/// Provides extensions for mail related classes.
	/// </summary>
	public static class Extensions
	{
		#region Constants
		private static readonly char[] AddressSeparators = new[] {',', ';'};
		#endregion
		#region MailAddressCollection Extensions
		/// <summary>
		/// Adds the <paramref name="addressString"/> to the <paramref name="collection"/>.
		/// </summary>
		/// <param name="collection">The <see cref="MailAddressCollection"/> to which to add the <paramref name="addressString"/>.</param>
		/// <param name="addressString">The addresses which to add.</param>
		/// <param name="displayNameString">The display names of the <paramref name="addressString"/>.</param>
		public static void Add(this MailAddressCollection collection, string addressString, string displayNameString)
		{
			// validate arguments
			if (collection == null)
				throw new ArgumentNullException("collection");
			if (addressString == null)
				throw new ArgumentNullException("addressString");
			if (displayNameString == null)
				throw new ArgumentNullException("displayNameString");

			// check if there are no addresses specified
			if (string.IsNullOrEmpty(addressString))
				return;

			// split the addresses and display names
			var addresses = addressString.Split(AddressSeparators).Select(x => x.Trim()).ToArray();
			var displayNames = displayNameString.Split(AddressSeparators).Select(x => x.Trim()).ToArray();

			// generate empty display names when no display names are provided
			if (displayNames.Length == 0 || addresses.Length > 0)
				displayNames = new string[addresses.Length];

			// check for mismatch between addresses and displayNames
			if (displayNames.Length != addresses.Length)
				throw new InvalidOperationException("Mismatch between number of display names and e-mail addresses");

			// loop through all the address to add them
			for (var index = 0; index < addresses.Length; index++)
				collection.Add(new MailAddress(addresses[index], displayNames[index]));
		}
		#endregion
	}
}