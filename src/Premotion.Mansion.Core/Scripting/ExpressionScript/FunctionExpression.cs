using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements a function expression.
	/// </summary>
	public abstract class FunctionExpression : PhraseExpression
	{
		#region ExecuteFunction Delegate
		/// <summary>
		/// executes this function.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the result.</returns>
		public delegate object ExecuteFunction(IMansionContext context);
		#endregion
		#region Initialize Methods
		/// <summary>
		/// Initializes this script funtion.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="argumentExpressions">The arguments expression.</param>
		public virtual void Initialize(IMansionContext context, ICollection<IExpressionScript> argumentExpressions)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");
			if (argumentExpressions == null)
				throw new ArgumentNullException("argumentExpressions");

			// store the argument expressions
			ArgumentExpressions = argumentExpressions;

			// get candidate, parameters + 1 = first parameters is context
			var methodInfo = FindEvaluateMethod(argumentExpressions);

			// build a dynamic method
			var dynamicMethod = new DynamicMethod(string.Empty, typeof (string), new[] {GetType(), typeof (IMansionContext)}, GetType());
			var generator = dynamicMethod.GetILGenerator();

			// push the 'this' and context instance to te stack
			generator.Emit(OpCodes.Ldarg_0); // loads this pointer
			generator.Emit(OpCodes.Ldarg_1); // loads the context instance

			// get the NotRemovable from which to get the argument expressions
			var argumentExpressionsField = GetType().GetField("ArgumentExpressions", BindingFlags.Instance | BindingFlags.NonPublic);
			if (argumentExpressionsField == null)
				throw new InvalidOperationException(string.Format("Could not find field ArgumentExpressions in type {0}", GetType()));

			// get the method which get the expression from the expression
			var getExpressionFromField = ArgumentExpressions.GetType().GetProperty("Item").GetGetMethod();

			// loop through all the arguments
			var parameterInfos = methodInfo.GetParameters().Skip(1).ToList();
			for (var parameterIndex = 0; parameterIndex < parameterInfos.Count; parameterIndex++)
			{
				var parameterInfo = parameterInfos[parameterIndex];

				// if the last parameter is an array and there are more argument expressions it is an varargs argument
				if (parameterIndex == (parameterInfos.Count - 1) && parameterInfo.ParameterType.IsArray)
				{
					// get the element type of the array
					var elementType = parameterInfo.ParameterType.GetElementType();

					// declare a local variable in which to store the array
					var paramsArrayLocalVarIndex = generator.DeclareLocal(parameterInfo.ParameterType).LocalIndex;

					// create the array and store it in loc 0 so it can be loaded
					var remainingArgumentExpressionCount = argumentExpressions.Count - (parameterInfos.Count - 1);
					generator.Emit(OpCodes.Ldc_I4_S, remainingArgumentExpressionCount);
					generator.Emit(OpCodes.Newarr, elementType);
					generator.Emit(OpCodes.Stloc, paramsArrayLocalVarIndex);

					// if there are expressions to execute
					if (remainingArgumentExpressionCount > 0)
					{
						// store the the result of the execution temporarily
						var varargsItemLocalVarIndex = generator.DeclareLocal(elementType).LocalIndex;

						// loop over the remaining parameters to store them in the array
						for (var argumentExpressionIndex = 0; argumentExpressionIndex < remainingArgumentExpressionCount; argumentExpressionIndex++)
						{
							// load the expression from the argument expression NotRemovable
							generator.Emit(OpCodes.Ldarg_0); // loads this pointer
							generator.Emit(OpCodes.Ldfld, argumentExpressionsField); // load the phrase NotRemovable
							generator.Emit(OpCodes.Ldc_I4, (parameterIndex + argumentExpressionIndex)); // push argument index to the stack
							generator.Emit(OpCodes.Callvirt, getExpressionFromField);

							// get the expression evalution method
							var expressionEvaluateMethod = typeof (IScript).GetMethods().Single(candidate => candidate.Name.Equals("Execute") && candidate.IsGenericMethod).MakeGenericMethod(elementType);

							// execute the expression
							generator.Emit(OpCodes.Ldarg_1); // load the context reference
							BakeMethodCall(expressionEvaluateMethod, generator); // execute the phrase evaluation function
							generator.Emit(OpCodes.Stloc, varargsItemLocalVarIndex); // store the result of the argument expression execution for later use

							// store the value in the parameter array
							generator.Emit(OpCodes.Ldloc, paramsArrayLocalVarIndex); // push array to eval stack
							generator.Emit(OpCodes.Ldc_I4, argumentExpressionIndex); // push array index to eval stack
							generator.Emit(OpCodes.Ldloc, varargsItemLocalVarIndex); // push argument expression execution value to stack
							generator.Emit(OpCodes.Stelem, elementType); // store the value in the array at the specified index
						}
					}

					//  push the array as the final parameter
					generator.Emit(OpCodes.Ldloc, paramsArrayLocalVarIndex);
				}
				else
				{
					// load the expression from the argument expression NotRemovable
					generator.Emit(OpCodes.Ldarg_0); // loads this pointer
					generator.Emit(OpCodes.Ldfld, argumentExpressionsField); // load the argument expresison field
					generator.Emit(OpCodes.Ldc_I4, parameterIndex); // push argument index to the stack
					generator.Emit(OpCodes.Callvirt, getExpressionFromField);

					// get the expression evalution method
					var expressionEvaluateMethod = typeof (IScript).GetMethods().Where(candidate => candidate.Name.Equals("Execute") && candidate.IsGenericMethod).Single().MakeGenericMethod(parameterInfo.ParameterType);

					// execute the expression
					generator.Emit(OpCodes.Ldarg_1); // load the context reference
					BakeMethodCall(expressionEvaluateMethod, generator); // execute the phrase evaluation function
				}
			}

			// bake the method call
			BakeMethodCall(methodInfo, generator);

			// bake the result processing
			BakeMethodResultToObject(methodInfo, generator);

			// bake the method return
			generator.Emit(OpCodes.Ret);

			// build the execution method
			executeFunction = (ExecuteFunction) dynamicMethod.CreateDelegate(typeof (ExecuteFunction), this);
		}
		/// <summary>
		/// Finds the evaluate method matching best.
		/// </summary>
		/// <param name="argumentExpressions">The arguments.</param>
		/// <returns>Returns the <see cref="MethodInfo"/> which to call.</returns>
		private MethodInfo FindEvaluateMethod(ICollection<IExpressionScript> argumentExpressions)
		{
			// find all candidates
			var weightedCandidates = (GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(candidate => "Evaluate".Equals(candidate.Name, StringComparison.OrdinalIgnoreCase))).Select(x => new
			                                                                                                                                                                                                                               {
			                                                                                                                                                                                                                               	Weight = DetermineMethodWeight(x, argumentExpressions),
			                                                                                                                                                                                                                               	Method = x
			                                                                                                                                                                                                                               }).ToList();
			if (weightedCandidates.Count == 0)
				throw new InvalidOperationException(string.Format("Could not find any Evaluate methods on the script function '{0}'", GetType()));

			// determine the max weight
			var maxWeight = weightedCandidates.Max(x => x.Weight);
			if (maxWeight == -1)
				throw new InvalidOperationException(string.Format("No suitable Evaluate methods found to execute on script function '{0}'", GetType()));

			// select only methods with maximum score
			var heaviestMethods = weightedCandidates.Where(x => x.Weight == maxWeight).ToList();

			// check for ambigious methods
			if (heaviestMethods.Count != 1)
				throw new InvalidOperationException(string.Format("Ambigious Evaluate method detected on the script function '{0}'", GetType()));

			return heaviestMethods[0].Method;
		}
		/// <summary>
		/// Determines the weigth of a method.
		/// </summary>
		/// <param name="candidate">The candidate <see cref="MethodInfo"/>.</param>
		/// <param name="argumentExpressions">The expressions of the arguments passed to the <paramref name="candidate"/>.</param>
		/// <returns>Returns the weight given to the method.</returns>
		private static int DetermineMethodWeight(MethodInfo candidate, ICollection<IExpressionScript> argumentExpressions)
		{
			var weight = -1;
			var candidateParameters = candidate.GetParameters();

			// if the number of parameters is equal to the number of argument expressions plus the context argument it is a very good match
			if (candidateParameters.Length == (argumentExpressions.Count + 1))
				weight += 10;

			// if the last parameter is a variable parameter it is a average match
			if (candidateParameters.Length > 1 && candidateParameters[candidateParameters.Length - 1].ParameterType.IsArray)
				weight += 5;

			return weight;
		}
		private static void BakeMethodResultToObject(MethodInfo methodInfo, ILGenerator generator)
		{
			if (methodInfo.ReturnType == typeof (void))
				generator.Emit(OpCodes.Ldnull);
			else if (methodInfo.ReturnType.IsValueType)
				generator.Emit(OpCodes.Box, methodInfo.ReturnType);
		}
		private static void BakeMethodCall(MethodInfo methodInfo, ILGenerator generator)
		{
			generator.Emit(methodInfo.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, methodInfo);
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Evaluates this expression.
		/// </summary>
		/// <typeparam name="TTarget">The target type.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the result of the evaluation.</returns>
		public override TTarget Execute<TTarget>(IMansionContext context)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");

			return context.Nucleus.ResolveSingle<IConversionService>().Convert<TTarget>(context, executeFunction(context));
		}
		#endregion
		#region Private Fields
		/// <summary>
		/// Gets the argument expressions.
		/// </summary>
		protected ICollection<IExpressionScript> ArgumentExpressions;
		private ExecuteFunction executeFunction;
		#endregion
	}
}