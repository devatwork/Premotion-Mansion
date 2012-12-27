using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// Base class for all tokenizers.
	/// </summary>
	public abstract class BaseTokenizer
	{
		#region Nested type: BaseTokenizerDescriptor
		/// <summary>
		/// Base class for all the tokenizer descriptors.
		/// </summary>
		protected abstract class BaseTokenizerDescriptor : BaseAnalysisDescriptor<BaseTokenizer>
		{
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a tokenizer.
		/// </summary>
		/// <param name="type">The type of tokenizer.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
		protected BaseTokenizer(string type)
		{
			// validate arguments
			if (string.IsNullOrEmpty(type))
				throw new ArgumentNullException("type");

			// set the type
			Type = type;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the type of tokenizer.
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; private set; }
		#endregion
	}
}