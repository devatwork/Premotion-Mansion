using System;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Thrown when a <see cref="ContextExtension"/> can not be found on a particular <see cref="IContext"/>.
	/// </summary>
	public class ContextExtensionNotFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="extensionType"></param>
		public ContextExtensionNotFoundException(Type extensionType) : base(string.Format("Could not find extension of type '{0}'", extensionType))
		{
		}
		#endregion
	}
}