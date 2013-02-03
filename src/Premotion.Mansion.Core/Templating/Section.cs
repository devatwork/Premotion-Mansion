using System;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Represents a single template section.
	/// </summary>
	public class Section : ISection
	{
		#region Constructors
		/// <summary>
		/// Constructs an section.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The descriptor of this section.</param>
		/// <param name="expression">The expressio of this section.</param>
		public Section(IMansionContext context, IPropertyBag properties, IExpressionScript expression)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");
			if (expression == null)
				throw new ArgumentNullException("expression");

			// set values
			Properties = properties;
			Expression = expression;
			Id = Guid.NewGuid().ToString();
			Name = Properties.Get<string>(context, "name");
			ShouldBeRenderedOnce = !Properties.Get(context, "repeatable", true);
			TargetField = Properties.Get(context, "field", Name);

			// check if there is a requires property
			string requiresExpressionString;
			if (Properties.TryGet(context, "requires", out requiresExpressionString))
			{
				// assemble the expression
				var expressionService = context.Nucleus.ResolveSingle<IExpressionScriptService>();
				var requiresExpression = expressionService.Parse(context, new LiteralResource(requiresExpressionString));

				// execute the expression
				areRequirementsSatisfied = requiresExpression.Execute<bool>;
			}
			else
				areRequirementsSatisfied = mansionContext => true;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the header descriptor for this section.
		/// </summary>
		private IPropertyBag Properties { get; set; }
		/// <summary>
		/// Gets the <see cref="ITemplate"/> to which this <see cref="ISection"/> belongs.
		/// </summary>
		public ITemplate Template { get; set; }
		/// <summary>
		/// Gets the unique identifier of this section.
		/// </summary>
		public string Id { get; private set; }
		/// <summary>
		/// Gets the expression for this section.
		/// </summary>
		public IExpressionScript Expression { get; private set; }
		/// <summary>
		/// Gets the name of this section.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the target field to which this section is rendered.
		/// </summary>
		public string TargetField { get; private set; }
		/// <summary>
		/// Checks whether this section is required or not.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns true when this section is required, otherwise false.</returns>
		public bool AreRequirementsSatified(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return areRequirementsSatisfied(context);
		}
		/// <summary>
		/// Gets a flag indicating whether this section should be rendered only once..
		/// </summary>
		/// <returns>Returns true if this section should be rendered once, otherwise false.</returns>
		public bool ShouldBeRenderedOnce { get; private set; }
		#endregion
		#region Private Fields
		private readonly Func<IMansionContext, bool> areRequirementsSatisfied;
		#endregion
	}
}