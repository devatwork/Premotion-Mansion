using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.Controls.Providers
{
	/// <summary>
	/// Represents a leaf in a tree.
	/// </summary>
	public class Leaf
	{
		#region Constructors
		/// <summary>
		/// Constructs a leaf.
		/// </summary>
		/// <param name="properties">The properties of the leaf.</param>
		/// <param name="children">The children of this leaf.</param>
		public Leaf(IPropertyBag properties, IEnumerable<Leaf> children)
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");
			if (children == null)
				throw new ArgumentNullException("properties");

			// set values
			this.properties = properties;
			this.children = children.ToList();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the properties of this lead.
		/// </summary>
		public IPropertyBag Properties
		{
			get { return properties; }
		}
		/// <summary>
		/// Gets the child <see cref="Leaf"/>s of this leaf.
		/// </summary>
		public IEnumerable<Leaf> Children
		{
			get { return children; }
		}
		/// <summary>
		/// Gets a flag indicating whether this leaf has children or not.
		/// </summary>
		public bool HasChildren
		{
			get { return children.Count != 0; }
		}
		#endregion
		#region Private Fields
		private readonly List<Leaf> children = new List<Leaf>();
		private readonly IPropertyBag properties;
		#endregion
	}
}