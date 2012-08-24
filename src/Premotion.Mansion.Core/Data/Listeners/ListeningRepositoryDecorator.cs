using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Listeners
{
	/// <summary>
	/// Decorates any <see cref="IRepository"/> with node event listeners.
	/// </summary>
	public class ListeningRepositoryDecorator : RepositoryDecorator
	{
		#region Constructors
		/// <summary>
		/// Constructs a listening decorated <see cref="IRepository"/>.
		/// </summary>
		/// <param name="decoratedRepository">The <see cref="IRepository"/> being decorated.</param>
		/// <param name="typeService">The <see cref="ITypeService"/>.</param>
		public ListeningRepositoryDecorator(IRepository decoratedRepository, ITypeService typeService) : base(decoratedRepository)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
		}
		#endregion
		#region Implementation of IRepository
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		protected override Node DoRetrieveSingleNode(IMansionContext context, Query query)
		{
			// retrieve the node
			var node = DecoratedRepository.RetrieveSingleNode(context, query);
			if (node == null)
				return null;

			// get the node listeners
			var nodeType = typeService.Load(context, node.Pointer.Type);
			var nodeListeners = GetListeners(nodeType);

			// attach missing property event
			node.MissingProperty += (sender, args) =>
			                        {
			                        	foreach (var nodeListener in nodeListeners)
			                        	{
			                        		object value;
			                        		if (!nodeListener.TryResolveMissingProperty(context, node, args.PropertyName, out value))
			                        			continue;
			                        		args.Value = value;
			                        		break;
			                        	}
			                        };

			// return the node
			return node;
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		protected override Nodeset DoRetrieveNodeset(IMansionContext context, Query query)
		{
			// retrieve the nodeset
			var nodeset = DecoratedRepository.RetrieveNodeset(context, query);

			// attach the event handler to each node
			foreach (var node in nodeset.Nodes)
			{
				// get the node listeners
				var nodeType = typeService.Load(context, node.Pointer.Type);
				var nodeListeners = GetListeners(nodeType);

				// attach missing property event
				var node1 = node;
				node.MissingProperty += (sender, args) =>
				                        {
				                        	foreach (var nodeListener in nodeListeners)
				                        	{
				                        		object value;
				                        		if (!nodeListener.TryResolveMissingProperty(context, node1, args.PropertyName, out value))
				                        			continue;
				                        		args.Value = value;
				                        		break;
				                        	}
				                        };
			}

			// return the nodeset
			return nodeset;
		}
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="newProperties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		protected override Node DoCreateNode(IMansionContext context, Node parent, IPropertyBag newProperties)
		{
			// get the type of this node
			var nodeType = newProperties.Get<ITypeDefinition>(context, "type");
			var nodeListeners = GetListeners(nodeType);

			// fire the on before create
			foreach (var listener in nodeListeners)
				listener.BeforeCreate(context, newProperties);

			// execute the derived class
			var node = DecoratedRepository.CreateNode(context, parent, newProperties);

			// fire the on after create
			foreach (var listener in nodeListeners)
				listener.AfterCreate(context, node, newProperties);

			// return the created node
			return node;
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		protected override void DoUpdateNode(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// get the type of this node
			var nodeType = typeService.Load(context, node.Pointer.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before update
			foreach (var listener in nodeListeners)
				listener.BeforeUpdate(context, node, modifiedProperties);

			// execute the derived class
			DecoratedRepository.UpdateNode(context, node, modifiedProperties);

			// fire the on after update
			foreach (var listener in nodeListeners)
				listener.AfterUpdate(context, node, modifiedProperties);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The pointer to the node which will be deleted.</param>
		protected override void DoDeleteNode(IMansionContext context, Node node)
		{
			// get the type of this node
			var nodeType = typeService.Load(context, node.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before delete
			foreach (var listener in nodeListeners)
				listener.BeforeDelete(context, node);

			// execute the derived class
			DecoratedRepository.DeleteNode(context, node);
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
			// get the type of this node
			var nodeType = typeService.Load(context, pointer.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before move
			var originalParentPointer = pointer.Parent;
			foreach (var listener in nodeListeners.OfType<NodeListener>())
				listener.BeforeMove(context, originalParentPointer, newParentPointer, pointer);

			// execute the derived class
			var node = DecoratedRepository.MoveNode(context, pointer, newParentPointer);

			// fire the on after move
			foreach (var listener in nodeListeners.OfType<NodeListener>())
				listener.AfterMove(context, originalParentPointer, newParentPointer, node);

			return node;
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
			// get the type of this node
			var nodeType = typeService.Load(context, pointer.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before copy
			var originalParentPointer = pointer.Parent;
			foreach (var listener in nodeListeners.OfType<NodeListener>())
				listener.BeforeCopy(context, originalParentPointer, targetParentPointer, pointer);

			// execute the derived class
			var node = DecoratedRepository.CopyNode(context, pointer, targetParentPointer);

			// fire the on after copy
			foreach (var listener in nodeListeners.OfType<NodeListener>())
				listener.AfterCopy(context, originalParentPointer, targetParentPointer, node);

			return node;
		}
		/// <summary>
		/// Starts this service. All other services are initialized.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoStart(IMansionContext context)
		{
			// loop through all their types to get the registered listeners for them
			foreach (var type in typeService.LoadAll(context))
			{
				foreach (var listener in type.GetDescriptors<RegisterListenerDescriptor>().Select(descriptor => descriptor.ListenerType.CreateInstance<RecordListener>(context.Nucleus)))
				{
					// add the listener to the type
					var listener1 = listener;
					listeners.AddOrUpdate(type, key =>
					                            {
					                            	var typeListeners = new List<RecordListener> {listener1};
					                            	return typeListeners;
					                            },
					                      (key, list) =>
					                      {
					                      	list.Add(listener1);
					                      	return list;
					                      }
						);
				}
			}

			// allow base to intialize
			base.DoStart(context);
		}
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Record"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override Record DoRetrieveSingle(IMansionContext context, Query query)
		{
			// retrieve the node
			var node = DecoratedRepository.RetrieveSingle(context, query);
			if (node == null)
				return null;

			// get the node listeners
			var nodeType = typeService.Load(context, node.Type);
			var nodeListeners = GetListeners(nodeType);

			// attach missing property event
			node.MissingProperty += (sender, args) =>
			                        {
			                        	foreach (var nodeListener in nodeListeners)
			                        	{
			                        		object value;
			                        		if (!nodeListener.TryResolveMissingProperty(context, node, args.PropertyName, out value))
			                        			continue;
			                        		args.Value = value;
			                        		break;
			                        	}
			                        };

			// return the node
			return node;
		}
		/// <summary>
		/// Retrieves a <see cref="Dataset"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing the results.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override RecordSet DoRetrieve(IMansionContext context, Query query)
		{
			// retrieve the nodeset
			var nodeset = DecoratedRepository.Retrieve(context, query);

			// attach the event handler to each node
			foreach (var node in nodeset.Records)
			{
				// get the node listeners
				var nodeType = typeService.Load(context, node.Type);
				var nodeListeners = GetListeners(nodeType);

				// attach missing property event
				var node1 = node;
				node.MissingProperty += (sender, args) =>
				                        {
				                        	foreach (var nodeListener in nodeListeners)
				                        	{
				                        		object value;
				                        		if (!nodeListener.TryResolveMissingProperty(context, node1, args.PropertyName, out value))
				                        			continue;
				                        		args.Value = value;
				                        		break;
				                        	}
				                        };
			}

			// return the nodeset
			return nodeset;
		}
		/// <summary>
		/// Creates a new record with the given <paramref name="properties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties from which to create a record.</param>
		/// <returns>Returns the created <see cref="Record"/>.</returns>
		protected override Record DoCreate(IMansionContext context, IPropertyBag properties)
		{
			// get the type of this node
			var nodeType = properties.Get<ITypeDefinition>(context, "type");
			var nodeListeners = GetListeners(nodeType);

			// fire the on before create
			foreach (var listener in nodeListeners)
				listener.BeforeCreate(context, properties);

			// execute the derived class
			var node = DecoratedRepository.Create(context, properties);

			// fire the on after create
			foreach (var listener in nodeListeners)
				listener.AfterCreate(context, node, properties);

			// return the created node
			return node;
		}
		/// <summary>
		/// Updates an existing <paramref name="record"/> in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be updated.</param>
		/// <param name="properties">The updated properties.</param>
		protected override void DoUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			// get the type of this node
			var nodeType = typeService.Load(context, record.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before update
			foreach (var listener in nodeListeners)
				listener.BeforeUpdate(context, record, properties);

			// execute the derived class
			DecoratedRepository.Update(context, record, properties);

			// fire the on after update
			foreach (var listener in nodeListeners)
				listener.AfterUpdate(context, record, properties);
		}
		/// <summary>
		/// Deletes an existing <paramref name="record"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record">The <see cref="Record"/> which will be deleted.</param>
		protected override void DoDelete(IMansionContext context, Record record)
		{
			// get the type of this node
			var nodeType = typeService.Load(context, record.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before delete
			foreach (var listener in nodeListeners)
				listener.BeforeDelete(context, record);

			// execute the derived class
			DecoratedRepository.Delete(context, record);
		}
		#endregion
		#region  Helper Methods
		/// <summary>
		/// Gets the <see cref="NodeListener"/>s for type <paramref name="nodeType"/>.
		/// </summary>
		/// <param name="nodeType">The type for which to get the listeners.</param>
		/// <returns>Returns the listeners for this particular type.</returns>
		private List<RecordListener> GetListeners(ITypeDefinition nodeType)
		{
			// validate arguments
			if (nodeType == null)
				throw new ArgumentNullException("nodeType");

			// loop through all the registered type listeners
			return listeners.Where(candidate => nodeType.IsAssignable(candidate.Key)).SelectMany(candidate => candidate.Value).ToList();
		}
		#endregion
		#region Private Fields
		private readonly ConcurrentDictionary<ITypeDefinition, List<RecordListener>> listeners = new ConcurrentDictionary<ITypeDefinition, List<RecordListener>>();
		private readonly ITypeService typeService;
		#endregion
	}
}