using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries.Converters;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Represents an executable select command.
	/// </summary>
	public class SelectNodeCommand : SelectCommand<Nodeset, Node>
	{
		#region Constructors
		/// <summary>
		/// Constructs an <see cref="SelectCommand"/>.
		/// </summary>
		/// <param name="converters">The <see cref="IQueryComponentConverter"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="converters"/> is null.</exception>
		public SelectNodeCommand(IEnumerable<IQueryComponentConverter> converters) : base(converters)
		{
		}
		#endregion
		#region Overrides of SelectCommand<Dataset,IPropertyBag>
		/// <summary>
		/// Creates a set.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="rows">The rows.</param>
		/// <param name="setProperties">The properties of the set.</param>
		/// <returns>Returns the created set.</returns>
		protected override Nodeset CreateSet(IMansionContext context, IEnumerable<Node> rows, IPropertyBag setProperties)
		{
			return new Nodeset(context, rows, setProperties);
		}
		/// <summary>
		/// Creates a new row.
		/// </summary>
		/// <returns>Returns the created row.</returns>
		protected override Node CreateRow()
		{
			return new Node();
		}
		/// <summary>
		/// Initializes the given row.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="row">The <see cref="Node"/> which to intialize.</param>
		protected override void Initialize(IMansionContext context, Node row)
		{
			row.Initialize(context);
		}
		#endregion
	}
}