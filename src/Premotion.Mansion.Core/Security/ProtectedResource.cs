using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Represents a protected resource which contains on or more protected action.
	/// </summary>
	public class ProtectedResource : IEquatable<ProtectedResource>
	{
		#region Constructors
		/// <summary>
		/// Constructs a protected resource with the specified ID.
		/// </summary>
		/// <param name="id">The ID of the resource.</param>
		public ProtectedResource(string id)
		{
			// validate arguments
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");

			// set values
			this.id = id;
		}
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(ProtectedResource other)
		{
			if (ReferenceEquals(null, other))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return Equals(other.id, id);
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return "resource " + (string.IsNullOrEmpty(Name) ? Id : string.Format("{0} ({1})", Name, Id));
		}
		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof (ProtectedResource))
				return false;
			return Equals((ProtectedResource) obj);
		}
		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return id.GetHashCode();
		}
		#endregion
		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ProtectedResource left, ProtectedResource right)
		{
			return Equals(left, right);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(ProtectedResource left, ProtectedResource right)
		{
			return !Equals(left, right);
		}
		#endregion
		#region Methods
		/// <summary>
		/// Adds a <paramref name="operation"/> to this resource.
		/// </summary>
		/// <param name="operation">The <see cref="ProtectedOperation"/> which to add.</param>
		public void Add(ProtectedOperation operation)
		{
			// validate arguments
			if (operation == null)
				throw new ArgumentNullException("operation");
			operations.Add(operation);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of this protected resource.
		/// </summary>
		public string Id
		{
			get { return id; }
		}
		/// <summary>
		/// Gets/Sets the friendly name of this resource.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// gets the <see cref="ProtectedOperation"/>s of this resource.
		/// </summary>
		public IEnumerable<ProtectedOperation> Operations
		{
			get { return operations; }
		}
		#endregion
		#region Private Fields
		private readonly string id;
		private readonly List<ProtectedOperation> operations = new List<ProtectedOperation>();
		#endregion
	}
}