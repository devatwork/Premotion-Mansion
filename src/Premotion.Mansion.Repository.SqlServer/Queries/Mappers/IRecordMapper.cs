using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Patterns.Prioritized;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Mappers
{
	/// <summary>
	/// Maps an <see cref="Record"/> to properties.
	/// </summary>
	public interface IRecordMapper : IPrioritized
	{
		#region Map Methods
		/// <summary>
		/// Maps the given <paramref name="record"/> to <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which to map.</param>
		/// <param name="properties">The <see cref="IPropertyBag"/> in which to store the mapped result.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		void Map(IMansionContext context, Record record, IPropertyBag properties);
		#endregion
	}
}