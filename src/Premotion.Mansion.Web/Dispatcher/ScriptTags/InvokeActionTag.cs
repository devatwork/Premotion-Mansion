using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Dispatcher.ScriptTags
{
	/// <summary>
	/// Invokes an action on a controllers.
	/// </summary>
	[Named(Constants.TagNamespaceUri, "invokeAction")]
	public class InvokeActionTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			// get the parameters
			var route = GetRequiredAttribute<IPropertyBag>(context, "route");
			var areaName = route.Get(context, "area", string.Empty);
			var controllerName = route.Get<string>(context, "controller");
			var actionName = route.Get<string>(context, "action");

			// copy to the original values
			route.Set("originalArea", areaName);
			route.Set("originalController", controllerName);
			route.Set("originalAction", actionName);

			// push original action to the stack);
			using (context.Stack.Push("Route", route))
			{
				// route
				RouteToControllerAction(context, route, areaName, controllerName, actionName);
			}
		}
		/// <summary>
		/// Routes the request to the specified controller action, if the controller action is not specified the the request is routed to the 404 controller.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="route">The <see cref="IPropertyBag"/> containing the route.</param>
		/// <param name="areaName">The name of the area in which the controller lives.</param>
		/// <param name="controllerName">The name of the controller.</param>
		/// <param name="actionName">The name of the action which to invoke.</param>
		private static void RouteToControllerAction(MansionContext context, IPropertyBag route, string areaName, string controllerName, string actionName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (route == null)
				throw new ArgumentNullException("route");
			if (areaName == null)
				throw new ArgumentNullException("areaName");
			if (string.IsNullOrEmpty(controllerName))
				throw new ArgumentNullException("controllerName");
			if (string.IsNullOrEmpty(actionName))
				throw new ArgumentNullException("actionName");

			// set the route
			route.Set("area", areaName);
			route.Set("controller", controllerName);
			route.Set("action", actionName);

			// get the services
			var resourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var scriptService = context.Nucleus.Get<ITagScriptService>(context);
			var templateService = context.Nucleus.Get<ITemplateService>(context);

			// get the paths
			var scriptResourcePath = resourceService.ParsePath(context, new PropertyBag
			                                                            {
			                                                            	{"path", areaName + "/" + controllerName + "Controller.xinclude"},
			                                                            	{"overridable", true}
			                                                            });
			var templateResourcePath = resourceService.ParsePath(context, new PropertyBag
			                                                              {
			                                                              	{"path", areaName + "/" + controllerName + "Controller.htm"},
			                                                              	{"overridable", true}
			                                                              });

			// get the resources and check if controller does not exist
			IEnumerable<IResource> scriptResources;
			if (!resourceService.TryGet(context, scriptResourcePath, out scriptResources))
			{
				// controller script not found
				RouteTo404Controller(context, route, string.Empty, "404", "NotFound");
				return;
			}
			IEnumerable<IResource> templateResources;
			resourceService.TryGet(context, templateResourcePath, out templateResources);

			// open the resources
			using (scriptService.Open(context, scriptResources))
			using (templateService.Open(context, templateResources))
			{
				// check if action does not exist
				IScript action;
				if (!context.ProcedureStack.TryPeek<IScript>("Handle" + actionName, out action))
				{
					// check if the default action does not exists
					if (!context.ProcedureStack.TryPeek<IScript>("HandleDefault", out action))
					{
						// action and default action not found
						RouteTo404Controller(context, route, string.Empty, "404", "NotFound");
						return;
					}
				}

				// invoke the InitializeController event
				using (context.Stack.Push("Arguments", new PropertyBag()))
				{
					foreach (var handler in context.EventHandlerStack.PeekAll("InitializeController"))
						handler.Execute(context);
				}

				// invoke the action on the controller procedure
				using (context.Stack.Push("Arguments", new PropertyBag()))
					action.Execute(context);
			}
		}
		/// <summary>
		/// Routes the request to the 404 controller action.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="route">The <see cref="IPropertyBag"/> containing the route.</param>
		/// <param name="areaName">The name of the area in which the controller lives.</param>
		/// <param name="controllerName">The name of the controller.</param>
		/// <param name="actionName">The name of the action which to invoke.</param>
		private static void RouteTo404Controller(MansionContext context, IPropertyBag route, string areaName, string controllerName, string actionName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (route == null)
				throw new ArgumentNullException("route");
			if (areaName == null)
				throw new ArgumentNullException("areaName");
			if (string.IsNullOrEmpty(controllerName))
				throw new ArgumentNullException("controllerName");
			if (string.IsNullOrEmpty(actionName))
				throw new ArgumentNullException("actionName");

			// guard against endless routing
			if ("404".Equals(controllerName))
			{
				var webContext = context.Cast<MansionWebContext>();
				webContext.HttpContext.Response.StatusCode = 404;
				context.BreakExecution = true;
				return;
			}

			// route to controller
			RouteToControllerAction(context, route, string.Empty, "404", "NotFound");
		}
		#endregion
	}
}