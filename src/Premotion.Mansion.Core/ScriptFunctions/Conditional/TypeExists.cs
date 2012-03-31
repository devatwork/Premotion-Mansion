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
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public TypeExists(ITypeService typeService)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="typeName">The type which to check.</param>
		/// <returns></returns>
		public bool Evaluate(IMansionContext context, string typeName)
		{
			// valdidate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				return false;

			// check if the type is found
			ITypeDefinition typeDefinition;
			return typeService.TryLoad(context, typeName, out typeDefinition);
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}