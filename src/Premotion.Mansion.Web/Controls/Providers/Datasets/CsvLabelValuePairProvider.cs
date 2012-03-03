using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Controls.Providers.Datasets
{
	/// <summary>
	/// Provides a <see cref="Dataset"/> containing name/value pairs of all <see cref="ITypeDefinition"/>s.
	/// </summary>
	public class CsvLabelValuePairProvider : DatasetProvider
	{
		#region Nested type: CsvLabelValuePairProviderFactoryTag
		/// <summary>
		/// Creates <see cref="TypeDefinitionDatasetProvider"/>s.
		/// </summary>
		[Named(Constants.DataProviderTagNamespaceUri, "csvLabelValuePairProvider")]
		public class CsvLabelValuePairProviderFactoryTag : DatasetProviderFactoryTag<DatasetProvider>
		{
			#region Overrides of DataProviderFactoryTag
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override DatasetProvider Create(MansionWebContext context)
			{
				return new CsvLabelValuePairProvider(GetRequiredAttribute<string>(context, "csv"));
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a dataset provider.
		/// </summary>
		private CsvLabelValuePairProvider(string csv)
		{
			foreach (var labelValuePairParts in csv.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).Select(labelValuePair => labelValuePair.Split(new[] {','})))
			{
				// validate
				if (labelValuePairParts.Length != 2)
					throw new InvalidOperationException(string.Format("CSV '{0}' contains invalid value '{1}'", csv, string.Join(",", labelValuePairParts)));

				// create a row
				dataset.AddRow(new PropertyBag
				               {
				               	{"value", labelValuePairParts[0].Trim()},
				               	{"label", labelValuePairParts[1].Trim()}
				               });
			}
		}
		#endregion
		#region Overrides of DataProvider<Dataset>
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the retrieve data.</returns>
		protected override Dataset DoRetrieve(MansionContext context)
		{
			return dataset;
		}
		#endregion
		#region Private Fields
		private readonly Dataset dataset = new Dataset();
		#endregion
	}
}