using System;
using System.Collections;
using System.Collections.Generic;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Provides a <see cref="IPropertyBagReader"/> around a <see cref="Dataset"/>.
	/// </summary>
	public class DatasetReader : DisposableBase, IPropertyBagReader
	{
		#region Constructors
		/// <summary>
		/// Constructs a reader from the given <paramref name="dataset"/>.
		/// </summary>
		/// <param name="dataset">The <see cref="Dataset"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public DatasetReader(Dataset dataset)
		{
			// validate arguments
			if (dataset == null)
				throw new ArgumentNullException("dataset");

			// set the value
			this.dataset = dataset;
		}
		#endregion
		#region IPropertyBagReader Members
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<IPropertyBag> GetEnumerator()
		{
			return dataset.Rows.GetEnumerator();
		}
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			// nothing to do here
		}
		#endregion
		#region Private Fields
		private readonly Dataset dataset;
		#endregion
	}
}