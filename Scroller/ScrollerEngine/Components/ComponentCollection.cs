﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Richard, Jonathan
    /// Provides a collection of Components, accessible by name or type.
    /// </summary>
    public class ComponentCollection : KeyedCollection<string, Component>
    {
        /// <summary>
        /// Called when a Component is added to this collection.
        /// </summary>
        public event Action<Component> ComponentAdded;

        /// <summary>
        /// Called when a Component is removed from this collection.
        /// </summary>
        public event Action<Component> ComponentRemoved;

        private Entity _Entity;

        /// <summary>
        /// Gets the Entity that owns this ComponentCollection.
        /// </summary>
        //[ContentSerializer(SharedResource=true)]
        [ContentSerializerIgnore]
        public Entity Entity 
        { 
            get { return _Entity; }
            set {
                if (_Entity != null)
                    throw new InvalidOperationException("Unable to change parent of entity after it has been set.");
                _Entity = value; 
            } 
        }
        
        /// <summary>
        /// Gets the first component that is, or derives from, the given type.
        /// If no component is found with the given type, returns null.
        /// </summary>
        /// <param name="t">The type of the component to get.</param>
        public Component this[Type t]
        {
            get
            {
                foreach (var Component in this)
                {
                    var CompType = Component.GetType();
                    if (CompType == t || CompType.IsSubclassOf(t))
                        return Component;
                }
                return null;
            }
        }

        /// <summary>
        /// Used only for Serialization.
        /// </summary>
        public ComponentCollection() { }

        /// <summary>
        /// Creates a new ComponentCollection for the given Entity.
        /// </summary>
        public ComponentCollection(Entity entity)
        {
            this._Entity = entity;
        }

        /// <summary>
        /// Returns the name of this Component as a key.
        /// </summary>
        protected override string GetKeyForItem(Component item)
        {
            return item.Name;
        }

        protected override void RemoveItem(int index)
        {
            var Component = this[index];
            base.RemoveItem(index);
            PostItemRemoved(Component);
        }

        protected override void SetItem(int index, Component item)
        {
            PreItemAssigned(item);
            base.SetItem(index, item);
            PostItemRemoved(item);
            PostItemAssigned(item);
        }
        
        protected override void InsertItem(int index, Component item)
        {
            PreItemAssigned(item);
            base.InsertItem(index, item);
            PostItemAssigned(item);
        }

        private void PreItemAssigned(Component item)
        {
            if (item.Parent != null)
                throw new InvalidOperationException("Unable to move a Component from one Entity to another."); // We could remove this limitation, but that's not tested. What if something stores Parent?
            if (item.IsSingleInstance && this[item.GetType()] != null)
                throw new ArgumentException("Attempted to add a Component to the collection that was SingleInstance, yet there was already a Component of that type in the collection.");
        }

        private void PostItemAssigned(Component item)
        {
            item.Parent = this.Entity;
            if (Entity != null && Entity.IsInitialized && !item.IsInitialized)
                item.Initialize(Entity.Scene);
            if (this.ComponentAdded != null)
                this.ComponentAdded(item);
        }

        private void PostItemRemoved(Component item)
        {
            if (this.ComponentRemoved != null)
                this.ComponentRemoved(item);
            if (!item.IsDisposed)
                item.Dispose();
        }

        public override string ToString()
        {
            return "ComponentCollection (" + this.Count + " Items)";
        }

    }


    /// <summary>
    /// An exception thrown when a Component is missing a specific dependency.
    /// </summary>
    public class MissingDependencyException : Exception
    {

        /// <summary>
        /// Creates a new MissingDependencyException.
        /// </summary>
        /// <param name="Component">The component that is missing the dependency.</param>
        /// <param name="MissingType">The type of the component that was missing.</param>
        public MissingDependencyException(Component Component, Type MissingType)
            : base("Unable to find a Component of type '" + MissingType.Name + "' that '" + Component.Name + "' (" + Component.GetType().Name + ") had a dependency on.")
        {

        }
    }
}
