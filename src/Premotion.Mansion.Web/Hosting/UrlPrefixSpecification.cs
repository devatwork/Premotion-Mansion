using System;
using Premotion.Mansion.Core.Patterns.Specifications;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Provides a specification which checkw whether the current request's URL is prefixed by a given prefix.
	/// </summary>
	public class UrlPrefixSpecification : ISpecification<IMansionWebContext, bool>
	{
		#region Constructors
		/// <summary>
		/// Constructs a URL prefix specification.
		/// </summary>
		/// <param name="prefix">The prefix a request URL must have in order to satisfy this specification.</param>
		public UrlPrefixSpecification(string prefix)
		{
			// validate arguments
			if (String.IsNullOrEmpty(prefix))
				throw new ArgumentNullException("prefix");

			// set value
			this.prefix = prefix;
		}
		#endregion
		#region Implementation of IRequestHandlerSpecification
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

			// get the relative url of the current reqest
			var relativeUrl = subject.HttpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2);

			// check if the requests starts with the given prefix
			return relativeUrl.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
		}
		#endregion
		#region Private Fields
		private readonly string prefix;
		#endregion
	}
}