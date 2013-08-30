using System;
using System.Text;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Security;

namespace Premotion.Mansion.Core.ScriptFunctions.Security
{
	/// <summary>
	/// Decrypts a string.
	/// </summary>
	[ScriptFunction("Decrypt")]
	public class Decrypt : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="Encrypt"/> function.
		/// </summary>
		/// <param name="encryptionService">The <see cref="IEncryptionService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public Decrypt(IEncryptionService encryptionService)
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
		/// Decrypts the given <paramref name="content"/> using <paramref name="key"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The key used to decrypt the <paramref name="content"/>.</param>
		/// <param name="content">The content which to decrypt.</param>
		/// <returns>Returns the decrypted string.</returns>
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
			var contentBytes = Convert.FromBase64String(content);

			// decrypt
			var decryptedBytes = encryptionService.Decrypt(context, keyBytes, contentBytes);

			// convert back to string
			return Encoding.UTF8.GetString(decryptedBytes);
		}
		#endregion
		#region Private Fields
		private readonly IEncryptionService encryptionService;
		#endregion
	}
}