using System;
using System.Text;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Rendering
{
	/// <summary>
	/// Caches the response in a template which can be personalized.
	/// </summary>
	[Named(Constants.NamespaceUri, "responseTemplate")]
	public class ResponseTemplateTag : ScriptTag
	{
		#region Nested type: ResponseOutputPipe
		/// <summary>
		/// Implements a special <see cref="StringOutputPipe"/>.
		/// </summary>
		public class ResponseOutputPipe : StringOutputPipe, IDependableCachedObject
		{
			#region Constructors
			/// <summary>
			/// Constructs a <see cref="StringOutputPipe"/>.
			/// </summary>
			/// <param name="buffer">The buffer to which to write.</param>
			public ResponseOutputPipe(StringBuilder buffer) : base(buffer)
			{
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets/Sets a flag indicating whether this response can be cached or not.
			/// </summary>
			public bool ResponseCacheEnabled { get; set; }
			#endregion
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Caches the response in a template which can be personalized.
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the parameters
			var cacheKey = cacheKeyPrefix + GetRequiredAttribute<string>(context, "cacheKey");

			// get the services
			var cacheService = context.Nucleus.Get<ICachingService>(context);

			// load the expression
			var expression = cacheService.GetOrAdd(
				context,
				(StringCacheKey) cacheKey,
				() =>
				{
					// get the response template
					var responseTemplateBuffer = new StringBuilder();
					bool isResponseCacheEnabled;
					using (var pipe = new ResponseOutputPipe(responseTemplateBuffer)
					                  {
					                  	ResponseCacheEnabled = GetAttribute(context, "enabled", true)
					                  })
					using (context.OutputPipeStack.Push(pipe))
					{
						ExecuteChildTags(context);
						isResponseCacheEnabled = pipe.ResponseCacheEnabled;
					}

					// turn it into an expression
					var expressionService = context.Nucleus.Get<IExpressionScriptService>(context);
					var responseTemplateExpression = expressionService.Parse(context, new LiteralResource(responseTemplateBuffer.ToString()));

					// check if the object can be cached
					return new CachedPhrase(responseTemplateExpression)
					       {
					       	IsCachable = isResponseCacheEnabled
					       };
				}
				);

			// evaluate the expression and write the result to the output
			context.OutputPipe.Writer.Write(expression.Execute<string>(context));
		}
		#endregion
		#region Private Fields
		/// <summary>
		/// This prefix uniquely identifies this response template tag. different tags with the same cacheKey will yield different results.
		/// </summary>
		private readonly string cacheKeyPrefix = "ResponseTemplate" + "_" + Guid.NewGuid() + "_";
		#endregion
	}
}