using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// A filter that stems words using a Snowball-generated stemmer.
	/// </summary>
	public class SnowballTokenFilter : BaseTokenFilter
	{
		#region Nested type: SnowballTokenFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="SnowballTokenFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "snowballTokenFilter")]
		private class SnowballTokenFilterDescriptor : BaseTokenFilterDescriptor
		{
			#region Overrides of BaseTokenFilterDescriptor
			/// <summary>
			/// Constructs a <see cref="BaseTokenFilter"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the created <see cref="BaseTokenFilter"/>.</returns>
			protected override BaseTokenFilter DoCreate(IMansionContext context)
			{
				// get the language
				var language = Properties.Get<string>(context, "language");
				if (string.IsNullOrEmpty(language))
					throw new InvalidOperationException("Language is a required attribute on SnowballTokenFilter");

				return new SnowballTokenFilter(language);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this token filter
		/// </summary>
		/// <param name="language"><see cref="Language"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="language"/> is null or empty.</exception>
		public SnowballTokenFilter(string language) : base("snowball")
		{
			// validate arguments
			if (string.IsNullOrEmpty(language))
				throw new ArgumentNullException("language");

			// set the value
			Language = language;
		}
		#endregion
		#region Properties
		/// <summary>
		/// The language parameter controls the stemmer with the following available values: Armenian, Basque, Catalan, Danish, Dutch, English, Finnish, French, German, German2, Hungarian, Italian, Kp, Lovins, Norwegian, Porter, Portuguese, Romanian, Russian, Spanish, Swedish, Turkish.
		/// </summary>
		[JsonProperty("language")]
		public string Language { get; private set; }
		#endregion
	}
}