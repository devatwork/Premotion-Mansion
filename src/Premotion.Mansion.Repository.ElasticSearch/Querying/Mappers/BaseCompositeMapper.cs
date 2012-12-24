using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Base class for composite specification mappers.
	/// </summary>
	/// <typeparam name="TSpecification">The type of <see cref="Specification"/> mapped by this mapper.</typeparam>
	public abstract class BaseCompositeMapper<TSpecification> : BaseSpecificationMapper<TSpecification> where TSpecification : Specification
	{
		#region Constructors
		/// <summary>
		/// Constructs a BaseCompositeMapper.
		/// </summary>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nucleus"/> is null.</exception>
		protected BaseCompositeMapper(INucleus nucleus)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// create factory
			mappers = new Lazy<IEnumerable<ISpecificationMapper>>(nucleus.Resolve<ISpecificationMapper>);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="ISpecificationMapper"/>s.
		/// </summary>
		protected IEnumerable<ISpecificationMapper> Mappers
		{
			get { return mappers.Value; }
		}
		#endregion
		#region Private Fields
		private readonly Lazy<IEnumerable<ISpecificationMapper>> mappers;
		#endregion
	}
}