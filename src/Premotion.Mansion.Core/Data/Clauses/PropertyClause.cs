using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the sql where clause.
	/// </summary>
	public class PropertyClause : NodeQueryClause
	{
		#region Nested type: PropertyClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "PropertyClause" />.
		/// </summary>
		public class PropertyClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public PropertyClauseInterpreter() : base(100)
			{
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

				// loop through all the remaining properties
				foreach (var propertyName in input.Names)
				{
					string values;
					if (!input.TryGet(context, propertyName, out values) || string.IsNullOrWhiteSpace(values))
						continue;
					yield return new PropertyClause(propertyName, values);
				}
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs a property clause.
		/// </summary>
		/// <param name = "property">The name of the property.</param>
		/// <param name = "values">The values of the property.</param>
		public PropertyClause(string property, string values)
		{
			// validate arguments
			if (string.IsNullOrEmpty(property))
				throw new ArgumentNullException("property");

			// set values
			Property = property;
			Values = (values ?? string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the name of the property.
		/// </summary>
		public string Property { get; private set; }
		/// <summary>
		/// 	Gets the values.
		/// </summary>
		public string[] Values { get; private set; }
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
			return string.Format("property-{0}:({1})", Property, string.Join(",", Values.Select(x => x.ToString())));
		}
		#endregion
	}
}