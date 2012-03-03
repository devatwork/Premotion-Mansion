using System;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Contains the state of an <see cref="Form"/>.
	/// </summary>
	public class FormState
	{
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether the form is posted or not.
		/// </summary>
		public bool IsPostback { get; set; }
		/// <summary>
		/// Gets the current <see cref="Step"/> of the form.
		/// </summary>
		public Step CurrentStep { get; set; }
		/// <summary>
		/// Gets/Sets the next <see cref="Step"/> of the form.
		/// </summary>
		public Step NextStep { get; set; }
		/// <summary>
		/// Gets the properties of the field of this form.
		/// </summary>
		public IPropertyBag FieldProperties { get; set; }
		/// <summary>
		/// Gets the instance properties of this form.
		/// </summary>
		public IPropertyBag FormInstanceProperties { get; set; }
		/// <summary>
		/// Gets a flag indicating whether this form has a data source.
		/// </summary>
		public bool HasDataSource
		{
			get { return dataSource != null; }
		}
		/// <summary>
		/// Gets/Sets the <see cref="IPropertyBag"/> acting as the data source of this form.
		/// </summary>
		public IPropertyBag DataSource
		{
			get
			{
				if (dataSource == null)
					throw new InvalidOperationException("This form state does not have a data source");
				return dataSource;
			}
			set { dataSource = value; }
		}
		/// <summary>
		/// Gets/Sets the name of the action being executed by this form.
		/// </summary>
		public string CurrentAction { get; set; }
		#endregion
		#region Private Fields
		private IPropertyBag dataSource;
		#endregion
	}
}