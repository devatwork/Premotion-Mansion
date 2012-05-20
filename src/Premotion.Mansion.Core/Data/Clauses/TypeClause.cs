using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the query part for types.
	/// </summary>
	public class TypeClause : NodeQueryClause, ITypeSpecifierClause
	{
		#region Nested type: TypeClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "TypeClause" />.
		/// </summary>
		public class TypeClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public TypeClauseInterpreter(ITypeService typeService) : base(10)
			{
				// validate arguments
				if (typeService == null)
					throw new ArgumentNullException("typeService");

				// set values
				this.typeService = typeService;
			}
			#endregion
			#region Interpret Methods
			/// <summary>
			/// 	Interprets the input.
			/// </summary>
			/// <param name = "context">The <see cref = "IMansionContext" />.</param>
			/// <param name = "input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(IMansionContext context, IPropertyBag input)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (input == null)
					throw new ArgumentNullException("input");

				string types;
				if (!input.TryGetAndRemove(context, "type", out types) || string.IsNullOrEmpty(types))
					yield break;

				// get the type
				yield return new TypeClause(types.Split(',').Select(x => typeService.Load(context, x)));
			}
			#endregion
			#region Private Fields
			private readonly ITypeService typeService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs this query part with the specified type.
		/// </summary>
		/// <param name = "types">The types.</param>
		public TypeClause(IEnumerable<ITypeDefinition> types)
		{
			// validate arguments
			if (types == null)
				throw new ArgumentNullException("types");

			// set value
			Types = types.ToArray();
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the base <see cref = "ITypeDefinition" />s.
		/// </summary>
		public IEnumerable<ITypeDefinition> Types { get; private set; }
		#endregion
		#region Overrides of Object
		/// <summary>
		/// 	Returns a <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </summary>
		/// <returns>
		/// 	A <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("type:{0}", string.Join(",", Types.Select(x => x.Name)));
		}
		#endregion
	}
}