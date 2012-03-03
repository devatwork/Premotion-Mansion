using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web.Assets
{
	/// <summary>
	/// Represents an asset.
	/// </summary>
	public class AssetEntry
	{
		#region Factory Methods
		/// <summary>
		/// Creates an <see cref="AssetEntry"/> instance from <paramref name="node"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="folder">The <see cref="AssetFolder"/>.</param>
		/// <param name="node">The <see cref="Node"/>.</param>
		/// <returns>Returns the created <see cref="AssetEntry"/> instance.</returns>
		public static AssetEntry Create(MansionContext context, AssetFolder folder, Node node)
		{
			//validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (folder == null)
				throw new ArgumentNullException("folder");
			if (node == null)
				throw new ArgumentNullException("node");

			return new AssetEntry
			       {
			       	AssetFolder = folder,
			       	Name = node.Pointer.Name,
			       	Size = node.Get<long>(context, "size", 0),
			       	ModificationDate = node.Get(context, "modified", DateTime.MinValue)
			       };
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the folder in which this asset was stored.
		/// </summary>
		public AssetFolder AssetFolder { get; set; }
		/// <summary>
		/// Gets the name of this asset.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the size of the asset in bytes.
		/// </summary>
		public long Size { get; private set; }
		/// <summary>
		/// Gets the last modified date of the asset.
		/// </summary>
		public DateTime ModificationDate { get; private set; }
		#endregion
	}
}