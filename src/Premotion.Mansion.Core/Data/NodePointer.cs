using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// This pointer points to a node in the repository.
	/// </summary>
	[Serializable, JsonObject(MemberSerialization.OptIn)]
	public class NodePointer : IEnumerable<NodePointer>, IEquatable<NodePointer>, IComparable<NodePointer>, IComparable
	{
		#region Constants
		/// <summary>
		/// Gets the character which is used to separate pointers.
		/// </summary>
		public const string PointerSeparator = "-";
		/// <summary>
		/// Gets the character which is used to separate paths.
		/// </summary>
		public const string PathSeparator = "~";
		/// <summary>
		/// Gets the character which is used to separate structures.
		/// </summary>
		public const string StructureSeparator = "~";
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs an instance of the node pointer class.
		/// </summary>
		/// <param name="pointer">The pointer.</param>
		/// <param name="structure">The structure.</param>
		/// <param name="path">The path.</param>
		public NodePointer(int[] pointer, string[] structure, string[] path)
		{
			// validate arguments
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (structure == null)
				throw new ArgumentNullException("structure");
			if (path == null)
				throw new ArgumentNullException("path");
			if (pointer.Length == 0)
				throw new ArgumentException("The pointer should not be empty");
			if (pointer.Length != structure.Length)
				throw new ArgumentException(string.Format("The structure ({0}) is not the same as the pointer ({1})", string.Join(StructureSeparator, structure), string.Join(PointerSeparator, pointer)), "structure");
			if (pointer.Length != path.Length)
				throw new ArgumentException(string.Format("The path ({0}) is not the same as the pointer ({1})", string.Join(PathSeparator, path), string.Join(PointerSeparator, pointer)), "path");

			// set values
			this.pointer = pointer;
			this.structure = structure;
			this.path = path;
			depth = pointer.Length;
		}
		#endregion
		#region Parse Functions
		/// <summary>
		/// Parses the input string into a node pointer.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>Returns the pointer when succesful, otherwise null.</returns>
		public static NodePointer Parse(string input)
		{
			// check for invalid strings
			if (string.IsNullOrEmpty(input))
				return null;

			// split the input
			var pointerParts = input.Split(new[] {PointerSeparator}, StringSplitOptions.RemoveEmptyEntries);
			var pointer = new int[pointerParts.Length];
			for (var index = 0; index < pointer.Length; index++)
				pointer[index] = int.Parse(pointerParts[index]);

			// create and return the pointer
			return new NodePointer(pointer, new string[pointer.Length], new string[pointer.Length]);
		}
		/// <summary>
		/// Parses the input string into a node pointer.
		/// </summary>
		/// <param name="pointerString"></param>
		/// <param name="pathString"></param>
		/// <param name="structureString"></param>
		/// <returns>Returns the pointer when succesful, otherwise null.</returns>
		public static NodePointer Parse(string pointerString, string structureString, string pathString)
		{
			// check for invalid strings
			if (string.IsNullOrEmpty(pointerString))
				return null;
			if (string.IsNullOrEmpty(structureString))
				return null;
			if (string.IsNullOrEmpty(pathString))
				return null;

			// split the pointer
			var pointerParts = pointerString.Split(new[] {PointerSeparator}, StringSplitOptions.RemoveEmptyEntries);
			var pointer = new int[pointerParts.Length];
			for (var index = 0; index < pointer.Length; index++)
				pointer[index] = int.Parse(pointerParts[index]);
			var structureParts = structureString.Split(new[] {StructureSeparator}, StringSplitOptions.RemoveEmptyEntries);
			var pathParts = pathString.Split(new[] {PathSeparator}, StringSplitOptions.RemoveEmptyEntries);

			// create and return the pointer
			return new NodePointer(pointer, structureParts, pathParts);
		}
		#endregion
		#region Relation Methods
		/// <summary>
		/// Checks whether this pointer is a child of the other pointer.
		/// </summary>
		/// <param name="other">The other pointer.</param>
		/// <returns>Returns true when this pointer is a child of the other pointer, otherwise false.</returns>
		public bool IsChildOf(NodePointer other)
		{
			// validate argument
			if (other == null)
				throw new ArgumentNullException("other");

			// first check if the child is not deeper than the parent
			if (Depth <= other.Depth)
				return false;

			// check if the child pointer contains the parent ID at the parent Depth
			return other.Id == Pointer[other.Depth - 1];
		}
		/// <summary>
		/// Checks whether this pointer is the parent of the other pointer.
		/// </summary>
		/// <param name="other">The other pointer.</param>
		/// <returns>Returns true when this pointer is the parent of the other pointer, otherwise false.</returns>
		public bool IsParentOf(NodePointer other)
		{
			// validate argument
			if (other == null)
				throw new ArgumentNullException("other");

			// first check if the child is not deeper than the parent
			if (Depth >= other.Depth)
				return false;

			// check if the child pointer contains the parent ID at the parent Depth
			return Id == other.Pointer[Depth - 1];
		}
		/// <summary>
		/// Checks whether this pointer is the parent of the other pointer.
		/// </summary>
		/// <param name="other">The other pointer.</param>
		/// <param name="level">The level on which to check.</param>
		/// <returns>Returns true when this pointer is the parent of the other pointer, otherwise false.</returns>
		public bool IsParentOf(NodePointer other, int level)
		{
			// validate argument
			if (other == null)
				throw new ArgumentNullException("other");

			// check if the depth matches up
			if (Depth + level != other.Depth)
				return false;

			return IsParentOf(other);
		}
		/// <summary>
		/// Creates a renamed version of this pointer.
		/// </summary>
		/// <param name="current">The current pointer.</param>
		/// <param name="name">The new name.</param>
		/// <returns>Returns the new pointer.</returns>
		public static NodePointer Rename(NodePointer current, string name)
		{
			// validate arguments
			if (current == null)
				throw new ArgumentNullException("current");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// get the current values
			var pointer = current.Pointer;
			var path = new string[pointer.Length];
			var structure = current.Structure;

			// set the new value
			Array.Copy(current.Path, path, path.Length - 1);
			path[path.Length - 1] = name;

			// create the new pointer
			return new NodePointer(pointer, structure, path);
		}
		/// <summary>
		/// Creates a new version of this pointer of a different type.
		/// </summary>
		/// <param name="current">The current pointer.</param>
		/// <param name="type">The new type.</param>
		/// <returns>Returns the new pointer.</returns>
		public static NodePointer ChangeType(NodePointer current, string type)
		{
			// validate arguments
			if (current == null)
				throw new ArgumentNullException("current");
			if (string.IsNullOrEmpty(type))
				throw new ArgumentNullException("type");

			// get the current values
			var pointer = current.Pointer;
			var path = current.Path;
			var structure = new string[pointer.Length];

			// set the new value
			Array.Copy(current.Structure, structure, structure.Length - 1);
			structure[structure.Length - 1] = type;

			// create the new pointer
			return new NodePointer(pointer, structure, path);
		}
		/// <summary>
		/// Changes the parent pointer of the node pointer.
		/// </summary>
		/// <param name="newParentPointer">The new parent pointer.</param>
		/// <param name="nodePointer">The current node pointer.</param>
		/// <returns>Returns the new pointer.</returns>
		public static NodePointer ChangeParent(NodePointer newParentPointer, NodePointer nodePointer)
		{
			// validate arguments
			if (newParentPointer == null)
				throw new ArgumentNullException("newParentPointer");
			if (nodePointer == null)
				throw new ArgumentNullException("nodePointer");

			// get the current values
			var pointer = new int[newParentPointer.Depth + 1];
			var path = new string[newParentPointer.Depth + 1];
			var structure = new string[newParentPointer.Depth + 1];

			// copy the parent values
			Array.Copy(newParentPointer.Pointer, pointer, pointer.Length - 1);
			Array.Copy(newParentPointer.Path, path, path.Length - 1);
			Array.Copy(newParentPointer.Structure, structure, structure.Length - 1);

			// copy current values
			pointer[pointer.Length - 1] = nodePointer.Id;
			path[path.Length - 1] = nodePointer.Name;
			structure[structure.Length - 1] = nodePointer.Type;

			// create the new pointer
			return new NodePointer(pointer, structure, path);
		}
		#endregion
		#region Relational Properties
		/// <summary>
		/// Gets the pointer to this node.
		/// </summary>
		public int[] Pointer
		{
			get { return pointer; }
		}
		/// <summary>
		/// Gets a string representation of the pointer.
		/// </summary>
		public string PointerString
		{
			get
			{
				// convert pointer parts to strings
				var parts = new string[Pointer.Length];
				for (var index = 0; index < parts.Length; index++)
					parts[index] = Pointer[index].ToString();

				return string.Join(PointerSeparator, parts);
			}
		}
		/// <summary>
		/// Gets the structure of this node containing the types of nodes.
		/// </summary>
		public string[] Structure
		{
			get { return structure; }
		}
		/// <summary>
		/// Gets a string representation of the structure.
		/// </summary>
		public string StructureString
		{
			get { return string.Join(StructureSeparator, Structure); }
		}
		/// <summary>
		/// Gets the path of this node containing all names of nodes.
		/// </summary>
		public string[] Path
		{
			get { return path; }
		}
		/// <summary>
		/// Gets a string representation of the path.
		/// </summary>
		public string PathString
		{
			get { return string.Join(PathSeparator, Path); }
		}
		/// <summary>
		/// Gets the depth of this node.
		/// </summary>
		public int Depth
		{
			get { return depth; }
		}
		#endregion
		#region Parent Properties
		/// <summary>
		/// Gets a flag indicating whether this node has a parent.
		/// </summary>
		public bool HasParent
		{
			get { return Depth > 1; }
		}
		/// <summary>
		/// Gets the parent of this node when there is one, otherwise null.
		/// </summary>
		public NodePointer Parent
		{
			get
			{
				// copy the values
				var parentPointer = new int[Depth - 1];
				var parentPath = new string[Depth - 1];
				var parentStructure = new string[Depth - 1];
				Array.Copy(Pointer, parentPointer, parentPointer.Length);
				Array.Copy(Path, parentPath, parentPath.Length);
				Array.Copy(Structure, parentStructure, parentStructure.Length);

				return new NodePointer(parentPointer, parentStructure, parentPath);
			}
		}
		/// <summary>
		/// Gets the hierarchy of this type. Top down.
		/// </summary>
		public IEnumerable<NodePointer> Hierarchy
		{
			get
			{
				// check if there is a parent
				if (HasParent)
				{
					foreach (var ancestor in Parent.Hierarchy)
						yield return ancestor;
				}

				yield return this;
			}
		}
		/// <summary>
		/// Gets the reverse hierarchy of this type. Bottom up.
		/// </summary>
		public IEnumerable<NodePointer> HierarchyReverse
		{
			get
			{
				yield return this;

				// check if there is no parent
				if (!HasParent)
					yield break;

				foreach (var ancestor in Parent.HierarchyReverse)
					yield return ancestor;
			}
		}
		#endregion
		#region Identifier Properties
		/// <summary>
		/// Gets the ID of this node.
		/// </summary>
		public int Id
		{
			get { return Pointer[Depth - 1]; }
		}
		/// <summary>
		/// Gets the type of this node.
		/// </summary>
		public string Type
		{
			get { return Structure[Depth - 1]; }
		}
		/// <summary>
		/// Gets the name of this node.
		/// </summary>
		public string Name
		{
			get { return Path[Depth - 1]; }
		}
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return PointerString;
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
			if (obj.GetType() != typeof (NodePointer))
				return false;
			return Equals((NodePointer) obj);
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
			return Id.GetHashCode();
		}
		#endregion
		#region IComparable Members
		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>. 
		/// </returns>
		/// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception><filterpriority>2</filterpriority>
		public int CompareTo(object obj)
		{
			return CompareTo(obj as NodePointer);
		}
		#endregion
		#region IComparable<NodePointer> Members
		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public int CompareTo(NodePointer other)
		{
			// guard
			if (other == null)
				return 1;

			// check if IDs match
			if (Id == other.Id)
				return 0;

			// check depth
			var depthCompare = Depth.CompareTo(other.Depth);

			// if depths are equal select the highest id
			return depthCompare != 0 ? depthCompare : Id.CompareTo(other.Id);
		}
		#endregion
		#region IEnumerable<NodePointer> Members
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<NodePointer> GetEnumerator()
		{
			if (HasParent)
			{
				foreach (var parentNodePointer in Parent)
					yield return parentNodePointer;
			}
			yield return this;
		}
		#endregion
		#region IEquatable<NodePointer> Members
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(NodePointer other)
		{
			if (ReferenceEquals(null, other))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return Equals(other.Id, Id);
		}
		#endregion
		#region Operator Overloads
		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static bool operator ==(NodePointer one, NodePointer other)
		{
			return Equals(one, other);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="one"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static bool operator !=(NodePointer one, NodePointer other)
		{
			return !Equals(one, other);
		}
		#endregion
		#region Private Fields
		[JsonProperty]
		private readonly string[] path;
		[JsonProperty]
		private readonly int[] pointer;
		[JsonProperty]
		private readonly string[] structure;
		[JsonProperty]
		private readonly int depth;
		#endregion
	}
}