using System;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.Controls
{
	/// <summary>
	/// Represents the meta data of an control.
	/// </summary>
	public class ControlDefinition
	{
		#region Constructors
		/// <summary>
		/// Constructs a control definition.
		/// </summary>
		/// <param name="id">The ID of the control.</param>
		/// <param name="properties">The properties of an control.</param>
		public ControlDefinition(string id, IPropertyBag properties)
		{
			// validate arguments
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// set values
			Id = id;
			Properties = properties;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of an <see cref="Control"/>.
		/// </summary>
		public string Id { get; private set; }
		/// <summary>
		/// Gets the <see cref="IPropertyBag"/> of this control.
		/// </summary>
		public IPropertyBag Properties { get; private set; }
		#endregion
	}
}