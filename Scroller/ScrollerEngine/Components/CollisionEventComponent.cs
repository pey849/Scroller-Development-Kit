using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ScrollerEngine.Scenes;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Richard, Jonathan
    /// Provides the base class for a Component that handles collisions with a given Entity, restricted to a specific classification.
    /// </summary>
    public abstract class CollisionEventComponent : Component
    {
        EntityClassification _Classification = EntityClassification.Any;
        private bool _DisposeOnCollision = false;
        private DateTime _LastTriggered;
        private TimeSpan _MinimumTriggerDelay;


        /// <summary>
        /// Gets or sets the classification that the Entity needs to have for the collision to occur.
        /// The default is to affect all entities.
        /// </summary>
        public virtual EntityClassification Classification
        {
            get { return _Classification; }
            set { _Classification = value; }
        }

        /// <summary>
        /// Indicates whether this component should be disposed of after a collision is handled.
        /// </summary>
        public bool DisposeOnCollision
        {
            get { return _DisposeOnCollision; }
            set { _DisposeOnCollision = value; }
        }

        /// <summary>
        /// Gets or sets the minimum delay between triggers of this component.
        /// That is, this component will not be triggered more often than this value.
        /// This property is ignored when DisposeOnCollision is true.
        /// The default value is zero, or no delay.
        /// </summary>
        public TimeSpan MinimumTriggerDelay
        {
            get { return _MinimumTriggerDelay; }
            set { _MinimumTriggerDelay = value; }
        }

        /// <summary>
        /// Gets the time that this component was last triggered, or null if it has not been triggered yet.
        /// </summary>
        [ContentSerializerIgnore]
        public DateTime? LastTriggered { get { return _LastTriggered; } }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var physics = this.GetDependency<PhysicsComponent>();
            physics.Collided += physics_Collided;
        }

        void physics_Collided(PhysicsComponent component, PhysicsComponent other)
        {
            CollisionDetected(component, other, true);
        }

        private void CollisionDetected(PhysicsComponent component, PhysicsComponent other, bool triggerRemaining)
        {
            if (this.IsDisposed)
                return;
            if (this.LastTriggered.HasValue && (DateTime.Now - this.LastTriggered.Value) < MinimumTriggerDelay)
                return;
            var classification = other.Parent.GetComponent<ClassificationComponent>();
            if (this._Classification != EntityClassification.Any && (classification == null || (classification.Classification & this.Classification) == 0))
                return;
            bool valid = OnCollision(other.Parent, classification == null ? EntityClassification.Unknown : classification.Classification);
            if (valid)
            {
                this._LastTriggered = DateTime.Now;
                if (DisposeOnCollision)
                {
                    this.Dispose();
                    if (triggerRemaining)
                    {
                        foreach (var otherEvent in Parent.Components.Select(c => c as CollisionEventComponent).Where(c => c != null && c != this).ToArray())
                            otherEvent.CollisionDetected(component, other, false);
                        Parent.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Called when a collision with the specified Entity of the given Classification occurs.
        /// Returns whether or not the collision was valid, causing this object to be disposed if DisposeOnCollision was true.
        /// </summary>
        protected abstract bool OnCollision(Entity Entity, EntityClassification Classification);
    }
}
