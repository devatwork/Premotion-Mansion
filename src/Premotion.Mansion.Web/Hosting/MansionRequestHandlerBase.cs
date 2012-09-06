using System;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Prioritized;
using Premotion.Mansion.Core.Patterns.Specifications;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Base class for all Mansion request handlers.
	/// </summary>
	[Exported(typeof (MansionRequestHandlerBase))]
	public abstract class MansionRequestHandlerBase : IPrioritized, ISpecification<IMansionWebContext, bool>
	{
		#region Nested type: AlwaysSatisfiedSpecification
		/// <summary>
		/// Each and every subject always satisfies this specification.
		/// </summary>
		protected class AlwaysSatisfiedSpecification : IRequestHandlerSpecification
		{
			#region Implementation of IRequestHandlerSpecification
			/// <summary>
			/// Checks whether the given <paramref name="subject"/> satisfies this specification.
			/// </summary>
			/// <param name="subject">The subject which to check against this specification.</param>
			/// <returns>Returns the result of this check.</returns>
			public bool IsSatisfiedBy(IMansionWebContext subject)
			{
				return true;
			}
			#endregion
			#region Singleton
			/// <summary>
			/// Gets the <see cref="AlwaysSatisfiedSpecification"/> instance.
			/// </summary>
			public static readonly AlwaysSatisfiedSpecification Instance = new AlwaysSatisfiedSpecification();
			/// <summary>
			/// Part of singleton implementation.
			/// </summary>
			private AlwaysSatisfiedSpecification()
			{
			}
			#endregion
		}
		#endregion
		#region Nested type: UrlPrefixSpeficiation
		/// <summary>
		/// Provides a specification which checkw whether the current request's URL is prefixed by a given prefix.
		/// </summary>
		protected class UrlPrefixSpeficiation : IRequestHandlerSpecification
		{
			#region Constructors
			/// <summary>
			/// Constructs a URL prefix specification.
			/// </summary>
			/// <param name="prefix">The prefix a request URL must have in order to satisfy this specification.</param>
			public UrlPrefixSpeficiation(string prefix)
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
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="MansionRequestHandlerBase"/> with the given <paramref name="priority"/>.
		/// </summary>
		/// <param name="priority">The relative priority of this object. The higher the priority, earlier this object is executed.</param>
		/// <param name="specification">The specification which checks wether the current request can be handled by this handler.</param>
		protected MansionRequestHandlerBase(int priority, IRequestHandlerSpecification specification)
		{
			// validate arguments
			if (specification == null)
				throw new ArgumentNullException("specification");

			// set values
			this.priority = priority;
			this.specification = specification;
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		public void Execute(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// invoke template method
			DoExecute(context);
		}
		#endregion
		#region Implementation of IPrioritized
		/// <summary>
		/// Gets the relative priority of this object. The higher the priority, earlier this object is executed.
		/// </summary>
		public int Priority
		{
			get { return priority; }
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

			// invoke template method
			return specification.IsSatisfiedBy(subject);
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		protected abstract void DoExecute(IMansionWebContext context);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the minimal required <see cref="RequiresSessionState"/> for this handler.
		/// </summary>
		public virtual RequiresSessionState MinimalStateDemand
		{
			get { return RequiresSessionState.No; }
		}
		#endregion
		#region Private Fields
		private readonly int priority;
		private readonly IRequestHandlerSpecification specification;
		#endregion
	}
}