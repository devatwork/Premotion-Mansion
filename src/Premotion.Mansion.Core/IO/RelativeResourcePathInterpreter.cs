using System;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Implements <see cref="ResourcePathInterpreter"/> for <see cref="RelativeResourcePath"/>.
	/// </summary>
	public class RelativeResourcePathInterpreter : ResourcePathInterpreter
	{
		#region Implementation of ICandidate<IPropertyBag>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected override VoteResult DoVote(IMansionContext context, IPropertyBag subject)
		{
			// check if a path is specified
			string path;
			return subject.TryGet(context, "path", out path) ? VoteResult.MediumInterest : VoteResult.Refrain;
		}
		#endregion
		#region Implementation of IInterpreter<in IPropertyBag,out IResourcePath>
		/// <summary>
		/// Interprets the input..
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		protected override IResourcePath DoInterpret(IMansionContext context, IPropertyBag input)
		{
			// get the path
			var path = input.Get<string>(context, "path");
			if (string.IsNullOrEmpty(path))
				throw new InvalidOperationException("The path should not be empty");
			var overridable = input.Get(context, "overridable", true);

			// return the resource path
			return new RelativeResourcePath(path, overridable);
		}
		#endregion
	}
}