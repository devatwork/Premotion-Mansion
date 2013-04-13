using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Represents a link base.
	/// </summary>
	public class Linkbase
	{
		#region Constructors
		/// <summary>
		/// Constructs a linkbase.
		/// </summary>
		/// <param name="definition"></param>
		/// <param name="data"></param>
		public Linkbase(LinkbaseDefinition definition, LinkbaseData data)
		{
			// validate arguments
			if (definition == null)
				throw new ArgumentNullException("definition");
			if (data == null)
				throw new ArgumentNullException("data");

			// set the values
			this.definition = definition;
			this.data = data;
		}
		#endregion
		#region Manipulation Methods
		/// <summary>
		/// Creates a link between this and <paramref name="target"/>. The link type is identified by <paramref name="name"/> and. Both reqcords must be saved after the link was created.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="target">The target <see cref="Linkbase"/> to which to create the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <param name="properties">The properties of the link.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the specified link could not be created.</exception>
		public void Link(IMansionContext context, Linkbase target, string name, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (target == null)
				throw new ArgumentNullException("target");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// create the link
			Link sourceToTarget;
			Link targetToSource;
			LinkDefinition.Create(context, this, target, name, properties, out sourceToTarget, out targetToSource);

			// add the links
			data.Links.Add(sourceToTarget);
			target.data.Links.Add(targetToSource);
		}
		/// <summary>
		/// Removes a link between this and <paramref name="target"/>. The link type is identified by <paramref name="name"/> and. Both reqcords must be saved after the link was removed.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="target">The target <see cref="Linkbase"/> to which to remove the link.</param>
		/// <param name="name">The name of the link which to create.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		public void Unlink(IMansionContext context, Linkbase target, string name)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (target == null)
				throw new ArgumentNullException("target");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// get the link definition
			var linkDefinition = definition.GetLinkDefinition(name);

			// find the matching links
			foreach (var to in Links.To(target).OfType(linkDefinition).ToList())
			{
				// find the corresponding from links
				foreach (var from in target.Links.To(this).OfType(linkDefinition).ToList())
					target.data.Links.Remove(from);

				// remove the link
				data.Links.Remove(to);
			}
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of this linkbase.
		/// </summary>
		public string Id
		{
			get { return data.Id; }
		}
		/// <summary>
		/// Gets the <see cref="Link"/>s of this linkbase.
		/// </summary>
		public IEnumerable<Link> Links
		{
			get { return data.Links; }
		}
		/// <summary>
		/// Gets the <see cref="LinkbaseDefinition"/> of this <see cref="Linkbase"/>.
		/// </summary>
		public LinkbaseDefinition Definition
		{
			get { return definition; }
		}
		/// <summary>
		/// Gets the <see cref="LinkbaseData"/>.
		/// </summary>
		public LinkbaseData Data
		{
			get { return data; }
		}
		#endregion
		#region Private Fields
		private readonly LinkbaseData data;
		private readonly LinkbaseDefinition definition;
		#endregion
	}
}