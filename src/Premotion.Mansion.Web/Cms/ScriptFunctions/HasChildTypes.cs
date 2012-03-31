using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;

namespace Premotion.Mansion.Web.Cms.ScriptFunctions
{
	/// <summary>
	/// Gets the label of a particular <see cref="ITypeDefinition"/>.
	/// </summary>
	[ScriptFunction("HasChildTypes")]
	public class HasChildTypes : FunctionExpression
	{
		/// <summary>
		/// Gets the label of a particular <paramref name="typeDefinition"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> for which to get the label.</param>
		public bool Evaluate(IMansionContext context, ITypeDefinition typeDefinition)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");

			// find the descriptor
			CmsBehaviorDescriptor cmsDescriptor;
			if (!typeDefinition.TryFindDescriptorInHierarchy(out cmsDescriptor))
				return false;

			// return the friendly name
			return cmsDescriptor.GetBehavior(context).HasChildren;
		}
	}
}