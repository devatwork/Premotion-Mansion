using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Dependencies;
using Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;
using Premotion.Mansion.Core.Patterns.Tokenizing;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements <see cref="IExpressionScriptService"/>.
	/// </summary>
	public class ExpressionScriptService : ManagedLifecycleService, IServiceWithDependencies, IExpressionScriptService
	{
		#region Implementation of IScriptingService<out IExpressionScript>
		/// <summary>
		/// Parses a script from the specified resource.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="resource">The resource wcich to parse as script.</param>
		/// <returns>Returns the parsed script.</returns>
		/// <exception cref="ParseScriptException">Thrown when an exception occurres while parsing the script.</exception>
		public IExpressionScript Parse(MansionContext context, IResource resource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// get the cache service
			var cacheKey = ResourceCacheKey.Create(resource);
			var cacheService = context.Nucleus.Get<ICachingService>(context);

			// open the script
			return cacheService.GetOrAdd(
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
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="rawPhrase">The phrase which to parse.</param>
		/// <returns>Returns the parsed script.</returns>
		/// <exception cref="ParseScriptException">Thrown when an exception occurres while parsing the script.</exception>
		private Phrase ParsePhrase(MansionContext context, string rawPhrase)
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
				var interpreter = Election<MansionContext, ExpressionPartInterpreter, string>.Elect(context, interpreters, token);

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
		#region Implementation of IStartableService
		/// <summary>
		/// Invoked just before this service is used for the first time.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		protected override void DoStart(INucleusAwareContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the naming and object factory services
			var namingService = context.Nucleus.Get<ITypeDirectoryService>(context);
			var objectFactoryService = context.Nucleus.Get<IObjectFactoryService>(context);

			// look up all the types implementing 
			interpreters.AddRange(objectFactoryService.Create<ExpressionPartInterpreter>(namingService.Lookup<ExpressionPartInterpreter>()));
		}
		#endregion
		#region Implementation of IServiceWithDependencies
		/// <summary>
		/// Gets the <see cref="DependencyModel"/> of this service.
		/// </summary>
		public DependencyModel Dependencies
		{
			get { return dependencies; }
		}
		#endregion
		#region Private Fields
		private static readonly DependencyModel dependencies = new DependencyModel().Add<ITypeDirectoryService>().Add<IObjectFactoryService>().Add<ICachingService>();
		private readonly List<ExpressionPartInterpreter> interpreters = new List<ExpressionPartInterpreter>();
		private readonly ITokenizer<string, string> tokenizer = new PhraseScriptTokenizer();
		#endregion
	}
}