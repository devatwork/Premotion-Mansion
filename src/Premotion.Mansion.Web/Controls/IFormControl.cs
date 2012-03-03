namespace Premotion.Mansion.Web.Controls
{
	/// <summary>
	/// Marker interface for controls who use a form.
	/// </summary>
	public interface IFormControl : IControl
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="ControlDefinition"/> of this form control.
		/// </summary>
		ControlDefinition Definition { get; }
		#endregion
	}
}