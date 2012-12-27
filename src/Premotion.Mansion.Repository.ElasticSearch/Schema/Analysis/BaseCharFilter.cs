using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// Base class for all character filters.
	/// </summary>
	public abstract class BaseCharFilter
	{
		#region Nested type: BaseCharFilterDescriptor
		/// <summary>
		/// Base class for all the tokenizer descriptors.
		/// </summary>
		protected abstract class BaseCharFilterDescriptor : BaseAnalysisDescriptor<BaseCharFilter>
		{
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a character filter.
		/// </summary>
		/// <param name="type">The type of character filter.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
		protected BaseCharFilter(string type)
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
		/// Gets the type of character filter.
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; private set; }
		#endregion
	}
}