using System;

namespace Premotion.Mansion.Core.Data.Listeners
{
	/// <summary>
	/// Implements the base class for <see cref="RecordListener"/>s.
	/// </summary>
	public abstract class RecordListener
	{
		#region Implementation of INodeListener
		/// <summary>
		/// This method is called just before a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The new properties of the node.</param>
		public void BeforeCreate(IMansionContext context, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			DoBeforeCreate(context, properties);
		}
		/// <summary>
		/// This method is called just after a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The properties from which the <paramref name="record"/> was constructed.</param>
		public void AfterCreate(IMansionContext context, Record record, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			DoAfterCreate(context, record, properties);
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The updated properties of the node.</param>
		public void BeforeUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			DoBeforeUpdate(context, record, properties);
		}
		/// <summary>
		/// This method is called just after a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The properties which were set to the updated <paramref name="record"/>.</param>
		public void AfterUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			DoAfterUpdate(context, record, properties);
		}
		/// <summary>
		/// This method is called just before a node is deleted from the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		public void BeforeDelete(IMansionContext context, Record record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");

			// invoke template method
			DoBeforeDelete(context, record);
		}
		/// <summary>
		/// This method is called when an property which is not on the node is accessed. Useful for lazy loading properties.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="propertyName">The name of the property being retrieved.</param>
		/// <param name="value">The missing value</param>
		public bool TryResolveMissingProperty(IMansionContext context, Record record, string propertyName, out object value)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");
			if (propertyName == null)
				throw new ArgumentNullException("propertyName");

			// invoke template method
			return DoTryResolveMissingProperty(context, record, propertyName, out value);
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// This method is called just before a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The new properties of the node.</param>
		protected virtual void DoBeforeCreate(IMansionContext context, IPropertyBag properties)
		{
		}
		/// <summary>
		/// This method is called just after a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The properties from which the <paramref name="record"/> was constructed.</param>
		protected virtual void DoAfterCreate(IMansionContext context, Record record, IPropertyBag properties)
		{
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The updated properties of the node.</param>
		protected virtual void DoBeforeUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
		}
		/// <summary>
		/// This method is called just after a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The properties which were set to the updated <paramref name="record"/>.</param>
		protected virtual void DoAfterUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
		}
		/// <summary>
		/// This method is called just before a node is deleted from the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		protected virtual void DoBeforeDelete(IMansionContext context, Record record)
		{
		}
		/// <summary>
		/// This method is called when an property which is not on the node is accessed. Useful for lazy loading properties.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="propertyName">The name of the property being retrieved.</param>
		/// <param name="value">The missing value</param>
		protected virtual bool DoTryResolveMissingProperty(IMansionContext context, Record record, string propertyName, out object value)
		{
			value = null;
			return false;
		}
		#endregion
	}
}