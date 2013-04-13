using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Linking.Descriptors;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Provides extension methods used by the link module.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Gets the <see cref="Linkbase"/> of the given <paramref name="this"/>.
		/// </summary>
		/// <param name="this">The <see cref="Record"/> from which to get the linkbase.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeService">The <see cref="ITypeService"/>.</param>
		/// <returns>Returns the <see cref="Linkbase"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		/// <exception cref="TypeNotFoundException">Thrown if the type of <paramref name="this"/> could not be found.</exception>
		/// <exception cref="InvalidLinkException">Thrown if the <see cref="Record.Type"/> does not have a definition of a linkbase.</exception>
		public static Linkbase GetLinkbase(this Record @this, IMansionContext context, ITypeService typeService)
		{
			// validate arguments
			if (@this == null)
				throw new ArgumentNullException("this");
			if (context == null)
				throw new ArgumentNullException("context");
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// check if the type has got a linkbase
			var type = typeService.Load(context, @this.Type);
			LinkbaseDescriptor linkbaseDescriptor;
			if (!type.TryFindDescriptorInHierarchy(out linkbaseDescriptor))
				throw new InvalidLinkException(string.Format("Type '{0}' does not have a link base", @this.Type));

			// get the definition of the linkbase
			var definition = linkbaseDescriptor.GetDefinition(context);

			// return the created linkbase
			return Linkbase.Create(context, definition, @this);
		}
		/// <summary>
		/// Filters <paramref name="this"/> to only include links to <paramref name="target"/>.
		/// </summary>
		/// <param name="this">The <see cref="Link"/>s which to filter.</param>
		/// <param name="target">The target <see cref="Linkbase"/>.</param>
		/// <returns>Returns the filtered <see cref="Link"/>s.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static IEnumerable<Link> To(this IEnumerable<Link> @this, Linkbase target)
		{
			//  validate arguments
			if (@this == null)
				throw new ArgumentNullException("this");
			if (target == null)
				throw new ArgumentNullException("target");
			return @this.Where(candidate => candidate.IsTo(target));
		}
		/// <summary>
		/// Filters <paramref name="this"/> to only include links of type <paramref name="definition"/>.
		/// </summary>
		/// <param name="this">The <see cref="Link"/>s which to filter.</param>
		/// <param name="definition">The <see cref="LinkDefinition"/>.</param>
		/// <returns>Returns the filtered <see cref="Link"/>s.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static IEnumerable<Link> OfType(this IEnumerable<Link> @this, LinkDefinition definition)
		{
			//  validate arguments
			if (@this == null)
				throw new ArgumentNullException("this");
			if (definition == null)
				throw new ArgumentNullException("definition");
			return @this.Where(candidate => candidate.IsInstanceOf(definition));
		}
	}
}