using System;
using System.Security.Cryptography;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Security
{
	/// <summary>
	/// Generates a random passsword.
	/// </summary>
	[ScriptFunction("GeneratePassword")]
	public class GeneratePassword : FunctionExpression
	{
		#region Constants
		/// <summary>
		/// Gets the allowed characters in the password.
		/// </summary>
		private const string AllowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Generates a random passsword.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the generated password.</returns>
		public string Evaluate(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			var chars = new char[8];
			var randomBytes = new byte[1];
			using (var rngCsp = new RNGCryptoServiceProvider())
			{
				for (var i = 0; i < 8; i++)
				{
					// get a random number within the allowed character range
					do
					{
						rngCsp.GetBytes(randomBytes);
					} while (randomBytes[0] >= 0 && randomBytes[0] < AllowedChars.Length);

					// get the character
					chars[i] = AllowedChars[randomBytes[0]];
				}
			}

			return new string(chars);
		}
		#endregion
	}
}