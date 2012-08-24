using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Represents the base class for all property tables.
	/// </summary>
	public abstract class PropertyTableDescriptor : TypeDescriptor
	{
		#region Factory Methods
		/// <summary>
		/// Creates the <see cref="Table"/> from the descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="schema">The <see cref="Schema"/>in which to create the table.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Create(IMansionContext context, Schema schema, IPropertyDefinition property)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (property == null)
				throw new ArgumentNullException("property");

			// invoke template method
			DoCreate(context, schema, property);
		}
		/// <summary>
		/// Creates the <see cref="Table"/> from the descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="schema">The <see cref="Schema"/>in which to create the table.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		protected abstract void DoCreate(IMansionContext context, Schema schema, IPropertyDefinition property);
		#endregion
	}
}