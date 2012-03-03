using System.Collections.Generic;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Defines the interface for <see cref="IExpressionScript"/> services.
	/// </summary>
	public interface IExpressionScriptService : IScriptingService<IExpressionScript>
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="ExpressionPartInterpreter"/>s registered with this expression script service.
		/// </summary>
		IEnumerable<ExpressionPartInterpreter> Interpreters { get; }
		#endregion
	}
}