using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// Base class for all the analyzers.
	/// </summary>
	public abstract class BaseAnalyzer
	{
		#region Nested type: BaseAnalyzerDescriptor
		/// <summary>
		/// Base class for all the analyzer descriptors.
		/// </summary>
		protected abstract class BaseAnalyzerDescriptor : BaseAnalysisDescriptor<BaseAnalyzer>
		{
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs an analyzer.
		/// </summary>
		/// <param name="type">The type of analyzer.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
		protected BaseAnalyzer(string type)
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
		/// Gets the type of analyzer.
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; private set; }
		#endregion
	}
}