using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether a type exists.
	/// </summary>
	[ScriptFunction("TypeExists")]
	public class TypeExists : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="typeName">The type which to check.</param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, string typeName)
		{
			// valdidate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				return false;

			// get the type service
			var typeService = context.Nucleus.Get<ITypeService>(context);

			// check if the type is found
			ITypeDefinition typeDefinition;
			return typeService.TryLoad(context, typeName, out typeDefinition);
		}
	}
}