using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the status clause.
	/// </summary>
	public class StatusClause : NodeQueryClause
	{
		#region Nested type: StatusClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "StatusClause" />.
		/// </summary>
		public class StatusClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public StatusClauseInterpreter() : base(10)
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

				string status;
				if (input.TryGetAndRemove(context, "status", out status) && !string.IsNullOrEmpty(status))
					yield return new StatusClause(status);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs a status clause.
		/// </summary>
		/// <param name = "status">The status.</param>
		public StatusClause(string status)
		{
			// validate arguments
			if (string.IsNullOrEmpty(status))
				throw new ArgumentNullException("status");

			foreach (var statusPart in status.Split(new[] {','}).Select(x => x.Trim().ToLower()))
			{
				switch (statusPart)
				{
					case "any":
					{
						Status |= NodeStatus.Any;
					}
						break;
					case "draft":
					{
						Status |= NodeStatus.Draft;
					}
						break;
					case "staged":
					{
						Status |= NodeStatus.Staged;
					}
						break;
					case "published":
					{
						Status |= NodeStatus.Published;
					}
						break;
					case "expired":
					{
						Status |= NodeStatus.Expired;
					}
						break;
					case "archived":
					{
						Status |= NodeStatus.Archived;
					}
						break;
					default:
						throw new InvalidOperationException(string.Format("'{0}' is an unknown node status", statusPart));
				}
			}
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the status a node should have.
		/// </summary>
		public NodeStatus Status { get; private set; }
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
			return string.Format("status:{0}", Status);
		}
		#endregion
	}
}