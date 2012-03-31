using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the default block rendering behavior.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "defaultBlockBehavior")]
	public class DefaultBlockBehaviorDescriptor : BlockBehaviorDescriptor
	{
		#region Render Methods
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		protected override void DoRender(IMansionContext context, IPropertyBag blockProperties, string targetField)
		{
			// get the services
			var resourceService = context.Nucleus.ResolveSingle<IApplicationResourceService>();
			var templateService = context.Nucleus.ResolveSingle<ITemplateService>();
			var tagScriptService = context.Nucleus.ResolveSingle<ITagScriptService>();

			// get the resource paths
			var templateResourcePath = resourceService.ParsePath(context, new PropertyBag
			                                                              {
			                                                              	{"type", blockProperties.Get<string>(context, "type")},
			                                                              	{"extension", "htm"}
			                                                              });
			var scriptResourcePath = resourceService.ParsePath(context, new PropertyBag
			                                                            {
			                                                            	{"type", blockProperties.Get<string>(context, "type")},
			                                                            	{"extension", "xinclude"}
			                                                            });

			// open the block template and script
			using (templateService.Open(context, resourceService.Get(context, templateResourcePath)))
			using (tagScriptService.Open(context, resourceService.Get(context, scriptResourcePath)))
			using (context.Stack.Push("BlockProperties", blockProperties))
			using (templateService.Render(context, "BlockContainer", targetField))
				context.ProcedureStack.Peek<IScript>("RenderBlock").Execute(context);
		}
		#endregion
	}
}