using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;
using Premotion.Mansion.Web.Http;

namespace Premotion.Mansion.Web.Cms.ScriptFunctions
{
	/// <summary>
	/// Gets the label of a particular <see cref="ITypeDefinition"/>.
	/// </summary>
	[ScriptFunction("GetTypeDefinitionIcon")]
	public class GetTypeDefinitionIcon : FunctionExpression
	{
		/// <summary>
		/// Gets the label of a particular <paramref name="typeName"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="typeName">The <see cref="ITypeDefinition"/> for which to get the label.</param>
		public string Evaluate(MansionContext context, string typeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			// try to find the type
			var typeService = context.Nucleus.Get<ITypeService>(context);
			ITypeDefinition type;
			if (!typeService.TryLoad(context, typeName, out type))
				return typeName;

			// find the descriptor
			CmsBehaviorDescriptor cmsDescriptor;
			if (!type.TryFindDescriptorInHierarchy(candidate => candidate.GetBehavior(context).HasIcon, out cmsDescriptor))
				return string.Empty;

			// get the behavior
			var behavior = cmsDescriptor.GetBehavior(context);

			// get the url
			var webContext = context.Cast<MansionWebContext>();
			var prefixedRelativePath = HttpUtilities.CombineIntoRelativeUrl(webContext.HttpContext.Request.ApplicationPath, PathRewriterModule.StaticResourcesPrefix, behavior.PathToIcon);
			var imageUrl = new Uri(webContext.HttpContext.Request.ApplicationBaseUri, prefixedRelativePath);

			// build the icon html
			return string.Format("<img class=\"icon\" src=\"{0}\" alt=\"{1}\">", imageUrl, behavior.Label ?? string.Empty);
		}
	}
}