using System;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Prioritized;
using Premotion.Mansion.Core.Patterns.Processing;

namespace Premotion.Mansion.Core.Data.Queries.Parser
{
	/// <summary>
	/// Base type for all <see cref="Query"/> argument processors. These processers parse parameters and turn them into parts of the <see cref="Query"/>.
	/// </summary>
	[Exported(typeof (QueryArgumentProcessor))]
	public abstract class QueryArgumentProcessor : IProcessor<IMansionContext, IPropertyBag, Query>, IPrioritized
	{
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="QueryArgumentProcessor"/> with the given <paramref name="priority"/>.
		/// </summary>
		/// <param name="priority">The priority of this arugment processor.</param>
		protected QueryArgumentProcessor(int priority)
		{
			this.priority = priority;
		}
		#endregion
		#region Implementation of IProcessor<in IMansionContext,in IPropertyBag,in Query>
		/// <summary>
		/// Processes the <paramref name="input"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to process.</param>
		/// <param name="output">The output on which to operate.</param>
		public void Process(IMansionContext context, IPropertyBag input, Query output)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				throw new ArgumentNullException("input");
			if (output == null)
				throw new ArgumentNullException("output");

			// invoke template method
			DoProcess(context, input, output);
		}
		#endregion
		#region Implementation of IPrioritized
		/// <summary>
		/// Gets the relative priority of this object. The higher the priority, earlier this object is executed.
		/// </summary>
		public int Priority
		{
			get { return priority; }
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// Processes the <paramref name="parameters"/> and turn them into <paramref name="query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parameters">The parameters which to process.</param>
		/// <param name="query">The <see cref="Query"/> in which to set the parameters.</param>
		protected abstract void DoProcess(IMansionContext context, IPropertyBag parameters, Query query);
		#endregion
		#region Private Fields
		private readonly int priority;
		#endregion
	}
}