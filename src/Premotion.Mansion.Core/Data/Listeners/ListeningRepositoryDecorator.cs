using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
		/// <param name="query">The query on the node.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		protected override Node DoRetrieveSingle(IMansionContext context, NodeQuery query)
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
		/// <param name="query">The query on the node.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		protected override Nodeset DoRetrieve(IMansionContext context, NodeQuery query)
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
		protected override Node DoCreate(IMansionContext context, Node parent, IPropertyBag newProperties)
		{
			// get the type of this node
			var nodeType = newProperties.Get<ITypeDefinition>(context, "type");
			var nodeListeners = GetListeners(nodeType);

			// fire the on before create
			foreach (var listener in nodeListeners)
				listener.BeforeCreate(context, parent, newProperties);

			// execute the derived class
			var node = DecoratedRepository.Create(context, parent, newProperties);

			// fire the on after create
			foreach (var listener in nodeListeners)
				listener.AfterCreate(context, parent, node, newProperties);

			// return the created node
			return node;
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		protected override void DoUpdate(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// get the type of this node
			var nodeType = typeService.Load(context, node.Pointer.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before update
			foreach (var listener in nodeListeners)
				listener.BeforeUpdate(context, node, modifiedProperties);

			// execute the derived class
			DecoratedRepository.Update(context, node, modifiedProperties);

			// fire the on after update
			foreach (var listener in nodeListeners)
				listener.AfterUpdate(context, node, modifiedProperties);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be deleted.</param>
		protected override void DoDelete(IMansionContext context, NodePointer pointer)
		{
			// get the type of this node
			var nodeType = typeService.Load(context, pointer.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before delete
			foreach (var listener in nodeListeners)
				listener.BeforeDelete(context, pointer);

			// execute the derived class
			DecoratedRepository.Delete(context, pointer);
		}
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		protected override Node DoMove(IMansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			// get the type of this node
			var nodeType = typeService.Load(context, pointer.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before move
			var originalParentPointer = pointer.Parent;
			foreach (var listener in nodeListeners)
				listener.BeforeMove(context, originalParentPointer, newParentPointer, pointer);

			// execute the derived class
			var node = DecoratedRepository.Move(context, pointer, newParentPointer);

			// fire the on after move
			foreach (var listener in nodeListeners)
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
		protected override Node DoCopy(IMansionContext context, NodePointer pointer, NodePointer targetParentPointer)
		{
			// get the type of this node
			var nodeType = typeService.Load(context, pointer.Type);
			var nodeListeners = GetListeners(nodeType);

			// fire the on before copy
			var originalParentPointer = pointer.Parent;
			foreach (var listener in nodeListeners)
				listener.BeforeCopy(context, originalParentPointer, targetParentPointer, pointer);

			// execute the derived class
			var node = DecoratedRepository.Copy(context, pointer, targetParentPointer);

			// fire the on after copy
			foreach (var listener in nodeListeners)
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
				foreach (var listener in type.GetDescriptors<RegisterListenerDescriptor>().Select(descriptor => descriptor.ListenerType.CreateInstance<NodeListener>(context.Nucleus)))
				{
					// add the listener to the type
					var listener1 = listener;
					listeners.AddOrUpdate(type, key =>
					                            {
					                            	var typeListeners = new List<NodeListener> {listener1};
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
		#endregion
		#region  Helper Methods
		/// <summary>
		/// Gets the <see cref="NodeListener"/>s for type <paramref name="nodeType"/>.
		/// </summary>
		/// <param name="nodeType">The type for which to get the listeners.</param>
		/// <returns>Returns the listeners for this particular type.</returns>
		private List<NodeListener> GetListeners(ITypeDefinition nodeType)
		{
			// validate arguments
			if (nodeType == null)
				throw new ArgumentNullException("nodeType");

			// loop through all the registered type listeners
			return listeners.Where(candidate => nodeType.IsAssignable(candidate.Key)).SelectMany(candidate => candidate.Value).ToList();
		}
		#endregion
		#region Private Fields
		private readonly ConcurrentDictionary<ITypeDefinition, List<NodeListener>> listeners = new ConcurrentDictionary<ITypeDefinition, List<NodeListener>>();
		private readonly ITypeService typeService;
		#endregion
	}
}