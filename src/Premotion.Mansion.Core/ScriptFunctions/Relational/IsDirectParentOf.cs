using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Relational
{
	/// <summary>
	/// Checks whether the candidate parent is indeed the parent of the child.
	/// </summary>
	[ScriptFunction("IsDirectParentOf")]
	public class IsDirectParentOf : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="candidateParent"></param>
		/// <param name="childPointer"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, NodePointer candidateParent, NodePointer childPointer)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (candidateParent == null)
				throw new ArgumentNullException("candidateParent");
			if (childPointer == null)
				throw new ArgumentNullException("childPointer");
			return candidateParent.Id.Equals(childPointer.Parent.Id);
		}
	}
}