using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Specifies the parent which to select.
	/// </summary>
	public class ParentOfClause : NodeQueryClause
	{
		#region Nested type: ParentOfClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "ParentOfClause" />.
		/// </summary>
		public class ParentOfClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public ParentOfClauseInterpreter() : base(10)
			{
			}
			#endregion
			#region Interpret Methods
			/// <summary>
			/// 	Interprets the input.
			/// </summary>
			/// <param name = "context">The <see cref = "MansionContext" />.</param>
			/// <param name = "input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(MansionContext context, IPropertyBag input)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (input == null)
					throw new ArgumentNullException("input");

				var clauseCounter = 0;

				// get the depth
				var depthValue = new Lazy<int?>(() =>
				                                {
				                                	int? depth = 1;
				                                	string depthString;
				                                	if (input.TryGetAndRemove(context, "depth", out depthString))
				                                	{
				                                		// check for any
				                                		if ("any".Equals(depthString, StringComparison.OrdinalIgnoreCase))
				                                			depth = null;
				                                		else
				                                		{
				                                			// parse the depth
				                                			var conversionService = context.Nucleus.Get<IConversionService>(context);
				                                			depth = conversionService.Convert(context, depthString, 1);
				                                		}
				                                	}
				                                	return depth;
				                                });

				// check for parentPointer
				NodePointer childPointer;
				if (input.TryGetAndRemove(context, "childPointer", out childPointer))
				{
					yield return new ParentOfClause(childPointer, depthValue.Value);
					clauseCounter++;
				}

				// check for pointer
				Node childNode;
				if (input.TryGetAndRemove(context, "childSource", out childNode))
				{
					// create the clause
					yield return new ParentOfClause(childNode.Pointer, depthValue.Value);
					clauseCounter++;
				}

				// check for ambigous clauses
				if (clauseCounter > 1)
					throw new InvalidOperationException("Detected an ambigious parent of clause. Remove either childPointer or childSource.");
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs an instance of the parent of query part. Selects the parent above the child.
		/// </summary>
		/// <param name = "childPointer">The pointer of the child.</param>
		/// <param name = "depth">The depth of the parent.</param>
		public ParentOfClause(NodePointer childPointer, int? depth)
		{
			// validate argument
			if (childPointer == null)
				throw new ArgumentNullException("");

			// set values
			ChildPointer = childPointer;
			Depth = depth;
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the pointer to the child from which to select the parent.
		/// </summary>
		public NodePointer ChildPointer { get; private set; }
		/// <summary>
		/// 	Get the depth of the childeren.
		/// 	Positive values are relative to the parent.
		/// 	Negative values are relative from the root.
		/// 	When value is null there is no depth specification.
		/// </summary>
		public int? Depth { get; private set; }
		#endregion
		#region Overrides of Object
		/// <summary>
		/// 	Returns a <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </summary>
		/// <returns>
		/// 	A <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("parent-of:{0}&depth:{1}", ChildPointer, Depth);
		}
		#endregion
	}
}