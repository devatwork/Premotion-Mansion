using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Web.Hosting.Security
{
	/// <summary>
	/// Represents a cross-site request forgery token.
	/// </summary>
	[Serializable]
	public class CsrfToken : IEquatable<CsrfToken>
	{
		#region Constructors
		/// <summary>
		/// Constructs this CSRF token with the specified <paramref name="signature"/>.
		/// </summary>
		/// <param name="signature">The signature bytes.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="signature"/> is null.</exception>
		public CsrfToken(byte[] signature)
		{
			// validate arugments
			if (signature == null)
				throw new ArgumentNullException("signature");

			// set the signature
			this.signature = signature;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates a <see cref="CsrfToken"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="encryptionService">The <see cref="IEncryptionService"/>.</param>
		/// <returns>Returns the <see cref="CsrfToken"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parmeters is null.</exception>
		public static CsrfToken Create(IMansionWebContext context, IEncryptionService encryptionService)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (encryptionService == null)
				throw new ArgumentNullException("encryptionService");

			// get the user agent string
			var tokenString = (context.Request.UserAgent ?? string.Empty);

			// calculate the hash
			var userAgentBytes = Encoding.UTF8.GetBytes(tokenString);
			var salt = new byte[16];
			RandomGenerator.GetBytes(salt);
			var hashBytes = encryptionService.Hash(context, salt, userAgentBytes);

			// create the token
			return new CsrfToken(hashBytes);
		}
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof (CsrfToken))
				return false;
			return Equals((CsrfToken) obj);
		}
		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			unchecked
			{
				return signature.GetHashCode();
			}
		}
		#endregion
		#region Implementation of IEquatable<CsrfToken>
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(CsrfToken other)
		{
			return other != null && signature.SequenceEqual(other.signature);
		}
		#endregion
		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(CsrfToken left, CsrfToken right)
		{
			return Equals(left, right);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(CsrfToken left, CsrfToken right)
		{
			return !Equals(left, right);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the signature bytes of this CSRF token.
		/// </summary>
		public byte[] Signature
		{
			get { return signature; }
		}
		#endregion
		#region Private Fields
		private static readonly RandomNumberGenerator RandomGenerator = new RNGCryptoServiceProvider();
		private readonly byte[] signature;
		#endregion
	}
}