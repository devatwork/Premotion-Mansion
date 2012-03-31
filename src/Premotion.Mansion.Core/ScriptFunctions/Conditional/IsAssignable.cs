using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether type is assignable to another type.
	/// </summary>
	[ScriptFunction("IsAssignable")]
	public class IsAssignable : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="orginalType"></param>
		/// <param name="targetType"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, ITypeDefinition orginalType, ITypeDefinition targetType)
		{
			// valdidate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (orginalType == null)
				throw new ArgumentNullException("orginalType");
			if (targetType == null)
				throw new ArgumentNullException("targetType");

			return orginalType.IsAssignable(targetType);
		}
	}
}