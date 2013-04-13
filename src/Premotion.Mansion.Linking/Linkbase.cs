using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

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
		/// <param name="id"></param>
		/// <param name="record"></param>
		private Linkbase(LinkbaseDefinition definition, string id, Record record)
		{
			// validate arguments
			if (definition == null)
				throw new ArgumentNullException("definition");
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");
			if (record == null)
				throw new ArgumentNullException("record");

			// set the values
			this.definition = definition;
			this.id = id;
			this.record = record;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Gets the <see cref="Linkbase"/> of the given <paramref name="record"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="definition">The <see cref="LinkbaseDefinition"/>.</param>
		/// <param name="record">The <see cref="Record"/>.</param>
		/// <returns>Returns the loaded <see cref="Linkbase"/>.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static Linkbase Create(IMansionContext context, LinkbaseDefinition definition, Record record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (definition == null)
				throw new ArgumentNullException("definition");
			if (record == null)
				throw new ArgumentNullException("record");

			// create the link base
			var linkbase = new Linkbase(definition, record.Get<string>(context, "guid"), record);

			// initialize the linkbase
			linkbase.Load(context);

			// return the initialized linkbase
			return linkbase;
		}
		#endregion
		#region Load & Store Methods
		private void Load(IMansionContext context)
		{
			throw new NotImplementedException();
		}
		private void Save(IMansionContext context)
		{
			throw new NotImplementedException();
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
			links.Add(sourceToTarget);
			target.links.Add(targetToSource);

			// store both bases
			Save(context);
			target.Save(context);
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
					target.links.Remove(from);

				// remove the link
				links.Remove(to);
			}

			// store both bases
			Save(context);
			target.Save(context);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of this linkbase.
		/// </summary>
		public string Id
		{
			get { return id; }
		}
		/// <summary>
		/// Gets the <see cref="Link"/>s of this linkbase.
		/// </summary>
		public IEnumerable<Link> Links
		{
			get { return links; }
		}
		/// <summary>
		/// Gets the <see cref="LinkbaseDefinition"/> of this <see cref="Linkbase"/>.
		/// </summary>
		public LinkbaseDefinition Definition
		{
			get { return definition; }
		}
		#endregion
		#region Private Fields
		private readonly LinkbaseDefinition definition;
		private readonly string id;
		private readonly List<Link> links = new List<Link>();
		private readonly Record record;
		#endregion
	}
}