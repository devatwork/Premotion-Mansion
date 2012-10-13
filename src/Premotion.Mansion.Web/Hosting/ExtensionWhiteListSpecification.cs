using System;
using System.Linq;
using Premotion.Mansion.Core.Patterns.Specifications;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Checks for request matching the specified extension.
	/// </summary>
	public class ExtensionWhiteListSpecification : ISpecification<IMansionWebContext, bool>
	{
		#region Constructors
		/// <summary>
		/// Constructs the extension whitelist specification.
		/// </summary>
		/// <param name="allowedExtensions">The extensions allowed.</param>
		/// <param name="allowNoExtension">Flag indicating whether request without an extension are allowed.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="allowedExtensions"/> is null.</exception>
		public ExtensionWhiteListSpecification(string[] allowedExtensions, bool allowNoExtension = false)
		{
			// validate arguments
			if (allowedExtensions == null)
				throw new ArgumentNullException("allowedExtensions");

			// set the allowed extensions
			this.allowedExtensions = allowedExtensions;
			this.allowNoExtension = allowNoExtension;
		}
		#endregion
		#region Implementation of ISpecification<in IMansionWebContext,out bool>
		/// <summary>
		/// Checks whether the given <paramref name="subject"/> satisfies this specification.
		/// </summary>
		/// <param name="subject">The subject which to check against this specification.</param>
		/// <returns>Returns the result of this check.</returns>
		public bool IsSatisfiedBy(IMansionWebContext subject)
		{
			// validate arguments
			if (subject == null)
				throw new ArgumentNullException("subject");

			// get the dot index
			var dotIndex = subject.Request.Url.Path.LastIndexOf('.');
			if (dotIndex == -1)
				return allowNoExtension;

			// get the relative url of the current reqest
			var extension = subject.Request.Url.Path.Substring(dotIndex);

			// check if the requests starts with the given prefix
			return allowedExtensions.Any(candidate => extension.Equals(candidate, StringComparison.OrdinalIgnoreCase));
		}
		#endregion
		#region Private Fields
		private readonly bool allowNoExtension;
		private readonly string[] allowedExtensions;
		#endregion
	}
}