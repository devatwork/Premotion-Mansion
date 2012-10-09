using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Implementation of the <see cref="IEncryptionService"/> using standard .NET classes.
	/// 
	/// Uses <see cref="Rijndael"/> and <see cref="SHA512Managed"/>.
	/// </summary>
	public class DotNetCryptoEncryptionService : IEncryptionService
	{
		#region Constants
		private const string ApplicationWideSaltConfigurationKey = "APPLICATION_SECURITY_SALT";
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this service.
		/// </summary>
		/// <param name="applicationWideSalt">The application wide salt.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="applicationWideSalt"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="applicationWideSalt"/></exception>
		public DotNetCryptoEncryptionService(byte[] applicationWideSalt)
		{
			// validate arguments
			if (applicationWideSalt == null)
				throw new ArgumentNullException("applicationWideSalt");
			if (applicationWideSalt.Length < 16)
				throw new ArgumentOutOfRangeException("applicationWideSalt", "The application-wide salt should at least be 16 bytes long");

			// set values
			this.applicationWideSalt = applicationWideSalt;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates an instance of the <see cref="DotNetCryptoEncryptionService"/>.
		/// </summary>
		/// <returns></returns>
		public static DotNetCryptoEncryptionService Create()
		{
			// get the application-wide salt
			var saltString = ConfigurationManager.AppSettings[ApplicationWideSaltConfigurationKey];
			if (string.IsNullOrEmpty(saltString))
				throw new InvalidOperationException("The application-wide salt was not found in the configuration, please check application configuration key: " + ApplicationWideSaltConfigurationKey);

			// get the bytes
			var salt = Encoding.UTF8.GetBytes(saltString);

			// create the service
			return new DotNetCryptoEncryptionService(salt);
		}
		#endregion
		#region Implementation of IEncryptionService
		/// <summary>
		/// Encrypts the given <paramref name="content"/> with the given <paramref name="key"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The key bytes used to encrypt the <paramref name="content"/>.</param>
		/// <param name="content">The content bytes which to encrypt.</param>
		/// <returns>Returns the encrypted <paramref name="content"/> bytes.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public byte[] Encrypt(IMansionContext context, byte[] key, byte[] content)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (key == null)
				throw new ArgumentNullException("key");
			if (content == null)
				throw new ArgumentNullException("content");

			// create the algorithm
			var algorithm = CreateSymmetricAlgorithm(key);

			// create a buffer
			using (var memoryStream = new MemoryStream())
			{
				// create the crypto stream
				using (var cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
				{
					// write the content bytes to the crypto stream
					cryptoStream.Write(content, 0, content.Length);
				}

				// return the bytes
				return memoryStream.ToArray();
			}
		}
		/// <summary>
		/// Decrypts the given <paramref name="content"/> with the given <paramref name="key"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The key bytes used to decrypt the <paramref name="content"/>.</param>
		/// <param name="content">The content bytes which to decrypt.</param>
		/// <returns>Returns the decrypted <paramref name="content"/> bytes.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public byte[] Decrypt(IMansionContext context, byte[] key, byte[] content)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (key == null)
				throw new ArgumentNullException("key");
			if (content == null)
				throw new ArgumentNullException("content");

			// create the algorithm
			var algorithm = CreateSymmetricAlgorithm(key);

			// create a buffer
			using (var memoryStream = new MemoryStream())
			{
				// create the crypto stream
				using (var cryptoStream = new CryptoStream(memoryStream, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
				{
					// write the content bytes to the crypto stream
					cryptoStream.Write(content, 0, content.Length);
				}

				// return the bytes
				return memoryStream.ToArray();
			}
		}
		/// <summary>
		/// Hashes the given <paramref name="content"/> using the specified <paramref name="salt"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="salt">The salt bytes.</param>
		/// <param name="content">The content bytes.</param>
		/// <returns>Returns the computed hash.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public byte[] Hash(IMansionContext context, byte[] salt, byte[] content)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (salt == null)
				throw new ArgumentNullException("salt");
			if (content == null)
				throw new ArgumentNullException("content");

			// create a new buffer to store the salted input
			var saltedContent = new byte[applicationWideSalt.Length + salt.Length + content.Length];
			applicationWideSalt.CopyTo(saltedContent, 0);
			salt.CopyTo(saltedContent, applicationWideSalt.Length);
			content.CopyTo(saltedContent, applicationWideSalt.Length + salt.Length);

			// create the hashing algoritm
			using (var hasher = new SHA512Managed())
			{
				// compute and return the hash
				return hasher.ComputeHash(saltedContent);
			}
		}
		/// <summary>
		/// Create the <see cref="SymmetricAlgorithm"/>.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns the created <see cref="SymmetricAlgorithm"/>.</returns>
		private SymmetricAlgorithm CreateSymmetricAlgorithm(byte[] key)
		{
			// create the Rijndael class
			var algorithm = Rijndael.Create();

			// create the password derive bytes in order to set the key and initialization vector (IV)
			using (var pdb = new Rfc2898DeriveBytes(key, applicationWideSalt, 1000))
			{
				algorithm.Key = pdb.GetBytes(algorithm.KeySize/8);
				algorithm.IV = pdb.GetBytes(algorithm.BlockSize/8);
			}

			// return the created algoritm;
			return algorithm;
		}
		#endregion
		#region Private Fields
		private readonly byte[] applicationWideSalt;
		#endregion
	}
}