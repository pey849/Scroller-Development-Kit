using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Peter, Richard
    /// Provides basic movement for entities.
    /// </summary>
    public class MovementComponent : Component
    {
        private bool _IsMoving = false;
        private Direction _CurrentDirection = Direction.None;
        private float _MoveSpeed = 500f;
        private float _MoveAcceleration = 15000;
        private float _JumpSpeed = 950;

        protected PhysicsComponent PC;

        /// <summary>
        /// Gets or sets whether this entity is moving.
        /// </summary>
        [ContentSerializerIgnore]
        public bool IsMoving
        {
            get { return _IsMoving; }
            private set { _IsMoving = value; }
        }

        /// <summary>
        /// Gets or sets the move speed of this Entity, in units per second.
        /// </summary>
        public float MoveSpeed
        {
            get { return _MoveSpeed; }
            set { _MoveSpeed = value; }
        }

        /// <summary>
        /// Gets or sets the speed to increase velocity by each frame when moving.
        /// This value is in units per second, and is affected by HorizontalDrag.
        /// </summary>
        public float MoveAcceleration
        {
            get { return _MoveAcceleration; }
            set { _MoveAcceleration = value; }
        }

        /// <summary>
        /// Gets or sets the move direction of this entity.
        /// </summary>
        [ContentSerializerIgnore]
        public Direction CurrentDirection
        {
            get { return _CurrentDirection; }
            set { _CurrentDirection = value; }
        }

        /// <summary>
        /// Gets or sets how fast this Entity jumps, in units per second.
        /// Note that this does not take into consideration gravity.
        /// </summary>
        public float JumpSpeed
        {
            get { return _JumpSpeed; }
            set { _JumpSpeed = value; }
        }

        /// <summary>
        /// Causes this Entity to being walking in the given direction (left or right).
        /// Walking may then be stopped through the StopWalking method.
        /// </summary>
        public void BeginMove(Direction dir)
        {
            _IsMoving = true;
            _CurrentDirection = dir;
        }

        /// <summary>
        /// Informs the Entity to stop walking, no longer applying walk velocity.
        /// If the Entity is not walking, this method does nothing.
        /// </summary>
        public void StopMove()
        {
            _IsMoving = false;
            _CurrentDirection = Direction.None;
        }

        /// <summary>
        /// Start a jump. allowMulti means the entitiy can continuous jump in the air. Useful for debugging.
        /// </summary>
        public void Jump(bool allowMulti)
        {
            if (PC.IsGrounded || allowMulti)
                PC.VelocityY = -JumpSpeed;
            //_CurrentDirection = Direction.Up;
        }

        /// <summary>
        /// Crouch?.
        /// </summary>
        public void Crouch()
        {
            if (PC.IsGrounded)
            {
                _IsMoving = false;
                _CurrentDirection = Direction.Down;
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            PC = this.GetDependency<PhysicsComponent>();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            if (IsMoving)
            {
                Vector2 v = new Vector2();
                if (CurrentDirection == Direction.Left)
                    v.X -= Math.Max(0, Math.Min(MoveSpeed + PC.VelocityX, MoveAcceleration * gameTime.GetTimeScalar()));
                else if (CurrentDirection == Direction.Right)
                    v.X += Math.Max(0, Math.Min(MoveSpeed - PC.VelocityX, MoveAcceleration * gameTime.GetTimeScalar()));
                PC.Velocity += v;
            }
        }

    }
}
