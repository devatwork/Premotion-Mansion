using System;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Repository.SqlServer.QueryCommands.Mappers
{
	/// <summary>
	/// Base class for all <see cref="IRecordMapper"/>s.
	/// </summary>
	public abstract class RecordMapper : IRecordMapper
	{
		#region Constructors
		/// <summary>
		/// Constructs this <see cref="IRecordMapper"/> with the given priority.
		/// </summary>
		/// <param name="priority">The priority.</param>
		protected RecordMapper(int priority)
		{
			// set value
			this.priority = priority;
		}
		#endregion
		#region Implementation of IRecordMapper
		/// <summary>
		/// Maps the given <paramref name="record"/> to <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to map.</param>
		/// <param name="properties">The <see cref="IPropertyBag"/> in which to store the mapped result.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Map(IMansionContext context, Record record, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// invoke template method
			DoMap(context, record, properties);
		}
		/// <summary>
		/// Maps the given <paramref name="record"/> to <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to map.</param>
		/// <param name="properties">The <see cref="IPropertyBag"/> in which to store the mapped result.</param>
		protected abstract void DoMap(IMansionContext context, Record record, IPropertyBag properties);
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
		#region Private Fields
		private readonly int priority;
		#endregion
	}
}