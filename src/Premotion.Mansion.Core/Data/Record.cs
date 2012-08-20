using System;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Defines a record.
	/// </summary>
	public class Record : PropertyBag, IRecord
	{
		#region Implementation of IRecord
		/// <summary>
		/// Initializes this record.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		public virtual void Initialize(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// invoke template method
			DoInitialize(context);
		}
		/// <summary>
		/// Initializes this record.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		protected virtual void DoInitialize(IMansionContext context)
		{
			Id = Get<int>(context, "id");
			Type = Get<string>(context, "type");
		}
		/// <summary>
		/// Gets the ID of this record.
		/// </summary>
		public int Id { get; private set; }
		/// <summary>
		/// Gets the type name of this record.
		/// </summary>
		public string Type { get; private set; }
		#endregion
	}
}