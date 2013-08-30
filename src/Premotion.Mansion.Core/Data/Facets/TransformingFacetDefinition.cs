using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting;

namespace Premotion.Mansion.Core.Data.Facets
{
	/// <summary>
	/// Implements a transforming <see cref="FacetDefinition"/>.
	/// </summary>
	public class TransformingFacetDefinition : FacetDefinition
	{
		#region Constructors
		/// <summary>
		/// Creates a facet with the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to facet.</param>
		/// <param name="friendlyName">The friendly name.</param>
		/// <param name="transformation">The name of the transform function for each of the facets value.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null or empty.</exception>
		public TransformingFacetDefinition(string propertyName, string friendlyName, IScript transformation) : base(propertyName, friendlyName)
		{
			// validate arguments
			if (transformation == null)
				throw new ArgumentNullException("transformation");

			// set values
			this.transformation = transformation;
		}
		#endregion
		#region Overrides of FacetDefinition
		/// <summary>
		/// Transforms the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="values">The <see cref="FacetValue"/>s which to transform.</param>
		/// <returns>Returns the transformed <see cref="FacetValue"/>s.</returns>
		protected override IEnumerable<FacetValue> DoTransform(IMansionContext context, IEnumerable<FacetValue> values)
		{
			using (context.Stack.Push("Facet", PropertyBagAdapterFactory.Adapt(context, this)))
			{
				return base.DoTransform(context, values.Select(value => {
					using (context.Stack.Push("Row", PropertyBagAdapterFactory.Adapt(context, value)))
						transformation.Execute(context);
					return value;
				})).ToArray();
			}
		}
		#endregion
		#region Private Fields
		private readonly IScript transformation;
		#endregion
	}
}