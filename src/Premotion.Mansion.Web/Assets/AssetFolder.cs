using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web.Assets
{
	/// <summary>
	/// Represents a collection of <see cref="AssetEntry"/>.
	/// </summary>
	public class AssetFolder
	{
		#region Factory Methods
		/// <summary>
		/// Creates an <see cref="AssetFolder"/> instance from the <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="assetType">The <see cref="AssetType"/>.</param>
		/// <param name="node">The <see cref="Node"/>.</param>
		/// <returns>Returns the constructed <see cref="AssetFolder"/> instance.</returns>
		public static AssetFolder Create(IMansionContext context, AssetType assetType, Node node)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (assetType == null)
				throw new ArgumentNullException("assetType");
			if (node == null)
				throw new ArgumentNullException("node");

			// create the folder
			return new AssetFolder
			       {
			       	AssetType = assetType,
			       	Path = "/" + node.Pointer.Name.ToLower() + "/",
			       	Label = node.Pointer.Name,
			       	Node = node,
			       	Url = AssetUtil.GetUrl(context, assetType.Node, node)
			       };
		}
		#endregion
		#region Properties
		/// <summary>
		/// gets the <see cref="AssetType"/> of this folder.
		/// </summary>
		public AssetType AssetType { get; private set; }
		/// <summary>
		/// Gets the base path of this asset folder.
		/// </summary>
		public string Path { get; private set; }
		/// <summary>
		/// Gets the label of the current folder.
		/// </summary>
		public string Label { get; private set; }
		/// <summary>
		/// Gets the <see cref="Node"/> backing this asset type.
		/// </summary>
		public Node Node { get; private set; }
		#endregion
		#region Static Properties
		/// <summary>
		/// Defines the unknown folder
		/// </summary>
		public static AssetFolder Unknown
		{
			get { return unknown; }
		}
		/// <summary>
		/// Gets the <see cref="Uri"/> to this folder.
		/// </summary>
		public Uri Url { get; private set; }
		#endregion
		#region Private Fields
		private static readonly AssetFolder unknown = new AssetFolder
		                                              {
		                                              	AssetType = AssetType.Unknown,
		                                              	Label = "Unknown"
		                                              };
		#endregion
	}
}