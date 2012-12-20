using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Patterns.Tokenizing;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements <see cref="IExpressionScriptService"/>.
	/// </summary>
	public class ExpressionScriptService : IExpressionScriptService
	{
		#region Constructors
		/// <summary>
		/// Constructs the expression service.
		/// </summary>
		/// <param name="interpreters">The <see cref="IEnumerable{T}"/>.</param>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="interpreters"/> or <paramref name="cachingService"/> is null.</exception>
		public ExpressionScriptService(IEnumerable<ExpressionPartInterpreter> interpreters, ICachingService cachingService)
		{
			// validate arguments
			if (interpreters == null)
				throw new ArgumentNullException("interpreters");
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// set values
			this.interpreters = interpreters;
			this.cachingService = cachingService;
		}
		#endregion
		#region Implementation of IScriptingService<out IExpressionScript>
		/// <summary>
		/// Parses a script from the specified resource.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resource">The resource wcich to parse as script.</param>
		/// <returns>Returns the parsed script.</returns>
		/// <exception cref="ParseScriptException">Thrown when an exception occurres while parsing the script.</exception>
		public IExpressionScript Parse(IMansionContext context, IResource resource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// open the script
			var cacheKey = ResourceCacheKey.Create(resource);
			return cachingService.GetOrAdd(
				context,
				cacheKey,
				() =>
				{
					// get the phrase
					string rawPhrase;
					using (var resourceStream = resource.OpenForReading())
					using (var reader = resourceStream.Reader)
						rawPhrase = reader.ReadToEnd();

					// let someone else do the heavy lifting
					var phrase = ParsePhrase(context, rawPhrase);

					// create the cache object
					return new CachedPhrase(phrase);
				});
		}
		/// <summary>
		/// Parses a script from the specified resource.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="rawPhrase">The phrase which to parse.</param>
		/// <returns>Returns the parsed script.</returns>
		/// <exception cref="ParseScriptException">Thrown when an exception occurres while parsing the script.</exception>
		private Phrase ParsePhrase(IMansionContext context, string rawPhrase)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (rawPhrase == null)
				throw new ArgumentNullException("rawPhrase");

			// create a new phrase
			var phrase = new Phrase();

			// tokenize the raw phrase into smaller sections
			foreach (var token in tokenizer.Tokenize(context, rawPhrase))
			{
				// get the interpreter for this input
				var interpreter = Election<ExpressionPartInterpreter, string>.Elect(context, interpreters, token);

				// interpret the token
				phrase.Add(interpreter.Interpret(context, token));
			}

			// return the parsed phrase
			return phrase;
		}
		#endregion
		#region Implementation of IExpressionScriptServiceImplementation
		/// <summary>
		/// Gets the <see cref="ExpressionPartInterpreter"/>s registered with this expression script service.
		/// </summary>
		public IEnumerable<ExpressionPartInterpreter> Interpreters
		{
			get { return interpreters; }
		}
		#endregion
		#region Private Fields
		private readonly ICachingService cachingService;
		private readonly IEnumerable<ExpressionPartInterpreter> interpreters;
		private readonly ITokenizer<string, string> tokenizer = new PhraseScriptTokenizer();
		#endregion
	}
}