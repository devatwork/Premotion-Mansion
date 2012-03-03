using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Web.Controls.Providers.Datasets
{
	/// <summary>
	/// 	Implements <see cref="DatasetProvider" /> by getting the content of child elements. The local name of the child element will be used as the key in the resulting <see cref="IPropertyBag"/> and the concattenated text of the element as the value.
	/// </summary>
	public class ChildXmlDatasetProvider : DatasetProvider
	{
		#region Constructors
		/// <summary>
		/// Constructs this provider with the specified <paramref name="children" />.
		/// </summary>
		/// <param name="children">The <see cref="XElement" />s from which to get the values.</param>
		public ChildXmlDatasetProvider(IEnumerable<XElement> children)
		{
			// validate arguments
			if (children == null)
				throw new ArgumentNullException("children");

			// create the dataset
			dataset = new Dataset();

			// loop over all the children and get the content of its children
			foreach (var child in children)
			{
				var properties = new PropertyBag();
				foreach (var contentChild in child.Elements())
					properties.Set(contentChild.Name.LocalName, contentChild.Value);
				dataset.AddRow(properties);
			}
		}
		#endregion
		#region Implementation of DatasetProvider
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext" />.</param>
		/// <returns>Returns the retrieve data.</returns>
		protected override Dataset DoRetrieve(MansionContext context)
		{
			return dataset;
		}
		#endregion
		#region Private Fields
		private readonly Dataset dataset;
		#endregion
	}
}