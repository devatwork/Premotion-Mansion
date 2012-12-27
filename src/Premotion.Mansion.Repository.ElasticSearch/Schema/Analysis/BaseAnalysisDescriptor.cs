using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// Base class for all analysis components.
	/// </summary>
	/// <typeparam name="TComponent">The type of component created by this descriptor.</typeparam>
	public abstract class BaseAnalysisDescriptor<TComponent> : TypeDescriptor where TComponent : class
	{
		#region Create Methods
		/// <summary>
		/// Creates the <see cref="AnalysisComponent{TComponent}"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the created <see cref="AnalysisComponent{TComponent}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if the descriptor does not contain a registeredName.</exception>
		public AnalysisComponent<TComponent> Create(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the logical name
			var registeredName = Properties.Get<string>(context, "registeredName");
			if (string.IsNullOrEmpty(registeredName))
				throw new InvalidOperationException("Analysis components should always have a registeredName");

			// create the component
			var component = DoCreate(context);

			// return the analysis component
			return new AnalysisComponent<TComponent>(registeredName, component);
		}
		/// <summary>
		/// Creates the <typeparamref name="TComponent"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the created <typeparamref name="TComponent"/>.</returns>
		protected abstract TComponent DoCreate(IMansionContext context);
		#endregion
	}
}