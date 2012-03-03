using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Specified the base type from which nodes must derive in order to satisfy this clause.
	/// </summary>
	public class BaseTypeClause : NodeQueryClause, ITypeSpecifierClause
	{
		#region Nested type: BaseTypeClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "BaseTypeClause" />.
		/// </summary>
		public class BaseTypeClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public BaseTypeClauseInterpreter() : base(10)
			{
			}
			#endregion
			#region Interpret Methods
			/// <summary>
			/// 	Interprets the input.
			/// </summary>
			/// <param name = "context">The <see cref = "MansionContext" />.</param>
			/// <param name = "input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(MansionContext context, IPropertyBag input)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (input == null)
					throw new ArgumentNullException("input");

				string baseTypes;
				if (!input.TryGetAndRemove(context, "baseType", out baseTypes) || string.IsNullOrEmpty(baseTypes))
					yield break;

				// return the clause
				var typeService = context.Nucleus.Get<ITypeService>(context);
				yield return new BaseTypeClause(baseTypes.Split(',').Select(x => typeService.Load(context, x)));
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs this query part with the specified base type.
		/// </summary>
		/// <param name = "types">The types.</param>
		public BaseTypeClause(IEnumerable<ITypeDefinition> types)
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
			return string.Format("base-type={0}", string.Join(",", Types.Select(x => x.Name)));
		}
		#endregion
	}
}