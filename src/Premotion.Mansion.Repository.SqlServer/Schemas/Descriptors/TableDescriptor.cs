using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Base class for all tyable descriptors.
	/// </summary>
	public abstract class TableDescriptor : TypeDescriptor
	{
		#region Factory Methods
		/// <summary>
		/// Creates the <see cref="Table"/> from the descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="schema">The <see cref="Schema"/>in which to create the table.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Create(IMansionContext context, Schema schema)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (schema == null)
				throw new ArgumentNullException("schema");

			// invoke template method
			DoCreate(context, schema);
		}
		/// <summary>
		/// Creates the <see cref="Table"/> from the descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="schema">The <see cref="Schema"/>in which to create the table.</param>
		protected abstract void DoCreate(IMansionContext context, Schema schema);
		#endregion
	}
}