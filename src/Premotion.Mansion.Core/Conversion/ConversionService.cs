using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Conversion
{
	/// <summary>
	/// Implements <see cref="IConversionService"/>.
	/// </summary>
	public class ConversionService : IConversionService
	{
		#region Constructors
		/// <summary>
		/// Constructs the conversion service with the given <paramref name="converters"/> and <paramref name="comparers"/>.
		/// </summary>
		/// <param name="converters">The <see cref="IEnumerable{T}"/>s</param>
		/// <param name="comparers">The <see cref="IEnumerable{IComparer}"/>s</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="converters"/> or <paramref name="comparers"/> is null.</exception>
		public ConversionService(IEnumerable<IConverter> converters, IEnumerable<IComparer> comparers)
		{
			// validate arguments
			if (converters == null)
				throw new ArgumentNullException("converters");
			if (comparers == null)
				throw new ArgumentNullException("comparers");

			// set values
			this.converters = converters.ToList();
			this.comparers = comparers.ToList();
		}
		#endregion
		#region Implementation of IConversionService
		/// <summary>
		/// Converts the source object into the desired target type.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <typeparam name="TTarget">The target type.</typeparam>
		/// <param name="source">The source object.</param>
		/// <returns>Returns the converted object.</returns>
		/// <exception cref="NoConverterFoundException">Thrown when there is no convert for <typeparamref name="TTarget"/>.</exception>
		public TTarget Convert<TTarget>(IMansionContext context, object source)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				return default(TTarget);

			// check if the conversion can be solved using casting
			var sourceType = source.GetType();
			if (typeof (TTarget).IsAssignableFrom(sourceType))
				return (TTarget) source;

			// get the converters for the target type
			var converter = GetConverter(context, sourceType, typeof (TTarget));

			// perform the conversion
			return (TTarget) converter.Convert(context, source, sourceType);
		}
		/// <summary>
		/// Converts the source object into the desired target type.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <typeparam name="TTarget">The target type.</typeparam>
		/// <param name="source">The source object.</param>
		/// <param name="defaultValue">The default value returned when the conversion can not be found.</param>
		/// <returns>Returns the converted object or <paramref name="defaultValue"/>.</returns>
		/// <exception cref="NoConverterFoundException">Thrown when there is no convert for <typeparamref name="TTarget"/>.</exception>
		public TTarget Convert<TTarget>(IMansionContext context, object source, TTarget defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				return defaultValue;

			// check if the conversion can be solved using casting
			var sourceType = source.GetType();
			if (typeof (TTarget).IsAssignableFrom(sourceType))
				return (TTarget) source;

			// get the converters for the target type
			var converter = GetConverter(context, sourceType, typeof (TTarget));

			// perform the conversion
			return (TTarget) converter.Convert(context, source, sourceType, defaultValue);
		}
		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="left">The left-hand object.</param>
		/// <param name="right">The right-hand object.</param>
		/// <returns>Returns 0 when the objects are equal, greater than zero when <paramref name="left"/> is greater than <paramref name="right"/>, or less than zero when <paramref name="left"/> is smaller than <paramref name="right"/>.</returns>
		public int Compare(IMansionContext context, object left, object right)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (left == null && right == null)
				return 0;
			if (left != null && right == null)
				return 1;
			if (left == null)
				return -1;

			// get the object type
			var leftType = left.GetType();
			var rightType = right.GetType();

			// make the two types the same type
			object leftObj;
			object rightObj;
			var type = MakeSameType(context, left, leftType, out leftObj, right, rightType, out rightObj);

			// check if there is a comparer for the common type
			var comparer = GetComparer(context, type);

			// return the result of the comparison
			return comparer.Compare(context, leftObj, rightObj);
		}
		/// <summary>
		/// Converts the source object into the desired target type.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source object.</param>
		/// <param name="targetType">The target type.</param>
		/// <returns>Returns the converted object.</returns>
		/// <exception cref="NoConverterFoundException">Thrown when there is no convert is found.</exception>
		private object ConvertObject(IMansionContext context, object source, Type targetType)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				return null;
			if (targetType == null)
				throw new ArgumentNullException("targetType");

			// check if the conversion can be solved using casting
			var sourceType = source.GetType();
			if (targetType.IsAssignableFrom(sourceType))
				return source;

			// get the converters for the target type
			var converter = GetConverter(context, sourceType, targetType);

			// perform the conversion
			return converter.Convert(context, source, sourceType);
		}
		/// <summary>
		/// Gets the converter for the specified input and output types.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sourceType">The input type.</param>
		/// <param name="targetType">The output type.</param>
		/// <returns>Returns the <see cref="IConverter"/>.</returns>
		private IConverter GetConverter(IMansionContext context, Type sourceType, Type targetType)
		{
			IConverter converter;
			if (!Election<IConverter, ConversionVotingSubject>.TryElect(context, converters, new ConversionVotingSubject(sourceType, targetType), out converter))
				throw new NoConverterFoundException(sourceType, targetType);
			return converter;
		}
		/// <summary>
		/// Makes two objects the same type.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="left">The left-hand object.</param>
		/// <param name="leftType">The type of <paramref name="left"/>.</param>
		/// <param name="leftObj">The resulting left-hand object.</param>
		/// <param name="right">The right-hand object.</param>
		/// <param name="rightType">The type of <paramref name="right"/>.</param>
		/// <param name="rightObj">The resulting right-hand object.</param>
		/// <returns>Returns the type to which the resulting objects were casted.</returns>
		private Type MakeSameType(IMansionContext context, object left, Type leftType, out object leftObj, object right, Type rightType, out object rightObj)
		{
			// check if the objects are already of equal type
			if (leftType.Equals(rightType))
			{
				leftObj = left;
				rightObj = right;
				return leftType;
			}

			// get the weights
			var leftWeight = GetTypeWeight(leftType);
			var rightWeight = GetTypeWeight(rightType);
			if (leftWeight != rightWeight)
			{
				// left type is heavier
				if (leftWeight > rightWeight)
				{
					leftObj = left;
					rightObj = ConvertObject(context, right, leftType);
					return leftType;
				}

				// right type is heavier
				leftObj = ConvertObject(context, left, rightType);
				rightObj = right;
				return rightType;
			}

			// TODO: add more code here
			throw new NotImplementedException();
		}
		/// <summary>
		/// Gets the weight the <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> for which tot get the weight.</param>
		/// <returns>Returns the weight.</returns>
		private static int GetTypeWeight(Type type)
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");

			// objects have least weights
			if (typeof (object).Equals(type))
				return 0;

			// string
			if (typeof (string).Equals(type))
				return 1;

			return 2;
		}
		/// <summary>
		/// Gets the <see cref="IComparer"/> for object of type <paramref name="objectType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="objectType">The type of object being compared.</param>
		/// <returns>Returns the <see cref="IComparer"/>.</returns>
		private IComparer GetComparer(IMansionContext context, Type objectType)
		{
			IComparer comparer;
			if (!Election<IComparer, Type>.TryElect(context, comparers, objectType, out comparer))
				throw new InvalidOperationException(string.Format("Could not find a comparer for objects of type '{0}'", objectType));
			return comparer;
		}
		#endregion
		#region Private Fields
		private readonly IEnumerable<IComparer> comparers;
		private readonly IEnumerable<IConverter> converters;
		#endregion
	}
}