using System;
using System.Linq;
using System.Text;

namespace Premotion.Mansion.Core.Data.Specifications.Node
{
	/// <summary>
	/// Specifies <see cref="NodeStatus"/>.
	/// </summary>
	public class StatusSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs this specification.
		/// </summary>
		/// <param name="status">The <see cref="NodeStatus"/> a <see cref="Node"/> should have in order to match this specification.</param>
		private StatusSpecification(NodeStatus status)
		{
			Status = status;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs a <see cref="StatusSpecification"/> specification matching the given <paramref name="status"/>.
		/// </summary>
		/// <param name="status">The <see cref="NodeStatus"/>.</param>
		/// <returns>Returns the created specification.</returns>
		public static StatusSpecification Is(string status)
		{
			// validate argumetns
			if (string.IsNullOrEmpty(status))
				throw new ArgumentNullException("status");

			var nodeStatus = (NodeStatus) 0;
			foreach (var statusPart in status.Split(new[] {','}).Select(x => x.Trim().ToLower()))
			{
				switch (statusPart)
				{
					case "any":
					{
						nodeStatus |= NodeStatus.Any;
					}
						break;
					case "draft":
					{
						nodeStatus |= NodeStatus.Draft;
					}
						break;
					case "staged":
					{
						nodeStatus |= NodeStatus.Staged;
					}
						break;
					case "published":
					{
						nodeStatus |= NodeStatus.Published;
					}
						break;
					case "expired":
					{
						nodeStatus |= NodeStatus.Expired;
					}
						break;
					case "archived":
					{
						nodeStatus |= NodeStatus.Archived;
					}
						break;
					default:
						throw new InvalidOperationException(string.Format("'{0}' is an unknown node status", statusPart));
				}
			}

			// return the specification
			return Is(nodeStatus);
		}
		/// <summary>
		/// Constructs a <see cref="StatusSpecification"/> specification matching the given <paramref name="status"/>.
		/// </summary>
		/// <param name="status">The <see cref="NodeStatus"/>.</param>
		/// <returns>Returns the created specification.</returns>
		public static StatusSpecification Is(NodeStatus status)
		{
			return new StatusSpecification(status);
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
			builder.Append("status=").Append(Status);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="NodeStatus"/> a <see cref="Node"/> should have in order to match this specification.
		/// </summary>
		public NodeStatus Status { get; private set; }
		#endregion
	}
}