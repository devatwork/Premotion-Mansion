using System.Net.Mail;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Web.Controls;
using Premotion.Mansion.Web.Controls.Forms;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents the context for mansion web applications.
	/// </summary>
	public interface IMansionWebContext : IMansionContext
	{
		#region Control Methods
		/// <summary>
		/// Generates a new control ID.
		/// </summary>
		/// <returns>Returns the generated control id.</returns>
		string GetNextControlId();
		/// <summary>
		/// Tries to get the top most control.
		/// </summary>
		/// <typeparam name="TControl">The type of <see cref="Control"/> which to get.</typeparam>
		/// <param name="control">The found control.</param>
		/// <returns>Returns true when the control was found, otherwise false.</returns>
		bool TryPeekControl<TControl>(out TControl control) where TControl : class, IControl;
		/// <summary>
		/// Tries to find a control in the <see cref="ControlStack"/>.
		/// </summary>
		/// <typeparam name="TControl">The type of <see cref="Control"/> which to find.</typeparam>
		/// <param name="control">The found control.</param>
		/// <returns>Returns true when the control was found, otherwise false.</returns>
		bool TryFindControl<TControl>(out TControl control) where TControl : class, IControl;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the top most <see cref="MailMessage"/> from the <see cref="MessageStack"/>.
		/// </summary>
		MailMessage Message { get; }
		/// <summary>
		/// Gets the message stack.
		/// </summary>
		IAutoPopStack<MailMessage> MessageStack { get; }
		/// <summary>
		/// Gets the <see cref="Control"/> stack.
		/// </summary>
		IAutoPopStack<IControl> ControlStack { get; }
		/// <summary>
		/// Gets the <see cref="Form"/> stack.
		/// </summary>
		IAutoPopStack<Form> FormStack { get; }
		/// <summary>
		/// Get the current <see cref="Form"/> from the <see cref="FormStack"/>.
		/// </summary>
		Form CurrentForm { get; }
		/// <summary>
		/// Gets the <see cref="WebRequest"/>.
		/// </summary>
		WebRequest Request { get; }
		/// <summary>
		/// Gets the <see cref="ISession"/>.
		/// </summary>
		ISession Session { get; }
		#endregion
	}
}