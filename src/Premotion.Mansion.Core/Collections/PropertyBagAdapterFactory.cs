using System;
using System.Collections.Generic;
using System.Reflection;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Constructs adapters for <see cref="IPropertyBag"/>.
	/// </summary>
	public static class PropertyBagAdapterFactory
	{
		#region Constants
		private const string CacheKeyPrefix = "PropertyBagAdapter_";
		#endregion
		#region Nested type: AdapterFactory
		/// <summary>
		/// Factory for creating adapter for objects of type <typeparamref name="TObject"/>.
		/// </summary>
		/// <typeparam name="TObject">The object which will be to adapted.</typeparam>
		private class AdapterFactory<TObject> where TObject : class
		{
			#region Constructors
			/// <summary>
			/// </summary>
			/// <param name="factoryMethod">The factory method.</param>
			private AdapterFactory(Func<TObject, IPropertyBag> factoryMethod)
			{
				// validate arguments
				if (factoryMethod == null)
					throw new ArgumentNullException("factoryMethod");

				// set values
				this.factoryMethod = factoryMethod;
			}
			#endregion
			#region Factory Methods
			/// <summary>
			/// Creates a <see cref="AdapterFactory{TObjects}"/>.
			/// </summary>
			/// <param name="context">The <see cref="IContext"/>.</param>
			/// <param name="type">The type for which to create the adapter factory.</param>
			/// <returns>Returns the created factory.</returns>
			public static AdapterFactory<TObject> Create(IContext context, Type type)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (type == null)
					throw new ArgumentNullException("type");

				// get the properties of this type
				var properties = type.GetProperties();

				// create the adapter factory
				return new AdapterFactory<TObject>(instance => new ReflectionAdapter(instance, properties));
			}
			/// <summary>
			/// Creates an instance of the adapter.
			/// </summary>
			/// <param name="obj">The object which to adapt.</param>
			/// <returns>Returns the adapted object.</returns>
			public IPropertyBag CreateInstance(TObject obj)
			{
				// validate arguments
				if (obj == null)
					throw new ArgumentNullException("obj");

				// use the factory method to create the instance
				return factoryMethod(obj);
			}
			#endregion
			#region Private Fields
			private readonly Func<TObject, IPropertyBag> factoryMethod;
			#endregion
		}
		#endregion
		#region Nested type: CachedAdapterFactory
		/// <summary>
		/// Implements the cached version of an adapter factory.
		/// </summary>
		/// <typeparam name="TObject">The type of object.</typeparam>
		private class CachedAdapterFactory<TObject> : CachedObject<AdapterFactory<TObject>> where TObject : class
		{
			#region Constructors
			/// <summary>
			/// </summary>
			/// <param name="obj"></param>
			public CachedAdapterFactory(AdapterFactory<TObject> obj) : base(obj)
			{
				Priority = Priority.NotRemovable;
			}
			#endregion
		}
		#endregion
		#region Adapt Methods
		/// <summary>
		/// Adapts any <see cref="object"/> to <see cref="IPropertyBag"/>, all public properties of the object are available in the <see cref="IPropertyBag"/>.
		/// </summary>
		/// <typeparam name="TObject"></typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="obj">The object which to wrap.</param>
		/// <returns>Return the property bag.</returns>
		public static IPropertyBag Adapt<TObject>(IContext context, TObject obj) where TObject : class
		{
			return Adapt(context, typeof (TObject), obj);
		}
		/// <summary>
		/// Adapts any <see cref="object"/> to <see cref="IPropertyBag"/>, all public properties of the object are available in the <see cref="IPropertyBag"/>.
		/// </summary>
		/// <typeparam name="TObject"></typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="type">The actual <see cref="Type"/> for which to create the adapter, <paramref name="obj"/> must be assignable from this <paramref name="type"/>.</param>
		/// <param name="obj">The object which to wrap.</param>
		/// <returns>Return the property bag.</returns>
		public static IPropertyBag Adapt<TObject>(IContext context, Type type, TObject obj) where TObject : class
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (obj == null)
				throw new ArgumentNullException("obj");
			if (type == null)
				throw new ArgumentNullException("type");

			// load the adapter from cache or create it
			return context.Cast<INucleusAwareContext>().Nucleus.Get<ICachingService>(context).GetOrAdd(context, (StringCacheKey) (CacheKeyPrefix + type), () =>
			                                                                                                                                              {
			                                                                                                                                              	// check if the 
			                                                                                                                                              	if (!type.IsAssignableFrom(obj.GetType()))
			                                                                                                                                              		throw new InvalidOperationException("The type must be assignable from obj");

			                                                                                                                                              	// create the factory
			                                                                                                                                              	var adapterFactory = AdapterFactory<TObject>.Create(context, type);

			                                                                                                                                              	// make it cachable
			                                                                                                                                              	return new CachedAdapterFactory<TObject>(adapterFactory);
			                                                                                                                                              }).CreateInstance(obj);
		}
		/// <summary>
		/// Unwraps an object of type <typeparamref name="TObject"/> to it's original form.
		/// </summary>
		/// <typeparam name="TObject">The type of wrapped object.</typeparam>
		/// <param name="wrapper">The wrapped object instance.</param>
		/// <returns>Returns the unwrapped object.</returns>
		public static TObject GetOriginalObject<TObject>(IPropertyBag wrapper)
		{
			// validate arguments
			if (wrapper == null)
				throw new ArgumentNullException("wrapper");

			// check if the object is indeed wrapped
			var adaptedObject = wrapper as IAdaptedObject;
			if (adaptedObject == null)
				throw new InvalidOperationException("The property bag is not wrapped by this factory");

			// check whether the wrapped object can be cast to TObject
			var wrappedObject = adaptedObject.Instance;
			var wrappedObjectType = wrappedObject.GetType();
			if (!typeof (TObject).IsAssignableFrom(wrappedObjectType))
				throw new InvalidOperationException(string.Format("Could not unwrap object of type '{0}' to '{1}'", wrappedObjectType, typeof (TObject)));

			// just cast the object
			return (TObject) wrappedObject;
		}
		#endregion
		#region Nested type: IAdaptedObject
		/// <summary>
		/// Marks an adapted object.
		/// </summary>
		public interface IAdaptedObject
		{
			#region Properties
			/// <summary>
			/// Gets the wrapped instance.
			/// </summary>
			object Instance { get; }
			#endregion
		}
		#endregion
		#region Nested type: ReflectionAdapter
		/// <summary>
		/// Implements the property bag adapter using relfection.
		/// </summary>
		private class ReflectionAdapter : PropertyBag, IAdaptedObject
		{
			#region Constructors
			/// <summary>
			/// </summary>
			/// <param name="instance"></param>
			/// <param name="propertyMethods"></param>
			public ReflectionAdapter(object instance, IEnumerable<PropertyInfo> propertyMethods)
			{
				// validate arguments
				if (instance == null)
					throw new ArgumentNullException("instance");
				if (propertyMethods == null)
					throw new ArgumentNullException("propertyMethods");

				// set values
				this.instance = instance;

				// loop over all properties and store them
				foreach (var propertyInfo in propertyMethods)
				{
					// store the value of the property in the bag
					base.Set(propertyInfo.Name, propertyInfo.GetValue(instance, null));

					// store the property
					properties.Add(propertyInfo.Name, propertyInfo);
				}
			}
			#endregion
			#region Overrides of PropertyBag
			/// <summary>
			/// Sets a new property in the bag with the name <paramref name="propertyName"/> and the value <paramref name="value"/>. Overwrites any value.
			/// </summary>
			/// <param name="propertyName">The name of the property.</param>
			/// <param name="value">The new value of the property.</param>
			public override void Set(string propertyName, object value)
			{
				// validate arguments
				if (string.IsNullOrEmpty(propertyName))
					throw new ArgumentNullException("propertyName");

				// get the property which to set
				PropertyInfo property;
				if (!properties.TryGetValue(propertyName, out property))
					throw new InvalidOperationException(string.Format("'{0}' is not a property of '{1}'", propertyName, instance.GetType()));

				// check if the property is read-only
				if (!property.CanWrite)
					throw new InvalidOperationException(string.Format("'{0}' is a read-only property of '{1}'", propertyName, instance.GetType()));

				// store the value in the object
				property.SetValue(instance, value, null);

				// store the value in the dictionary
				base.Set(propertyName, value);
			}
			#endregion
			#region Implementation of IAdaptedObject
			/// <summary>
			/// Gets the wrapped instance.
			/// </summary>
			public object Instance
			{
				get { return instance; }
			}
			#endregion
			#region Private Fields
			private readonly object instance;
			private readonly Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
			#endregion
		}
		#endregion
	}
}