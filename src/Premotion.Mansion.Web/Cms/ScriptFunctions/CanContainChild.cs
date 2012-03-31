using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;

namespace Premotion.Mansion.Web.Cms.ScriptFunctions
{
	/// <summary>
	/// Checks whether the parent type can contain the child.
	/// </summary>
	[ScriptFunction("CanContainChild")]
	public class CanContainChild : FunctionExpression
	{
		/// <summary>
		/// Gets the label of a particular <paramref name="parentType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parentType">The <see cref="ITypeDefinition"/> which to check for.</param>
		/// <param name="childType">The <see cref="ITypeDefinition"/> which to check on.</param>
		public bool Evaluate(IMansionContext context, ITypeDefinition parentType, ITypeDefinition childType)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parentType == null)
				throw new ArgumentNullException("parentType");

			// find the descriptor
			CmsBehaviorDescriptor cmsDescriptor;
			return parentType.TryFindDescriptorInHierarchy(out cmsDescriptor) && cmsDescriptor.GetBehavior(context).GetAllowedChildTypes(context).Any(childType.IsAssignable);
		}
	}
}