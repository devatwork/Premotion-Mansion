using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Data.Specifications.Node;

namespace Premotion.Mansion.Core.Data.Parser
{
	/// <summary>
	/// Implements the parent of query argument processor.
	/// </summary>
	public class ParentOfQueryArgumentProcessor : QueryArgumentProcessor
	{
		#region Constructors
		/// <summary>
		/// Constructs this processor.
		/// </summary>
		public ParentOfQueryArgumentProcessor(IConversionService conversionService) : base(200)
		{
			// validate arguments
			if (conversionService == null)
				throw new ArgumentNullException("conversionService");

			// set values
			this.conversionService = conversionService;
		}
		#endregion
		#region Overrides of QueryArgumentProcessor
		/// <summary>
		/// Processes the <paramref name="parameters"/> and turn them into <paramref name="query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parameters">The parameters which to process.</param>
		/// <param name="query">The <see cref="Query"/> in which to set the parameters.</param>
		protected override void DoProcess(IMansionContext context, IPropertyBag parameters, Query query)
		{
			var clauseCounter = 0;

			// get the depth
			var depthValue = new Lazy<int?>(() =>
			                                {
			                                	int? depth = 1;
			                                	string depthString;
			                                	if (parameters.TryGetAndRemove(context, "depth", out depthString))
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
			if (parameters.TryGetAndRemove(context, "childPointer", out parentPointer))
			{
				query.Add(ParentOfSpecification.Child(parentPointer, depthValue.Value));
				clauseCounter++;
			}

			// check for pointer
			Node parentNode;
			if (parameters.TryGetAndRemove(context, "childSource", out parentNode))
			{
				query.Add(ParentOfSpecification.Child(parentNode.Pointer, depthValue.Value));
				clauseCounter++;
			}

			// check for ambigous parameters
			if (clauseCounter > 1)
				throw new InvalidOperationException("Detected an ambigious parent of clause. Remove either childPointer or childSource.");
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		#endregion
	}
}