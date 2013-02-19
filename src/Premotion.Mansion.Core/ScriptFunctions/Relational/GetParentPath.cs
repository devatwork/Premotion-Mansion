using System;
using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Relational
{
	/// <summary>
	/// Gets the path of a given node.
	/// </summary>
	[ScriptFunction("GetParentPath")]
	public class GetParentPath : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The <see cref="NodePointer"/>.</param>
		/// <returns>Returns the path.</returns>
		public string Evaluate(IMansionContext context, NodePointer pointer)
		{
			return Evaluate(context, pointer, 0);
		}
		/// <summary>
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The <see cref="NodePointer"/>.</param>
		/// <param name="depth">The depth from which to get the path.</param>
		/// <returns>Returns the path.</returns>
		public string Evaluate(IMansionContext context, NodePointer pointer, int depth)
		{
			return Evaluate(context, pointer, depth, " / ");
		}
		/// <summary>
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The <see cref="NodePointer"/>.</param>
		/// <param name="depth">The depth from which to get the path.</param>
		/// <param name="separator">The separator to use.</param>
		/// <returns>Returns the path.</returns>
		public string Evaluate(IMansionContext context, NodePointer pointer, int depth, string separator)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			return string.Join(separator, pointer.Path.Skip(depth).TakeWhile((x, y) => y != (pointer.Depth - depth - 1)));
		}
	}
}