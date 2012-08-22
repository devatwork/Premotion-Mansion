using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;

namespace Premotion.Mansion.Web.Web.Types.Default
{
	/// <summary>
	/// This listener propagates properties to all children of updated <see cref="Node"/>.
	/// </summary>
	public class PropagateListener : NodeListener
	{
		#region Constants
		/// <summary>
		/// The prefix of propagated properties.
		/// </summary>
		private const string PropagatePrefix = "_propagate";
		#endregion
		#region Overrides of NodeListener
		/// <summary>
		/// This method is called just after a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The modified node.</param>
		/// <param name="modifiedProperties">The properties which were set to the updated <paramref name="node"/>.</param>
		protected override void DoAfterUpdate(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// get the propagate property names
			var propagatePropertyNames = modifiedProperties.Names.Where(x => x.StartsWith(PropagatePrefix, StringComparison.OrdinalIgnoreCase));

			// get the property names who's value is true
			var propagatedNowProperties = propagatePropertyNames.Where(x => modifiedProperties.Get(context, x, false));

			// loop over all the updated properties and propagated properties
			foreach (var propagatePropertyName in propagatedNowProperties)
			{
				// get the name of the property being propagated
				var propagatedPropertyName = propagatePropertyName.Substring(PropagatePrefix.Length);

				// get the propagated property value
				var propagatedPropertyValue = modifiedProperties.Get<object>(context, propagatedPropertyName);

				// retrieve all the children nodes and update them all
				foreach (var childNode in context.Repository.RetrieveNodeset(context, new PropertyBag
				                                                                      {
				                                                                      	{"parentSource", node},
				                                                                      	{"depth", "any"}
				                                                                      }).Nodes)
				{
					context.Repository.UpdateNode(context, childNode, new PropertyBag
					                                                  {
					                                                  	{propagatedPropertyName, propagatedPropertyValue}
					                                                  });
				}
			}
		}
		#endregion
	}
}