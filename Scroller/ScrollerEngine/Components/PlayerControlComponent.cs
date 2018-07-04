using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrollerEngine;
using ScrollerEngine.Components;
using ScrollerEngine.Components.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Peter, Richard
    /// Provides a central location to manage player input methods.
    /// Only 'Player' entities should have this.
    /// </summary>
    public class PlayerControlComponent : Component
    {
        [ContentSerializerIgnore]
        public Direction prevDirection;

        protected PhysicsComponent PC;
        protected MovementComponent MC;
        protected SpriteComponent SC;
        protected HealthComponent HC;
        protected ProjectileEntityComponent PEC;

        /// <summary>
        /// Moves the player in the specified direction.
        /// </summary>
        public void BeginMove(Direction dir)
        {
            MC.BeginMove(dir);
        }

        /// <summary>
        /// Stops the player's movement.
        /// </summary>
        public void StopMove()
        {
            MC.StopMove();
        }

        /// <summary>
        /// Start a jump.
        /// </summary>
        public void Jump(bool allowMulti)
        {
            MC.Jump(allowMulti);
        }

        /// <summary>
        /// Crouch on the ground. Only applicable if IsGrounded
        /// </summary>
        public void Crouch()
        {
            MC.Crouch();
        }

        public void Shoot(string textureName)
        {
            PEC.ShootBullet(textureName);
        }

        public void IncreaseHealth()
        {
            HC.DamageEntity(-5);
        }

        public void DecreaseHealth()
        {
            HC.DamageEntity(5);
        }

        /*
         * Return the animation associated with the player's current velocity/direction.
         * The only directions that work here are left and right. Not none, up or down
         */
        protected string getDirectionalAnimation(string animation, float velocityX, Direction dir, bool mirrored)
        {
            string curDirection = "_right";

            if (mirrored)
                return animation + curDirection;

            if (velocityX > 0)
            {
                curDirection = "_right";
            }
            else if (velocityX < 0)
            {
                curDirection = "_left";
            }
            else if (velocityX == 0)
            {
                if (dir == Direction.Right)
                    curDirection = "_right";
                else if (dir == Direction.Left)
                    curDirection = "_left";
            }

            return animation + curDirection;
        }

        protected override void OnInitialize()
        {
            PC = this.GetDependency<PhysicsComponent>();
            MC = this.GetDependency<MovementComponent>();
            SC = this.GetDependency<SpriteComponent>();
            HC = this.GetDependency<HealthComponent>();
            PEC = this.GetDependency<ProjectileEntityComponent>();
            base.OnInitialize();
        }

        protected override void OnUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            string newAnimation = "";

            if (PC.VelocityX != 0)
                newAnimation = getDirectionalAnimation("walk", PC.VelocityX, SC.currentFacingDirection, SC.isMirrored);
            else if (PC.VelocityX == 0)
                newAnimation = getDirectionalAnimation("stand", PC.VelocityX, SC.currentFacingDirection, SC.isMirrored);

            if (MC.CurrentDirection == Direction.Down && PC.IsGrounded)
                newAnimation = getDirectionalAnimation("crouch", PC.VelocityX, SC.currentFacingDirection, SC.isMirrored);

            if (PC.VelocityY < 0)
                newAnimation = getDirectionalAnimation("jump_up", PC.VelocityX, SC.currentFacingDirection, SC.isMirrored);
            else if (PC.VelocityY > 0)
                newAnimation = getDirectionalAnimation("jump_down", PC.VelocityX, SC.currentFacingDirection, SC.isMirrored);

            if (newAnimation != SC.CurrentAnimation)
                SC.CurrentAnimation = newAnimation;

            prevDirection = MC.CurrentDirection;

            base.OnUpdate(gameTime);
        }
    }
}
