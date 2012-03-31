using System;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Thrown when a <see cref="MansionContextExtension"/> can not be found on a particular <see cref="IMansionContext"/>.
	/// </summary>
	public class MansionContextExtensionNotFoundException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="extensionType"></param>
		public MansionContextExtensionNotFoundException(Type extensionType) : base(string.Format("Could not find extension of type '{0}'", extensionType))
		{
		}
		#endregion
	}
}