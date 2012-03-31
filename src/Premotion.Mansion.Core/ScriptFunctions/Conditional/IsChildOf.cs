using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether the candidate child is a child of the parent.
	/// </summary>
	[ScriptFunction("IsChildOf")]
	public class IsChildOf : FunctionExpression
	{
		/// <summary>
		/// Checks whether the candidate child is a child of the parent.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="child"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, Node child, Node parent)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (child == null)
				throw new ArgumentNullException("child");
			if (parent == null)
				throw new ArgumentNullException("parent");

			return child.Pointer.IsChildOf(parent.Pointer);
		}
	}
}