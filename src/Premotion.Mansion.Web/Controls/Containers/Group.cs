using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Containers
{
	/// <summary>
	/// Represents a group of <see cref="Control"/>s.
	/// </summary>
	public class Group : Container
	{
		#region Nested type: GroupFactoryTag
		/// <summary>
		/// Constructs <see cref="Group"/>s.
		/// </summary>
		[Named(Constants.ControlTagNamespaceUri, "group")]
		public class GroupFactoryTag : ControlFactoryTag<Group>
		{
			#region Overrides of ControlFactoryTag<Group>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Group Create(MansionWebContext context, ControlDefinition definition)
			{
				return new Group(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Group(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}