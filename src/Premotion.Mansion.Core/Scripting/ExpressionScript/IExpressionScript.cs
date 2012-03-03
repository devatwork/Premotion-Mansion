using System.Collections.Generic;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Represents an executable script expression.
	/// </summary>
	public interface IExpressionScript : IScript
	{
		#region Properties
		/// <summary>
		/// Gets the expressions which make up this expression.
		/// </summary>
		IEnumerable<IExpressionScript> Expressions { get; }
		#endregion
	}
}