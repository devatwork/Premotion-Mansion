﻿using System;
using System.Globalization;
using System.Text;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptFunctions
{
	/// <summary>
	/// Invokes a procedure and returns it output.
	/// </summary>
	[ScriptFunction("InvokeProcedure")]
	public class InvokeProcedure : FunctionExpression
	{
		#region Evaluate Methods
		/// <summary>
		/// Invokes a procedure and returns it output.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="procedureName">The name of the section which to render.</param>
		/// <param name="arguments">The arguments of the procedure.</param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string procedureName, params object[] arguments)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(procedureName))
				throw new ArgumentNullException("procedureName");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// get the procedure
			ScriptTag procedure;
			if (!context.ProcedureStack.TryPeek(procedureName, out procedure))
				throw new InvalidOperationException(string.Format("No procedure found with name '{0}'", procedureName));

			// assemble the arguments
			var procedureArguments = new PropertyBag();
			for (var index = 0; index < arguments.Length; ++index)
				procedureArguments.Set(index.ToString(CultureInfo.InvariantCulture), arguments[index]);

			// render the control
			var buffer = new StringBuilder();
			using (var pipe = new StringOutputPipe(buffer))
			using (context.OutputPipeStack.Push(pipe))
			using (context.Stack.Push("Arguments", procedureArguments))
				procedure.Execute(context);

			// make sure the break procedure flag is cleared
			context.BreakTopMostProcedure = false;

			// return the buffer
			return buffer.ToString();
		}
		#endregion
	}
}