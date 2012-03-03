using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.ScriptFunctions.Types
{
	/// <summary>
	/// Gets the subtypes of a particular type as a CSV.
	/// </summary>
	[ScriptFunction("GetSubTypes")]
	public class GetSubTypes : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, ITypeDefinition type)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (type == null)
				throw new ArgumentNullException("type");

			return string.Join(",", type.GetInheritingTypes(context).Select(inheritedType => inheritedType.Name));
		}
	}
}