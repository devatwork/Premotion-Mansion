using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web.Assets
{
	/// <summary>
	/// Represents an asset type.
	/// </summary>
	public class AssetType
	{
		#region Factory Methods
		/// <summary>
		/// Creates an instance of <see cref="AssetType"/> from <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/>.</param>
		/// <returns>Returns the created <see cref="AssetType"/>.</returns>
		public static AssetType Create(IMansionContext context, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");

			return new AssetType
			       {
			       	Label = node.Pointer.Name,
			       	BasePath = "/" + node.Pointer.Name.ToLower() + "/",
			       	Node = node,
			       	Url = AssetUtil.GetUrl(context, node, node)
			       };
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the friendly name of this resource type.
		/// </summary>
		public string Label { get; private set; }
		/// <summary>
		/// Gets the base path of this resource type.
		/// </summary>
		public string BasePath { get; private set; }
		/// <summary>
		/// Gets the <see cref="Node"/> backing this asset type.
		/// </summary>
		public Node Node { get; private set; }
		/// <summary>
		/// Gets the <see cref="Mansion.Web.Url"/> to this asset type.
		/// </summary>
		public Url Url { get; private set; }
		#endregion
		#region Static Properties
		/// <summary>
		/// Defines the unknown asset type.
		/// </summary>
		public static AssetType Unknown
		{
			get { return unknown; }
		}
		#endregion
		#region Private Fields
		private static readonly AssetType unknown = new AssetType
		                                            {
		                                            	Label = "unknown",
		                                            	BasePath = "/unknown/"
		                                            };
		#endregion
	}
}