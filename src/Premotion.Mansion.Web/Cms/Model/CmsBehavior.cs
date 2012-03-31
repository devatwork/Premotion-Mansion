using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;

namespace Premotion.Mansion.Web.Cms.Model
{
	/// <summary>
	/// Describes the behavior of an type within the CMS.
	/// </summary>
	public class CmsBehavior
	{
		#region Constructors
		/// <summary>
		/// Private constructor, use <see cref="CmsBehavior.Create"/> factory method.
		/// </summary>
		private CmsBehavior()
		{
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates an instance of <see cref="CmsBehavior"/> from the <paramref name="descriptor"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="descriptor">The <see cref="CmsBehaviorDescriptor"/> from which to extract the behavoir.</param>
		/// <returns>Returns the <see cref="CmsBehavior"/> instance.</returns>
		public static CmsBehavior Create(IMansionContext context, CmsBehaviorDescriptor descriptor)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (descriptor == null)
				throw new ArgumentNullException("descriptor");

			// set the properties
			var behavior = new CmsBehavior
			               {
			               	Label = descriptor.Properties.Get(context, "label", descriptor.TypeDefinition.Name),
			               	PathToIcon = descriptor.Properties.Get(context, "icon", string.Empty),
			               	IsAbstract = descriptor.Properties.Get(context, "abstract", false)
			               };

			//  add all the child type
			foreach (var childTypeDescriptor in descriptor.GetDescriptors<ChildTypeDescriptor>())
				childTypeDescriptor.SetChildTypes(context, behavior);

			// return the instantiated behavior
			return behavior;
		}
		#endregion
		#region Child Type Methods
		/// <summary>
		/// Adds a <paramref name="childType"/> to this CMS behavior.
		/// </summary>
		/// <param name="childType">The <see cref="ChildType"/> which to add.</param>
		public void Add(ChildType childType)
		{
			// validate arguments
			if (childType == null)
				throw new ArgumentNullException("childType");
			children.Add(childType);
		}
		/// <summary>
		/// Gets the allowed child <see cref="ITypeDefinition"/> based on the <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the allowed child <see cref="ITypeDefinition"/>.</returns>
		public IEnumerable<ITypeDefinition> GetAllowedChildTypes(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the allowed children
			return children.Where(candidate => candidate.IsAllowed(context)).Select(x => x.Type);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether this type may contain children.
		/// </summary>
		public bool HasChildren
		{
			get { return children.Count != 0; }
		}
		/// <summary>
		/// Gets a flag indicating whether this type has an icon.
		/// </summary>
		public bool HasIcon
		{
			get { return !string.IsNullOrEmpty(PathToIcon); }
		}
		/// <summary>
		/// Gets the path to the icon of this type.
		/// </summary>
		public string PathToIcon { get; private set; }
		/// <summary>
		/// Gets the label of the <see cref="ITypeDefinition"/> as displayed within the CMS.
		/// </summary>
		public string Label { get; private set; }
		/// <summary>
		/// Gets a flag indicating whether the type is abstract and should not be created in the CMS. Default is false
		/// </summary>
		public bool IsAbstract { get; private set; }
		#endregion
		#region Private Fields
		private readonly ICollection<ChildType> children = new List<ChildType>();
		#endregion
	}
}