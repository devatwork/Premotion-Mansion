using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Implements <see cref="BaseStorageEngine"/> using SQL-server.
	/// </summary>
	public class SqlServerStorageEngine : BaseStorageEngine
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="SqlServerStorageEngine"/>.
		/// </summary>
		/// <param name="repository">The <see cref="SqlServerRepository"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="repository"/> is null.</exception>
		public SqlServerStorageEngine(SqlServerRepository repository)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");

			// set values
			this.repository = repository;
		}
		#endregion
		#region Overrides of BaseStorageEngine
		/// <summary>
		/// Creates a new record with the given <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties from which to create a record.</param>
		/// <returns>Returns the created <see cref="Record"/>.</returns>
		protected override Record DoCreate(IMansionContext context, IPropertyBag properties)
		{
			return repository.Create(context, properties);
		}
		/// <summary>
		/// Updates an existing <paramref name="record"/> in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be updated.</param>
		/// <param name="properties">The updated properties.</param>
		protected override Record DoUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			// update the node
			repository.Update(context, record, properties);

			// merge the properties
			record.Merge(properties);

			// return the updated node
			return record;
		}
		/// <summary>
		/// Deletes an existing <paramref name="record"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be deleted.</param>
		protected override void DoDelete(IMansionContext context, Record record)
		{
			repository.Delete(context, record);
		}
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="properties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		protected override Node DoCreateNode(IMansionContext context, Node parent, IPropertyBag properties)
		{
			return repository.CreateNode(context, parent, properties);
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="properties">The properties which to update.</param>
		protected override Node DoUpdateNode(IMansionContext context, Node node, IPropertyBag properties)
		{
			// update the node
			repository.UpdateNode(context, node, properties);

			// merge the properties
			node.Merge(properties);

			// return the updated node
			return node;
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The pointer to the node which will be deleted.</param>
		protected override void DoDeleteNode(IMansionContext context, Node node)
		{
			repository.DeleteNode(context, node);
		}
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		protected override Node DoMoveNode(IMansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			return repository.MoveNode(context, pointer, newParentPointer);
		}
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		protected override Node DoCopyNode(IMansionContext context, NodePointer pointer, NodePointer targetParentPointer)
		{
			return repository.CopyNode(context, pointer, targetParentPointer);
		}
		#endregion
		#region Private Fields
		private readonly SqlServerRepository repository;
		#endregion
	}
}