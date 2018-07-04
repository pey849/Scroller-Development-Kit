using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Richard
    /// Provides information about a single Component used in the Game.
    /// </summary>
    public class Component : SceneObject
    {
        private Entity _Parent;

        /// <summary>
        /// Gets the Entity that owns this Component.
        /// </summary>
        //[ContentSerializer(SharedResource = true)]
        [ContentSerializerIgnore]
        public Entity Parent
        {
            get { return _Parent; }
            set
            {
                if (_Parent != null)
                    throw new InvalidOperationException("Unable to change parent of entity after it has been set.");
                _Parent = value;
            }
        }

        /// <summary>
        /// Indicates if only one instance of this Component can exist within an Entity.
        /// The default value is true.
        /// </summary>
        [ContentSerializerIgnore]
        public virtual bool IsSingleInstance { get { return true; } }

        /// <summary>
        /// Creates a new Component with no Parent yet.
        /// </summary>
        public Component() 
        {
            this.Name = this.GetType().Name;
        }

        /// <summary>
        /// Returns the first Component of the specified type, throwing a MissingDependencyException if it's not found.
        /// In the future, this may be used to allow providing dependency graph information between Components, but that's not the case yet.
        /// </summary>
        protected T GetDependency<T>() where T : Component
        {
            var Result = Parent.Components[typeof(T)];
            if (Result == null)
                throw new MissingDependencyException(this, typeof(T));
            return (T)Result;
        }

        protected override void OnDispose()
        {
            if (Parent != null && !Parent.IsDisposed)
                Parent.Components.Remove(this.Name);
            base.OnDispose();
        }
    }
}
