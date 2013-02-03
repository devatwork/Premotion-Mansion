using System;
using System.Linq;
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
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public IsAssignable(ITypeService typeService)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			this.typeService = typeService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="orginalType"></param>
		/// <param name="targetTypes"></param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, ITypeDefinition orginalType, string targetTypes)
		{
			// valdidate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (orginalType == null)
				throw new ArgumentNullException("orginalType");
			if (string.IsNullOrEmpty(targetTypes))
				throw new ArgumentNullException("targetTypes");

			// get the types
			var candidates = targetTypes.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => typeService.Load(context, x.Trim()));

			// check if any of the candidates match
			return candidates.Any(orginalType.IsAssignable);
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}