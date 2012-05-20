using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Represents a child of node clause.
	/// </summary>
	public class ChildOfClause : NodeQueryClause
	{
		#region Nested type: ChildOfClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "ChildOfClause" />.
		/// </summary>
		public class ChildOfClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public ChildOfClauseInterpreter(IConversionService conversionService) : base(10)
			{
				// validate arguments
				if (conversionService == null)
					throw new ArgumentNullException("conversionService");

				// set values
				this.conversionService = conversionService;
			}
			#endregion
			#region Interpret Methods
			/// <summary>
			/// 	Interprets the input.
			/// </summary>
			/// <param name = "context">The <see cref = "IMansionContext" />.</param>
			/// <param name = "input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(IMansionContext context, IPropertyBag input)
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
				                                			depth = conversionService.Convert(context, depthString, 1);
				                                		}
				                                	}
				                                	return depth;
				                                });

				// check for parentPointer
				NodePointer parentPointer;
				if (input.TryGetAndRemove(context, "parentPointer", out parentPointer))
				{
					yield return new ChildOfClause(parentPointer, depthValue.Value);
					clauseCounter++;
				}

				// check for pointer
				Node parentNode;
				if (input.TryGetAndRemove(context, "parentSource", out parentNode))
				{
					// create the clause
					yield return new ChildOfClause(parentNode.Pointer, depthValue.Value);
					clauseCounter++;
				}

				// check for ambigous clauses
				if (clauseCounter > 1)
					throw new InvalidOperationException("Detected an ambigious parent of clause. Remove either parentPointer or parentSource.");
			}
			#endregion
			#region Private Fields
			private readonly IConversionService conversionService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs an instance of the child of query part. Selects the children at the specified depth.
		/// </summary>
		/// <param name = "parentPointer">The pointer of the parent.</param>
		/// <param name = "depth">The depth from which to select.</param>
		public ChildOfClause(NodePointer parentPointer, int? depth = 1)
		{
			// validate argument
			if (parentPointer == null)
				throw new ArgumentNullException("parentPointer");

			// set values
			ParentPointer = parentPointer;
			Depth = depth;
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the pointer to the parent from which to select the children.
		/// </summary>
		public NodePointer ParentPointer { get; private set; }
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
			return string.Format("child-of:{0}&depth:{1}", ParentPointer, Depth);
		}
		#endregion
	}
}