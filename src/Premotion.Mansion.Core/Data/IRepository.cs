﻿using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries;

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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		Node RetrieveSingleNode(IMansionContext context, Query query);
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		Nodeset RetrieveNodeset(IMansionContext context, Query query);
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="newProperties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		Node CreateNode(IMansionContext context, Node parent, IPropertyBag newProperties);
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		void UpdateNode(IMansionContext context, Node node, IPropertyBag modifiedProperties);
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The pointer to the node which will be deleted.</param>
		void DeleteNode(IMansionContext context, Node node);
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		Node MoveNode(IMansionContext context, NodePointer pointer, NodePointer newParentPointer);
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		Node CopyNode(IMansionContext context, NodePointer pointer, NodePointer targetParentPointer);
		#endregion
		#region Raw Data Methods
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Record"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		Record RetrieveSingle(IMansionContext context, Query query);
		/// <summary>
		/// Retrieves a <see cref="Dataset"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing the results.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		RecordSet Retrieve(IMansionContext context, Query query);
		/// <summary>
		/// Creates a new record with the given <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties from which to create a record.</param>
		/// <returns>Returns the created <see cref="Record"/>.</returns>
		Record Create(IMansionContext context, IPropertyBag properties);
		/// <summary>
		/// Updates an existing <paramref name="record"/> in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be updated.</param>
		/// <param name="properties">The updated properties.</param>
		void Update(IMansionContext context, Record record, IPropertyBag properties);
		/// <summary>
		/// Deletes an existing <paramref name="record"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be deleted.</param>
		void Delete(IMansionContext context, Record record);
		#endregion
		#region Start Methods
		/// <summary>
		/// Starts this object. This methods must be called after the object has been created and before it is used.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		void Start(IMansionContext context);
		#endregion
	}
}