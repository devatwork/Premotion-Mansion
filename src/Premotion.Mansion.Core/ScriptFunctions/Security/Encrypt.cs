using System;
using System.Text;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptFunctions.Security
{
	/// <summary>
	/// Encrypts a string.
	/// </summary>
	[ScriptFunction("Encrypt")]
	public class Encrypt : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="Encrypt"/> function.
		/// </summary>
		/// <param name="encryptionService">The <see cref="IEncryptionService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public Encrypt(IEncryptionService encryptionService)
		{
			// validate arguments
			if (encryptionService == null)
				throw new ArgumentNullException("encryptionService");

			// set values
			this.encryptionService = encryptionService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Encrypts the given <paramref name="content"/> using <paramref name="key"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The key used to encrypt the <paramref name="content"/>.</param>
		/// <param name="content">The content which to encrypt.</param>
		/// <returns>Returns the encrypted string.</returns>
		public string Evaluate(IMansionContext context, string key, string content)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(content))
				throw new ArgumentNullException("content");
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			// get the bytes
			var keyBytes = Encoding.UTF8.GetBytes(key);
			var contentBytes = Encoding.UTF8.GetBytes(content);

			// encrypt
			var encryptedBytes = encryptionService.Encrypt(context, keyBytes, contentBytes);

			// return the base64 representation of the bytes
			return Convert.ToBase64String(encryptedBytes);
		}
		#endregion
		#region Private Fields
		private readonly IEncryptionService encryptionService;
		#endregion
	}
}