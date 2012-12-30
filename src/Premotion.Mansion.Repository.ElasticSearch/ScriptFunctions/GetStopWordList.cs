using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Repository.ElasticSearch.ScriptFunctions
{
	/// <summary>
	/// Returns a comma separated list of stop words.
	/// </summary>
	[ScriptFunction("GetStopWordList")]
	public class GetStopWordList : FunctionExpression
	{
		#region Constructors
		/// <summary></summary>
		/// <param name="resourceService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public GetStopWordList(IApplicationResourceService resourceService)
		{
			// validate arguments
			if (resourceService == null)
				throw new ArgumentNullException("resourceService");

			// set value
			this.resourceService = resourceService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Returns a comma separated list of stop words.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="language">The language name.</param>
		/// <returns>Returns a comma separated list of stop words.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public string Evaluate(IMansionContext context, string language)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(language))
				throw new ArgumentNullException("language");

			// create the path
			var pathString = string.Format("Assets/{0}/stopwords.txt", language);
			var path = resourceService.ParsePath(context, new PropertyBag
			                                              {
			                                              	{"path", pathString}
			                                              });

			// get the resource
			var resource = resourceService.GetSingle(context, path);

			// parse the file
			var words = new List<string>();
			using (var pipe = resource.OpenForReading())
			{
				// loop over each line
				string line;
				while ((line = pipe.Reader.ReadLine()) != null)
				{
					// strip the comment if any
					var commentIndex = line.IndexOfAny(new[] {'#', '|'});
					if (commentIndex > -1)
						line = line.Substring(0, commentIndex);

					// trim the line
					line = line.Trim();

					// if there is nothing on this line skip it
					if (line.Length == 0)
						continue;

					// add the word to the list
					words.Add(line);
				}
			}

			// format
			return string.Join(Environment.NewLine, words);
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService resourceService;
		#endregion
	}
}