using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Represents the definition of an index.
	/// </summary>
	public class IndexDefinition
	{
		#region Constructors
		/// <summary>
		/// Constructs an index definition.
		/// </summary>
		/// <param name="name">The name of the index.</param>
		public IndexDefinition(string name)
		{
			// make sure the index name is valid
			name.ValidateAsIndexName();

			// set values
			Name = name.ToLower();
			Settings = new IndexSettings();
			Mappings = new Dictionary<string, TypeMapping>(StringComparer.OrdinalIgnoreCase);
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds the given <paramref name="mapping"/> to <see cref="Mappings"/>.
		/// </summary>
		/// <param name="mapping">The <see cref="TypeMapping"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Add(TypeMapping mapping)
		{
			// validate arguments
			if (mapping == null)
				throw new ArgumentNullException("mapping");

			// add the mapping
			Mappings.Add(mapping.Name, mapping);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this index.
		/// </summary>
		[JsonIgnore]
		public string Name { get; private set; }
		/// <summary>
		/// Gets the <see cref="IndexSettings"/> of this index.
		/// </summary>
		[JsonProperty("settings")]
		public IndexSettings Settings { get; private set; }
		/// <summary>
		/// Gets the <see cref="TypeMapping"/>s of this index.
		/// </summary>
		[JsonProperty("mappings")]
		public IDictionary<string, TypeMapping> Mappings { get; private set; }
		#endregion
	}
}