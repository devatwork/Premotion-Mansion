using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Implements <see cref="ITypeDefinition"/>.
	/// </summary>
	public class TypeDefinition : DescripteeBase, ITypeDefinition
	{
		#region Constructors
		/// <summary>
		/// Constructs a new type definition.
		/// </summary>
		/// <param name="name">The name of the type.</param>
		/// <param name="parent">The parent of this type definition.</param>
		public TypeDefinition(string name, ITypeDefinition parent)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set values
			Name = name;
			Parent = parent;
		}
		#endregion
		#region Property Methods
		/// <summary>
		/// Gets a property by it's name.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns>Returns the property.</returns>
		/// <exception cref="PropertyNotFoundException">Thrown when a property with the specified name can not be found.</exception>
		public IPropertyDefinition GetProperty(string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			var property = (from candidate in Properties
			                where name.Equals(candidate.Name, StringComparison.OrdinalIgnoreCase)
			                select candidate).SingleOrDefault();
			if (property == null)
				throw new PropertyNotFoundException(this, name);
			return property;
		}
		#endregion
		#region Relational Methods
		/// <summary>
		/// Checks whether this type inherits from <paramref name="targetType"/>.
		/// </summary>
		/// <param name="targetType">The parent type.</param>
		/// <returns>Returns true when this type does intherit from <paramref name="targetType"/>, otherwise false.</returns>
		public bool IsAssignable(ITypeDefinition targetType)
		{
			// validate arguments
			if (targetType == null)
				throw new ArgumentNullException("targetType");

			return HierarchyReverse.Any(candidate => candidate.Name.Equals(targetType.Name, StringComparison.OrdinalIgnoreCase));
		}
		/// <summary>
		/// Gets the types inheriting from this type.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <returns>Returns the inheriting types.</returns>
		public IEnumerable<ITypeDefinition> GetInheritingTypes(IContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// loop through all the types
			var typeService = context.Cast<INucleusAwareContext>().Nucleus.Get<ITypeService>(context);

			// returns inheriting types
			return from candidate in typeService.LoadAll(context)
			       where !Name.Equals(candidate.Name, StringComparison.OrdinalIgnoreCase) && candidate.IsAssignable(this)
			       select candidate;
		}
		#endregion
		#region Relational Properties
		/// <summary>
		/// Gets a flag indicating whether this type has a parent.
		/// </summary>
		public bool HasParent
		{
			get { return Parent != null; }
		}
		/// <summary>
		/// Gets the parent of this type definition.
		/// </summary>
		public ITypeDefinition Parent { get; private set; }
		/// <summary>
		/// Gets the hierarchy of this type. Top down.
		/// </summary>
		public IEnumerable<ITypeDefinition> Hierarchy
		{
			get
			{
				// check if there is a parent
				if (HasParent)
				{
					foreach (var ancestor in Parent.Hierarchy)
						yield return ancestor;
				}

				yield return this;
			}
		}
		/// <summary>
		/// Gets the reverse hierarchy of this type. Bottom up.
		/// </summary>
		public IEnumerable<ITypeDefinition> HierarchyReverse
		{
			get
			{
				yield return this;

				// check if there is a parent
				if (HasParent)
				{
					foreach (var ancestor in Parent.HierarchyReverse)
						yield return ancestor;
				}
			}
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the properties of this type.
		/// </summary>
		public ICollection<IPropertyDefinition> InternalProperties
		{
			get { return properties; }
		}
		/// <summary>
		/// Gets the name of this type definition.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the properties of this type.
		/// </summary>
		public IEnumerable<IPropertyDefinition> Properties
		{
			get { return properties; }
		}
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return Name;
		}
		#endregion
		#region Private Fields
		private readonly List<IPropertyDefinition> properties = new List<IPropertyDefinition>();
		#endregion
	}
}