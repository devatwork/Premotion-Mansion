using System;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// Represents an analysis component.
	/// </summary>
	/// <typeparam name="TComponent">The type of component.</typeparam>
	public class AnalysisComponent<TComponent> where TComponent : class
	{
		#region Constructors
		/// <summary>
		/// Constructs an analysis component.
		/// </summary>
		/// <param name="registeredName">The registered name of this component.</param>
		/// <param name="component">The actual component.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public AnalysisComponent(string registeredName, TComponent component)
		{
			// validate arguments
			if (string.IsNullOrEmpty(registeredName))
				throw new ArgumentNullException("registeredName");
			if (component == null)
				throw new ArgumentNullException("component");

			// set values
			RegisteredName = registeredName.ToLower();
			Component = component;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the registered name of this component.
		/// </summary>
		public string RegisteredName { get; private set; }
		/// <summary>
		/// Gets the <typeparamref name="TComponent"/>.
		/// </summary>
		public TComponent Component { get; private set; }
		#endregion
	}
}