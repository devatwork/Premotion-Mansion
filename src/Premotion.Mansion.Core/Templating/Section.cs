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
		/// <param name="properties">The descriptor of this section.</param>
		/// <param name="expression">The expressio of this section.</param>
		public Section(IPropertyBag properties, IExpressionScript expression)
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");
			if (expression == null)
				throw new ArgumentNullException("expression");

			// set values
			Properties = properties;
			Expression = expression;
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
		/// Gets the expression for this section.
		/// </summary>
		public IExpressionScript Expression { get; private set; }
		/// <summary>
		/// Gets the name of this section.
		/// </summary>
		/// <summary>
		/// Gets the name of this section.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		public string GetName(IContext context)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");

			// return the name
			return Properties.Get<string>(context, "name");
		}
		/// <summary>
		/// Gets the target field to which this section is rendered.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		public string GetTargetField(IContext context)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");

			// return the name
			return Properties.Get(context, "field", GetName(context));
		}
		/// <summary>
		/// Checks whether this section is required or not.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns true when this section is required, otherwise false.</returns>
		public bool AreRequirementsSatified(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if there is a requires property
			string requiresExpressionString;
			if (!Properties.TryGet(context, "requires", out requiresExpressionString))
				return true;

			// assemble the expression
			var expressionService = context.Nucleus.Get<IExpressionScriptService>(context);
			var requiresExpression = expressionService.Parse(context, new LiteralResource(requiresExpressionString));

			// execute the expression
			return requiresExpression.Execute<bool>(context);
		}
		#endregion
	}
}