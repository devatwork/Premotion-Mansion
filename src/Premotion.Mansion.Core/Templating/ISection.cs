using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Represents a section in a template.
	/// </summary>
	public interface ISection
	{
		#region Properties
		/// <summary>
		/// Gets the expression for this section.
		/// </summary>
		IExpressionScript Expression { get; }
		/// <summary>
		/// Gets the <see cref="ITemplate"/> to which this <see cref="ISection"/> belongs.
		/// </summary>
		ITemplate Template { get; }
		/// <summary>
		/// Gets the unique identifier of this section.
		/// </summary>
		string Id { get; }
		/// <summary>
		/// Gets the name of this section.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Gets the target field to which this section is rendered.
		/// </summary>
		string TargetField { get; }
		/// <summary>
		/// Gets a flag indicating whether this section should be rendered only once..
		/// </summary>
		/// <returns>Returns true if this section should be rendered once, otherwise false.</returns>
		bool ShouldBeRenderedOnce { get; }
		/// <summary>
		/// Checks whether this section is required or not.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns true when this section is required, otherwise false.</returns>
		bool AreRequirementsSatified(IMansionContext context);
		#endregion
	}
}