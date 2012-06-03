using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;

namespace Premotion.Mansion.Web.Cms.Model
{
	/// <summary>
	/// Represents a child type.
	/// </summary>
	public class ChildType
	{
		#region Constructors
		/// <summary>
		/// Private constructor, use <see cref="Create"/>.
		/// </summary>
		private ChildType(ITypeDefinition type)
		{
			// set values
			this.type = type;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates an instance of <see cref="ChildType"/> from <paramref name="descriptor"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="descriptor">The <see cref="ChildTypeDescriptor"/>.</param>
		/// <param name="behavior">The <see cref="CmsBehavior"/>.</param>
		public static void Create(IMansionContext context, ChildTypeDescriptor descriptor, CmsBehavior behavior)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (descriptor == null)
				throw new ArgumentNullException("descriptor");
			if (behavior == null)
				throw new ArgumentNullException("behavior");

			// get the type
			var type = descriptor.Properties.Get<ITypeDefinition>(context, "type", null);
			var baseType = descriptor.Properties.Get<ITypeDefinition>(context, "baseType", null);
			if (type == null && baseType == null)
				throw new InvalidOperationException(string.Format("Invalid child type descriptor on type '{0}'. Specify either an type or a baseType.", descriptor.TypeDefinition.Name));
			if (type != null && baseType != null)
				throw new InvalidOperationException(string.Format("Invalid child type descriptor on type '{0}'. Ambigious type detected. Specify either an type or a baseType.", descriptor.TypeDefinition.Name));

			if (type != null)
				CreateChildType(context, descriptor, behavior, type);
			else
			{
				CreateChildType(context, descriptor, behavior, baseType);
				foreach (var inheritingType in baseType.GetInheritingTypes(context))
					CreateChildType(context, descriptor, behavior, inheritingType);
			}
		}
		/// <summary>
		/// Creates the child type.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="descriptor"></param>
		/// <param name="behavior"></param>
		/// <param name="type"></param>
		private static void CreateChildType(IMansionContext context, ChildTypeDescriptor descriptor, CmsBehavior behavior, ITypeDefinition type)
		{
			// create the child type
			var childType = new ChildType(type);

			// check if there is an is allowed expression
			var isAllowedExpressionString = descriptor.Properties.Get(context, "allowedExpression", string.Empty);
			if (!string.IsNullOrEmpty(isAllowedExpressionString))
			{
				// get the expresion script service
				var expressionScriptService = context.Nucleus.ResolveSingle<IExpressionScriptService>();

				// compile the script
				childType.IsAllowedExpression = expressionScriptService.Parse(context, new LiteralResource(isAllowedExpressionString));
			}

			behavior.Add(childType);
		}
		#endregion
		#region Methods
		/// <summary>
		/// Gets a flag indicating whether this child type is allowed within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns true when allowed, otherwise false.</returns>
		public bool IsAllowed(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// if there is no expression the type is always allowed
			if (IsAllowedExpression == null)
				return true;

			// execute the expression
			using (context.Stack.Push("Candidate", new PropertyBag
			                                       {
			                                       	{"type", Type}
			                                       }))
				return IsAllowedExpression.Execute<bool>(context);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="ITypeDefinition"/> of the child.
		/// </summary>
		public ITypeDefinition Type
		{
			get { return type; }
		}
		/// <summary>
		/// Gets/Sets the is allowed <see cref="IScript"/>.
		/// </summary>
		private IScript IsAllowedExpression { get; set; }
		#endregion
		#region Private Fields
		private readonly ITypeDefinition type;
		#endregion
	}
}