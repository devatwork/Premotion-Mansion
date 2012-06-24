using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Descriptors;

namespace Premotion.Mansion.Web.Portal.ScriptFunctions
{
	/// <summary>
	/// Checks whether the given type is a shared block.
	/// </summary>
	[ScriptFunction("IsSharedBlock")]
	public class IsSharedBlock : FunctionExpression
	{
		/// <summary>
		/// Checks whether the given type is a shared block.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <returns>Returns true if the type is a static block, otherwise false.</returns>
		public bool Evaluate(IMansionContext context, ITypeDefinition type)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (type == null)
				throw new ArgumentNullException("type");

			// check if this type has a shared block
			SharedBlockDescriptor sharedBlockDescriptor;
			return type.TryFindDescriptorInHierarchy(out sharedBlockDescriptor);
		}
	}
}