using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ScrollerEngine.Components.Graphics;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// A delegate used when a component between two entities occurs.
    /// </summary>
    public delegate void CollisionDelegate(PhysicsComponent component, PhysicsComponent other);

    /// <summary>
    /// Provides a component used to give components physics information which the PhysicsSystem uses.
    /// </summary>
    public sealed class PhysicsComponent : Component
    {
        /// <summary>
        /// An event raised when this Entity collides with a different Entity.
        /// </summary>
        public event CollisionDelegate Collided;

        private bool _IsGrounded = false;
        private float _GravityCoefficient = 1;
        private float _HorizontalDragCoefficient = 1;
        private float _TerminalVelocity = 700;
        private Vector2 _Velocity = Vector2.Zero;

        /// <summary>
        /// Gets a reference to the SpriteComponent, if any.
        /// Mainly for efficiency reasons.
        /// </summary>
        [ContentSerializerIgnore]
        public SpriteComponent SC { get; private set; }

        /// <summary>
        /// Gets or sets the velocity of this entity.
        /// </summary>
        [ContentSerializerIgnore]
        public Vector2 Velocity
        {
            get { return _Velocity; }
            set { _Velocity = value; }
        }

        /// <summary>
        /// Gets or sets the X component of the velocity.
        /// This is simply a shortcut to work around property limitations.
        /// </summary>
        [ContentSerializerIgnore]
        public float VelocityX
        {
            get { return _Velocity.X; }
            set { Velocity = new Vector2(value, Velocity.Y); }
        }

        /// <summary>
        /// Gets or sets the Y component of the velocity.
        /// This is simply a shortcut to work around property limitations.
        /// </summary>
        [ContentSerializerIgnore]
        public float VelocityY
        {
            get { return _Velocity.Y; }
            set { Velocity = new Vector2(Velocity.X, value); }
        }

        /// <summary>
        /// Gets or sets whether if this Entity is currently touching the ground.
        /// This is generally assigned by a PhysicsSystem.
        /// </summary>
        [ContentSerializerIgnore]
        public bool IsGrounded
        {
            get { return _IsGrounded; }
            set { _IsGrounded = value; }
        }

        /// <summary>
        /// Gets or sets the amount to multiply the force of gravity by.
        /// For an Entity that should not be affected by gravity, this should be 0.
        /// </summary>
        public float GravityCoefficient
        {
            get { return _GravityCoefficient; }
            set { _GravityCoefficient = value; }
        }

        /// <summary>
        /// Gets or sets the amount to multiply the force of horizontal drag by.
        /// For an Entity that should not be affected by horizontal drag, this should be 0.
        /// </summary>
        public float HorizontalDragCoefficient
        {
            get { return _HorizontalDragCoefficient; }
            set { _HorizontalDragCoefficient = value; }
        }

        /// <summary>
        /// Gets or sets the maximum speed an Entity can fall.
        /// Applies only to positive VelocityY, no restriction on how you can rise.
        /// </summary>
        public float TerminalVelocity
        {
            get { return _TerminalVelocity; }
            set { _TerminalVelocity = value; }
        }
        
        /// <summary>
        /// Notifies this PhysicsComponent that a collision occurred with the other component.
        /// This should only be called by the PhysicsSystem.
        /// </summary>
        public void NotifyCollision(PhysicsComponent Other)
        {
            if (this.Collided != null)
                this.Collided(this, Other);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            SC = this.Parent.GetComponent<SpriteComponent>();
        }

    }
}
