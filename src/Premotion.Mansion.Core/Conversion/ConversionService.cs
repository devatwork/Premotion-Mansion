using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Dependencies;
using Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Conversion
{
	/// <summary>
	/// Implements <see cref="IConversionService"/>.
	/// </summary>
	public class ConversionService : ManagedLifecycleService, IConversionService, IServiceWithDependencies
	{
		#region Implementation of IConversionService
		/// <summary>
		/// Converts the source object into the desired target type.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <typeparam name="TTarget">The target type.</typeparam>
		/// <param name="source">The source object.</param>
		/// <returns>Returns the converted object.</returns>
		/// <exception cref="NoConverterFoundException">Thrown when there is no convert for <typeparamref name="TTarget"/>.</exception>
		public TTarget Convert<TTarget>(IContext context, object source)
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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <typeparam name="TTarget">The target type.</typeparam>
		/// <param name="source">The source object.</param>
		/// <param name="defaultValue">The default value returned when the conversion can not be found.</param>
		/// <returns>Returns the converted object or <paramref name="defaultValue"/>.</returns>
		/// <exception cref="NoConverterFoundException">Thrown when there is no convert for <typeparamref name="TTarget"/>.</exception>
		public TTarget Convert<TTarget>(IContext context, object source, TTarget defaultValue)
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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="left">The left-hand object.</param>
		/// <param name="right">The right-hand object.</param>
		/// <returns>Returns 0 when the objects are equal, greater than zero when <paramref name="left"/> is greater than <paramref name="right"/>, or less than zero when <paramref name="left"/> is smaller than <paramref name="right"/>.</returns>
		public int Compare(IContext context, object left, object right)
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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The source object.</param>
		/// <param name="targetType">The target type.</param>
		/// <returns>Returns the converted object.</returns>
		/// <exception cref="NoConverterFoundException">Thrown when there is no convert is found.</exception>
		private object ConvertObject(IContext context, object source, Type targetType)
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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="sourceType">The input type.</param>
		/// <param name="targetType">The output type.</param>
		/// <returns>Returns the <see cref="IConverter"/>.</returns>
		private IConverter GetConverter(IContext context, Type sourceType, Type targetType)
		{
			IConverter converter;
			if (!Election<IConverter, ConversionVotingSubject>.TryElect(context, converters, new ConversionVotingSubject(sourceType, targetType), out converter))
				throw new NoConverterFoundException(sourceType, targetType);
			return converter;
		}
		/// <summary>
		/// Makes two objects the same type.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="left">The left-hand object.</param>
		/// <param name="leftType">The type of <paramref name="left"/>.</param>
		/// <param name="leftObj">The resulting left-hand object.</param>
		/// <param name="right">The right-hand object.</param>
		/// <param name="rightType">The type of <paramref name="right"/>.</param>
		/// <param name="rightObj">The resulting right-hand object.</param>
		/// <returns>Returns the type to which the resulting objects were casted.</returns>
		private Type MakeSameType(IContext context, object left, Type leftType, out object leftObj, object right, Type rightType, out object rightObj)
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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="objectType">The type of object being compared.</param>
		/// <returns>Returns the <see cref="IComparer"/>.</returns>
		private IComparer GetComparer(IContext context, Type objectType)
		{
			IComparer comparer;
			if (!Election<IComparer, Type>.TryElect(context, comparers, objectType, out comparer))
				throw new InvalidOperationException(string.Format("Could not find a comparer for objects of type '{0}'", objectType));
			return comparer;
		}
		#endregion
		#region Implementation of IServiceWithDependencies
		/// <summary>
		/// Gets the <see cref="DependencyModel"/> of this service.
		/// </summary>
		public DependencyModel Dependencies
		{
			get { return dependencies; }
		}
		#endregion
		#region Overrides of ManagedLifecycleService
		/// <summary>
		/// Invoked just before this service is used for the first time.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		protected override void DoStart(INucleusAwareContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the naming and object factory services
			var namingService = context.Nucleus.Get<ITypeDirectoryService>(context);
			var objectFactoryService = context.Nucleus.Get<IObjectFactoryService>(context);

			// look up all the types implementing 
			converters.AddRange(objectFactoryService.Create<IConverter>(namingService.Lookup<IConverter>()));
			comparers.AddRange(objectFactoryService.Create<IComparer>(namingService.Lookup<IComparer>()));
		}
		#endregion
		#region Private Fields
		private static readonly DependencyModel dependencies = new DependencyModel().Add<ITypeDirectoryService>().Add<IObjectFactoryService>();
		private readonly List<IComparer> comparers = new List<IComparer>();
		private readonly List<IConverter> converters = new List<IConverter>();
		#endregion
	}
}