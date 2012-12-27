using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// Base class for all the token filters.
	/// </summary>
	public abstract class BaseTokenFilter
	{
		#region Nested type: BaseTokenFilterDescriptor
		/// <summary>
		/// Base class for all the token descriptors.
		/// </summary>
		protected abstract class BaseTokenFilterDescriptor : BaseAnalysisDescriptor<BaseTokenFilter>
		{
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a token filter.
		/// </summary>
		/// <param name="type">The type of token filter.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
		protected BaseTokenFilter(string type)
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
		/// Gets the type of token filter.
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; private set; }
		#endregion
	}
}