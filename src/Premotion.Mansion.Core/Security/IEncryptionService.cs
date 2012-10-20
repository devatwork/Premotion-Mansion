using System;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Provides encryption methods for both for symmetrical and asymmetrical encryption.
	/// 
	/// This services use industry standard encryption but can be tailed on application level for specific needs.
	/// </summary>
	public interface IEncryptionService
	{
		#region Symmetrical Encryption Methods
		/// <summary>
		/// Encrypts the given <paramref name="content"/> with the given <paramref name="key"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The key bytes used to encrypt the <paramref name="content"/>.</param>
		/// <param name="content">The content bytes which to encrypt.</param>
		/// <returns>Returns the encrypted <paramref name="content"/> bytes.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		byte[] Encrypt(IMansionContext context, byte[] key, byte[] content);
		/// <summary>
		/// Decrypts the given <paramref name="content"/> with the given <paramref name="key"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The key bytes used to decrypt the <paramref name="content"/>.</param>
		/// <param name="content">The content bytes which to decrypt.</param>
		/// <returns>Returns the decrypted <paramref name="content"/> bytes.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		byte[] Decrypt(IMansionContext context, byte[] key, byte[] content);
		#endregion
		#region Asymmetrical Encryption Methods
		/// <summary>
		/// Hashes the given <paramref name="content"/> using the specified <paramref name="salt"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="salt">The salt bytes.</param>
		/// <param name="content">The content bytes.</param>
		/// <returns>Returns the computed hash.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		byte[] Hash(IMansionContext context, byte[] salt, byte[] content);
		#endregion
	}
}