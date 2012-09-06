using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;
using Premotion.Mansion.Web.Hosting;

namespace Premotion.Mansion.Web.Cms.ScriptFunctions
{
	/// <summary>
	/// Gets the label of a particular <see cref="ITypeDefinition"/>.
	/// </summary>
	[ScriptFunction("GetTypeDefinitionIcon")]
	public class GetTypeDefinitionIcon : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public GetTypeDefinitionIcon(ITypeService typeService)
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
				return string.Empty;

			// find the descriptor
			CmsBehaviorDescriptor cmsDescriptor;
			if (!type.TryFindDescriptorInHierarchy(candidate => candidate.GetBehavior(context).HasIcon, out cmsDescriptor))
				return string.Empty;

			// get the behavior
			var behavior = cmsDescriptor.GetBehavior(context);

			// get the url
			var webContext = context.Cast<IMansionWebContext>();
			var prefixedRelativePath = HttpUtilities.CombineIntoRelativeUrl(webContext.HttpContext.Request.ApplicationPath, StaticResourceRequestHandler.Prefix, behavior.PathToIcon);
			var imageUrl = new Uri(webContext.ApplicationBaseUri, prefixedRelativePath);

			// build the icon html
			return string.Format("<i><img class=\"icon\" src=\"{0}\" alt=\"{1}\"></i>", imageUrl, behavior.Label ?? string.Empty);
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}