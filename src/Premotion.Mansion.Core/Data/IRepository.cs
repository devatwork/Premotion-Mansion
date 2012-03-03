using System;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Represents a repository containing nodes.
	/// </summary>
	public interface IRepository : IDisposable
	{
		#region CRUD Methods
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		Node RetrieveSingle(MansionContext context, NodeQuery query);
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		Nodeset Retrieve(MansionContext context, NodeQuery query);
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="newProperties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		Node Create(MansionContext context, Node parent, IPropertyBag newProperties);
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		void Update(MansionContext context, Node node, IPropertyBag modifiedProperties);
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be deleted.</param>
		void Delete(MansionContext context, NodePointer pointer);
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		Node Move(MansionContext context, NodePointer pointer, NodePointer newParentPointer);
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		Node Copy(MansionContext context, NodePointer pointer, NodePointer targetParentPointer);
		#endregion
		#region Parse Query Methods
		/// <summary>
		/// Parses <paramref name="arguments" /> into a <see cref="NodeQuery" />.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns the parsed query.</returns>
		NodeQuery ParseQuery(MansionContext context, IPropertyBag arguments);
		#endregion
		#region Start Methods
		/// <summary>
		/// Starts this object. This methods must be called after the object has been created and before it is used.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		void Start(MansionContext context);
		#endregion
	}
}