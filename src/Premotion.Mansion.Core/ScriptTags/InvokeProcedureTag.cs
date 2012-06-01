using System;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags
{
	/// <summary>
	/// Calls a procedure.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "invokeProcedure")]
	public class InvokeProcedureTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get all the attributes
			var procedureArguments = GetAttributes(context);

			// get the name of the procedure
			string procedureName;
			if (!procedureArguments.TryGetAndRemove(context, "procedureName", out procedureName) || string.IsNullOrEmpty(procedureName))
				throw new InvalidOperationException("The attribute procedureName must contain a valide procedure name.");
			bool checkExists;
			if (!procedureArguments.TryGetAndRemove(context, "checkExists", out checkExists))
				checkExists = true;

			// find the procedure
			IScript procedure;
			if (!context.ProcedureStack.TryPeek(procedureName, out procedure))
			{
				if (checkExists)
					throw new InvalidOperationException(string.Format("No procedure found with name '{0}'", procedureName));
				return;
			}

			// invoke the procedure
			using (context.Stack.Push("Arguments", procedureArguments, false))
				procedure.Execute(context);

			// make sure the break procedure flag is cleared
			context.BreakTopMostProcedure = false;
		}
	}
}