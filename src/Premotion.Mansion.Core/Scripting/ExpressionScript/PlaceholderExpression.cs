using System;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements a property expression.
	/// </summary>
	public class PlaceholderExpression : PhraseExpression
	{
		#region Constructors
		/// <summary>
		/// Constructs a placeholder expression.
		/// </summary>
		/// <param name="name"></param>
		public PlaceholderExpression(string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set values
			Name = name;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Evaluates this expression.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the result of the evaluation.</returns>
		public override TTarget Execute<TTarget>(MansionContext context)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");

			// do nothing
			return context.Nucleus.Get<IConversionService>(context).Convert<TTarget>(context, Content);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this placeholder.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets/Sets the content of this placeholder.
		/// </summary>
		public object Content { get; set; }
		#endregion
	}
}