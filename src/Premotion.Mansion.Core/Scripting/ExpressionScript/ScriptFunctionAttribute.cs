using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Decorated classes are identified as script functions.
	/// </summary>
	public class ScriptFunctionAttribute : NamedAttribute
	{
		#region Constructors
		/// <summary>
		/// Constructs a script function attribute.
		/// </summary>
		/// <param name="name">The name of the script function.</param>
		public ScriptFunctionAttribute(string name) : base(typeof (FunctionExpression), Constants.NamespaceUri, name)
		{
		}
		#endregion
	}
}