using System;
using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Decorated classes are identified as script functions.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ScriptFunctionAttribute : NamedAttribute
	{
		#region Constructors
		/// <summary>
		/// Constructs a script function attribute.
		/// </summary>
		/// <param name="name">The name of the script function.</param>
		public ScriptFunctionAttribute(string name) : base(Constants.NamespaceUri, name)
		{
		}
		#endregion
	}
}