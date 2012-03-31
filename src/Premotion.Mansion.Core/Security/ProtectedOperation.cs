using System;

namespace Premotion.Mansion.Core.Security
{
	/// <summary>
	/// Represents a protected action.
	/// </summary>
	public class ProtectedOperation : IEquatable<ProtectedOperation>
	{
		#region Constructors
		/// <summary>
		/// Constructs a protected resource with the specified ID.
		/// </summary>
		/// <param name="id">The ID of the resource.</param>
		/// <param name="resource">The <see cref="ProtectedResource"/>.</param>
		public ProtectedOperation(string id, ProtectedResource resource)
		{
			// validate arguments
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// set values
			this.id = id;
			this.resource = resource;
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
		public bool Equals(ProtectedOperation other)
		{
			if (ReferenceEquals(null, other))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return Equals(other.id, id) && Equals(other.Resource, Resource);
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
			return "operation " + (string.IsNullOrEmpty(Name) ? Id : string.Format("{0} ({1})", Name, Id)) + " on " + Resource;
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
			unchecked
			{
				return (id.GetHashCode()*397) ^ (Resource.GetHashCode());
			}
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
			if (obj.GetType() != typeof (ProtectedOperation))
				return false;
			return Equals((ProtectedOperation) obj);
		}
		#endregion
		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(ProtectedOperation left, ProtectedOperation right)
		{
			return Equals(left, right);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(ProtectedOperation left, ProtectedOperation right)
		{
			return !Equals(left, right);
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates a <see cref="ProtectedOperation"/> from the <paramref name="resourceId"/> and <paramref name="operationId"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resourceId">The ID of the <see cref="ProtectedResource"/>.</param>
		/// <param name="operationId">The ID of the <see cref="ProtectedOperation"/>.</param>
		/// <returns>Returns the created <see cref="ProtectedOperation"/>.</returns>
		public static ProtectedOperation Create(IMansionContext context, string resourceId, string operationId)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(resourceId))
				throw new ArgumentNullException("resourceId");
			if (string.IsNullOrEmpty(operationId))
				throw new ArgumentNullException("operationId");

			// create the resource
			var resource = new ProtectedResource(resourceId);

			// create and return the operation
			return new ProtectedOperation(operationId, resource);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of this protected operation.
		/// </summary>
		public string Id
		{
			get { return id; }
		}
		/// <summary>
		/// Gets/Sets the friendly name of this protected operation.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Gets the <see cref="ProtectedResource"/> to on which this operation is performed.
		/// </summary>
		public ProtectedResource Resource
		{
			get { return resource; }
		}
		#endregion
		#region Private Fields
		private readonly string id;
		private readonly ProtectedResource resource;
		#endregion
	}
}