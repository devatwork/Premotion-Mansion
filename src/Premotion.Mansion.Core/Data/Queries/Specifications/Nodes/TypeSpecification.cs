using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Queries.Specifications.Nodes
{
	/// <summary>
	/// Specifies <see cref="ITypeDefinition"/>.
	/// </summary>
	public class TypeSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs this query part with the specified types.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="types"/> is empty.</exception>
		private TypeSpecification(IEnumerable<ITypeDefinition> types)
		{
			// validate arguments
			if (types == null)
				throw new ArgumentNullException("types");

			// guard against empty types
			var typeArray = types.ToArray();
			if (typeArray.Length == 0)
				throw new InvalidOperationException("The type specification should at least include one type");

			// set value
			Types = typeArray;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs a <see cref="TypeSpecification"/> specification matching the given <paramref name="typeNames"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeNames">The type name string.</param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="typeNames"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="typeNames"/> does not contain a type name.</exception>
		public static TypeSpecification Is(IMansionContext context, string typeNames)
		{
			// validate argumetns
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeNames))
				throw new ArgumentNullException("typeNames");

			// get the type service
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();

			// get the types
			var types = typeNames.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(typeName => typeService.Load(context, typeName.Trim()));

			// return the specification
			return IsAny(types);
		}
		/// <summary>
		/// Constructs a <see cref="TypeSpecification"/> specification matching the given <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The <see cref="ITypeDefinition"/></param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="type"/> is empty.</exception>
		public static TypeSpecification Is(ITypeDefinition type)
		{
			return IsAny(type);
		}
		/// <summary>
		/// Constructs a <see cref="TypeSpecification"/> specification matching any of the given <paramref name="types"/>.
		/// </summary>
		/// <param name="types">The <see cref="ITypeDefinition"/>s.</param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="types"/> is empty.</exception>
		public static TypeSpecification IsAny(params ITypeDefinition[] types)
		{
			return new TypeSpecification(types);
		}
		/// <summary>
		/// Constructs a <see cref="TypeSpecification"/> specification matching any of the given <paramref name="types"/>.
		/// </summary>
		/// <param name="types">The <see cref="ITypeDefinition"/>s.</param>
		/// <returns>Returns the created specification.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="types"/> is empty.</exception>
		public static TypeSpecification IsAny(IEnumerable<ITypeDefinition> types)
		{
			// validate arguments
			if (types == null)
				throw new ArgumentNullException("types");

			// create the specification
			return new TypeSpecification(types);
		}
		#endregion
		#region Overrides of Specification
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected override void DoAsString(StringBuilder builder)
		{
			builder.Append("type=").Append(string.Join(",", Types.Select(x => x.Name)));
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the <see cref = "ITypeDefinition" />s.
		/// </summary>
		public IEnumerable<ITypeDefinition> Types { get; private set; }
		#endregion
	}
}