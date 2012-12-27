using System;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Represents an engine which indexes data so it can be queries using <see cref="BaseQueryEngine"/>.
	/// </summary>
	public abstract class BaseIndexEngine
	{
		#region Index Methods
		/// <summary>
		/// Indexes the given <paramref name="record"/> into this engine.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to index.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="record"/> is null.</exception>
		public void Index(IMansionContext context, Record record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");

			// invoke template method
			DoIndex(context, record);
		}
		/// <summary>
		/// Indexes the given <paramref name="record"/> into this engine.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to index.</param>
		protected abstract void DoIndex(IMansionContext context, Record record);
		#endregion
		#region Delete Methods
		/// <summary>
		/// Deletes the given <paramref name="record"/> from this engine.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to delete.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="record"/> is null.</exception>
		public void Delete(IMansionContext context, Record record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");

			// invoke template method
			DoDelete(context, record);
		}
		/// <summary>
		/// Deletes the given <paramref name="record"/> from this engine.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to delete.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="record"/> is null.</exception>
		protected abstract void DoDelete(IMansionContext context, Record record);
		#endregion
	}
}