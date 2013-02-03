using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Represents the event args for a missing property event.
	/// </summary>
	public class MissingPropertyEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="propertyName"></param>
		public MissingPropertyEventArgs(IMansionContext context, string propertyName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			Context = context;
			PropertyName = propertyName;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether a value was set.
		/// </summary>
		public bool HasValue { get; private set; }
		/// <summary>
		/// Gets the new value of the object.
		/// </summary>
		public object Value
		{
			get { return value; }
			set
			{
				if (HasValue)
					throw new InvalidOperationException("A value was already set");
				this.value = value;
				HasValue = true;
			}
		}
		/// <summary>
		/// Gets the <see cref="IMansionContext"/> in which the event was executed.
		/// </summary>
		public IMansionContext Context { get; private set; }
		/// <summary>
		/// Gets the name of the missing property.
		/// </summary>
		public string PropertyName { get; private set; }
		#endregion
		#region Private Fields
		private object value;
		#endregion
	}
	/// <summary>
	/// Implements <see cref="PropertyBag"/>.
	/// </summary>
	[Serializable, JsonObject(MemberSerialization.OptIn)]
	public class PropertyBag : IPropertyBag
	{
		#region Constructors
		/// <summary>
		/// Constructs a property bag.
		/// </summary>
		public PropertyBag()
		{
		}
		/// <summary>
		/// Constructs a property bag from the specified properties.
		/// </summary>
		/// <param name="properties"></param>
		public PropertyBag(IEnumerable<KeyValuePair<string, object>> properties)
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");

			// copy the values
			foreach (var property in properties)
				InnerData.Add(property.Key, property.Value);
		}
		#endregion
		#region Property Methods
		/// <summary>
		/// Gets the value for a specific property.
		/// </summary>
		/// <typeparam name="TValue">The value type.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property which to get.</param>
		/// <returns>Returns the value.</returns>
		/// <exception cref="PropertyNotFoundException">Thrown when the property can not be found in this bag.</exception>
		public TValue Get<TValue>(IMansionContext context, string propertyName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// get the value
			object value;
			TryGetInternalValue(context, propertyName, out value);

			// get the conversion service
			var conversionService = context.Nucleus.ResolveSingle<IConversionService>();

			// convert the value
			return conversionService.Convert<TValue>(context, value);
		}
		/// <summary>
		/// Gets the value for a specific property.
		/// </summary>
		/// <typeparam name="TValue">The value type.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property which to get.</param>
		/// <param name="defaultValue">The default value when the property can not be found or cast.</param>
		/// <returns>Returns the value.</returns>
		public TValue Get<TValue>(IMansionContext context, string propertyName, TValue defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// try to get the value
			object value;
			if (!TryGetInternalValue(context, propertyName, out value))
				return defaultValue;

			// get the conversion service
			var conversionService = context.Nucleus.ResolveSingle<IConversionService>();

			// convert the value
			return conversionService.Convert(context, value, defaultValue);
		}
		/// <summary>
		/// Tries to get the value from this bag.
		/// </summary>
		/// <typeparam name="TValue">The type of value.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="target">The target.</param>
		/// <returns>Returns true when the property could be retrieved otherwise false.</returns>
		public bool TryGet<TValue>(IMansionContext context, string propertyName, out TValue target)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// get the value
			object value;
			if (!TryGetInternalValue(context, propertyName, out value))
			{
				target = default(TValue);
				return false;
			}

			// get the conversion service
			var conversionService = context.Nucleus.ResolveSingle<IConversionService>();

			// convert the value
			target = conversionService.Convert<TValue>(context, value);
			return true;
		}
		/// <summary>
		/// Tries to get the value from this bag and remove it.
		/// </summary>
		/// <typeparam name="TValue">The type of value.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="target">The target.</param>
		/// <returns>Returns true when the property could be retrieved otherwise false.</returns>
		public bool TryGetAndRemove<TValue>(IMansionContext context, string propertyName, out TValue target)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// get the value
			object value;
			if (!TryGetInternalValue(context, propertyName, out value))
			{
				target = default(TValue);
				return false;
			}
			InnerData.Remove(propertyName);
			if (value == null)
			{
				target = default(TValue);
				return false;
			}

			// get the conversion service
			var conversionService = context.Nucleus.ResolveSingle<IConversionService>();

			// convert the value
			target = conversionService.Convert<TValue>(context, value);
			return true;
		}
		/// <summary>
		/// Sets a new property in the bag with the name <paramref name="propertyName"/> and the value <paramref name="value"/>. Overwrites any value.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="value">The new value of the property.</param>
		public virtual void Set(string propertyName, object value)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			if (InnerData.ContainsKey(propertyName))
				InnerData[propertyName] = value;
			else
				InnerData.Add(propertyName, value);
		}
		/// <summary>
		/// Sets a new property in the bag with the name <paramref name="propertyName"/> and the value <paramref name="value"/> only when the property does not already exist.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="value">The new value of the property.</param>
		/// <returns>Returns true when the property did not already exist.</returns>
		public bool TrySet(string propertyName, object value)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			if (!InnerData.ContainsKey(propertyName))
			{
				InnerData[propertyName] = value;
				return true;
			}

			return false;
		}
		/// <summary>
		/// Merges properties into this bag.
		/// </summary>
		/// <param name="properties"></param>
		/// <returns>Returns this for chaining.</returns>
		public IPropertyBag Merge(IEnumerable<KeyValuePair<string, object>> properties)
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");

			// loop through the properties
			foreach (var property in properties)
				Set(property.Key, property.Value);
			return this;
		}
		/// <summary>
		/// Checks whether this property bag contains the <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property for which to check.</param>
		/// <returns>Returns true when this bag contains the property otherwise false.</returns>
		public bool Contains(string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			return InnerData.ContainsKey(propertyName);
		}
		/// <summary>
		/// Removes a property from this bag.
		/// </summary>
		/// <param name="propertyName">The name of the property which to remove.</param>
		public void Remove(string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			InnerData.Remove(propertyName);
		}
		/// <summary>
		/// Tries to read a value from the internal data representation.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property which to read.</param>
		/// <param name="value">The value of the property.</param>
		/// <returns>Returns true when the property was found, otherwise false.</returns>
		private bool TryGetInternalValue(IMansionContext context, string propertyName, out object value)
		{
			// if the property is not found, fire the event
			if (InnerData.TryGetValue(propertyName, out value))
				return true;

			// raise the missing property event
			var args = new MissingPropertyEventArgs(context, propertyName);
			RaiseServiceRegistering(args);

			// if no value was set, return false
			if (!args.HasValue)
				return false;

			// set the new value
			value = args.Value;
			Set(propertyName, args.Value);
			return true;
		}
		/// <summary>
		/// Convenience method which allows the property bag to be initalized by a dictionary initializer. Calls <see cref="Set"/> internally.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="value">The new value of the property.</param>
		public void Add(string propertyName, object value)
		{
			Set(propertyName, value);
		}
		#endregion
		#region IEnumerable implementation
		/// <summary>
		/// Enumerates through this bag.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return InnerData.GetEnumerator();
		}
		/// <summary>
		/// Enumerates through this bag.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		#region Events
		/// <summary>
		/// Fired when a property missing is detected.
		/// </summary>
		public event EventHandler<MissingPropertyEventArgs> MissingProperty;
		/// <summary>
		/// Raises the <see cref="MissingProperty"/> event.
		/// </summary>
		/// <param name="args">The <see cref="MissingPropertyEventArgs"/>.</param>
		private void RaiseServiceRegistering(MissingPropertyEventArgs args)
		{
			var handlers = MissingProperty;
			if (handlers != null)
				handlers(this, args);
		}
		#endregion
		#region Helpers
		/// <summary>
		/// Gets the modified properties.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="original">The original properties.</param>
		/// <param name="modifications">The modifications.</param>
		/// <returns>Returns only additions or modified properties.</returns>
		public static IPropertyBag GetModifiedProperties(IMansionContext context, IPropertyBag original, IEnumerable<KeyValuePair<string, object>> modifications)
		{
			// validate arugments
			if (context == null)
				throw new ArgumentNullException("context");
			if (original == null)
				throw new ArgumentNullException("original");
			if (modifications == null)
				throw new ArgumentNullException("modifications");

			// create a new property bag
			var modifiedProperties = new PropertyBag(from candidate in modifications
			                                         where (original.Names.Contains(candidate.Key) && !AreEqual(original.Get<object>(context, candidate.Key), candidate.Value)) || !original.Names.Contains(candidate.Key)
			                                         select candidate);

			return modifiedProperties;
		}
		/// <summary>
		/// Compares two objects.
		/// </summary>
		/// <param name="one"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		private static bool AreEqual(object one, object other)
		{
			// check for null
			if (one == null && other == null)
				return true;
			if (one == null || other == null)
				return false;

			// check for IComparable
			var oneComparable = one as IComparable;
			if (oneComparable != null && one.GetType() == other.GetType())
				return oneComparable.CompareTo(other) == 0;

			// default
			return one.Equals(other);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the inner data of this bag.
		/// </summary>
		[JsonProperty]
		private IDictionary<string, object> InnerData
		{
			get { return innerData; }
		}
		/// <summary>
		/// Gets the names of all the properties in this bag.
		/// </summary>
		public IEnumerable<string> Names
		{
			get { return InnerData.Keys; }
		}
		/// <summary>
		/// Gets the number of properties in this bag.
		/// </summary>
		public int Count
		{
			get { return InnerData.Count; }
		}
		#endregion
		#region Private Fields
		private readonly IDictionary<string, object> innerData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		#endregion
	}
}