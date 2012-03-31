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
	[ScriptFunction("GetTypeDefinitionLabel")]
	public class GetTypeDefinitionLabel : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public GetTypeDefinitionLabel(ITypeService typeService)
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
		/// Gets the label of a particular <paramref name="typeName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeName">The <see cref="ITypeDefinition"/> for which to get the label.</param>
		public string Evaluate(IMansionContext context, string typeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			// try to find the type
			ITypeDefinition type;
			if (!typeService.TryLoad(context, typeName, out type))
				return typeName;

			// find the descriptor
			CmsBehaviorDescriptor cmsDescriptor;
			if (!type.TryFindDescriptorInHierarchy(out cmsDescriptor))
				return typeName;

			// return the friendly name
			return cmsDescriptor.GetBehavior(context).Label;
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}