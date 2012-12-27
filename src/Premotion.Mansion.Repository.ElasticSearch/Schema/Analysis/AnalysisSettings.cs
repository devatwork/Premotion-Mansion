using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// Contains the analysis settings for a given index.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class AnalysisSettings
	{
		#region Add Methods
		/// <summary>
		/// Adds the given <paramref name="components"/> to the analysis settings.
		/// </summary>
		/// <param name="components">The <see cref="AnalysisComponent{TComponent}"/> which to add or replace.</param>
		/// <returns>Returns the <see cref="AnalysisSettings"/> for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="components"/> is null.</exception>
		public AnalysisSettings Add(IEnumerable<AnalysisComponent<BaseAnalyzer>> components)
		{
			// validate arguments
			if (components == null)
				throw new ArgumentNullException("components");

			// loop over all the components
			foreach (var component in components)
			{
				// check if the component is already registered, in which case we will replace it
				if (analyzers.ContainsKey(component.RegisteredName))
					analyzers[component.RegisteredName] = component.Component;
				else
					analyzers.Add(component.RegisteredName, component.Component);
			}

			// return this for chaining
			return this;
		}
		/// <summary>
		/// Adds the given <paramref name="components"/> to the analysis settings.
		/// </summary>
		/// <param name="components">The <see cref="AnalysisComponent{TComponent}"/> which to add or replace.</param>
		/// <returns>Returns the <see cref="AnalysisSettings"/> for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="components"/> is null.</exception>
		public AnalysisSettings Add(IEnumerable<AnalysisComponent<BaseCharFilter>> components)
		{
			// validate arguments
			if (components == null)
				throw new ArgumentNullException("components");
			// loop over all the components
			foreach (var component in components)
			{
				// check if the component is already registered, in which case we will replace it
				if (analyzers.ContainsKey(component.RegisteredName))
					charFilters[component.RegisteredName] = component.Component;
				else
					charFilters.Add(component.RegisteredName, component.Component);
			}

			// return this for chaining
			return this;
		}
		/// <summary>
		/// Adds the given <paramref name="components"/> to the analysis settings.
		/// </summary>
		/// <param name="components">The <see cref="AnalysisComponent{TComponent}"/> which to add or replace.</param>
		/// <returns>Returns the <see cref="AnalysisSettings"/> for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="components"/> is null.</exception>
		public AnalysisSettings Add(IEnumerable<AnalysisComponent<BaseTokenFilter>> components)
		{
			// validate arguments
			if (components == null)
				throw new ArgumentNullException("components");

			// loop over all the components
			foreach (var component in components)
			{
				// check if the component is already registered, in which case we will replace it
				if (analyzers.ContainsKey(component.RegisteredName))
					tokenFilters[component.RegisteredName] = component.Component;
				else
					tokenFilters.Add(component.RegisteredName, component.Component);
			}

			// return this for chaining
			return this;
		}
		/// <summary>
		/// Adds the given <paramref name="components"/> to the analysis settings.
		/// </summary>
		/// <param name="components">The <see cref="AnalysisComponent{TComponent}"/> which to add or replace.</param>
		/// <returns>Returns the <see cref="AnalysisSettings"/> for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="components"/> is null.</exception>
		public AnalysisSettings Add(IEnumerable<AnalysisComponent<BaseTokenizer>> components)
		{
			// validate arguments
			if (components == null)
				throw new ArgumentNullException("components");

			// loop over all the components
			foreach (var component in components)
			{
				// check if the component is already registered, in which case we will replace it
				if (analyzers.ContainsKey(component.RegisteredName))
					tokenizers[component.RegisteredName] = component.Component;
				else
					tokenizers.Add(component.RegisteredName, component.Component);
			}

			// return this for chaining
			return this;
		}
		#endregion
		#region Private Fields
		[JsonProperty("analyzer")]
		private readonly IDictionary<string, BaseAnalyzer> analyzers = new Dictionary<string, BaseAnalyzer>(StringComparer.OrdinalIgnoreCase);
		[JsonProperty("char_filter")]
		private readonly IDictionary<string, BaseCharFilter> charFilters = new Dictionary<string, BaseCharFilter>(StringComparer.OrdinalIgnoreCase);
		[JsonProperty("filter")]
		private readonly IDictionary<string, BaseTokenFilter> tokenFilters = new Dictionary<string, BaseTokenFilter>(StringComparer.OrdinalIgnoreCase);
		[JsonProperty("tokenizer")]
		private readonly IDictionary<string, BaseTokenizer> tokenizers = new Dictionary<string, BaseTokenizer>(StringComparer.OrdinalIgnoreCase);
		#endregion
	}
}