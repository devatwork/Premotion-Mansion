using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;

namespace Premotion.Mansion.Web.Cms.ScriptFunctions
{
	/// <summary>
	/// Gets a property of a particular <see cref="ITypeDefinition"/>.
	/// </summary>
	[ScriptFunction("GetTypeDefinitionProperty")]
	public class GetTypeDefinitionProperty : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public GetTypeDefinitionProperty(ITypeService typeService)
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
		/// <param name="propertyName">The name of the property which to get.</param>
		/// <param name="defaultValue">The default value.</param>
		public object Evaluate(IMansionContext context, string typeName, string propertyName, object defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// try to find the type
			ITypeDefinition type;
			if (!typeService.TryLoad(context, typeName, out type))
				return defaultValue;

			// find the descriptor
			CmsBehaviorDescriptor cmsDescriptor;
			if (!type.TryFindDescriptorInHierarchy(candidate => true, out cmsDescriptor))
				return defaultValue;

			// get the value of the property
			object value;
			return !cmsDescriptor.Properties.TryGet(context, propertyName, out value) ? defaultValue : value;
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}