using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ScrollerEngine;
using ScrollerEngine.Components.Graphics;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Mary, Richard, Emmanuel
    /// Determines what state the AI is in.
    /// </summary>
    public enum AIState
    {
        Searching,
        Attacking
    }

    /// <summary>
    /// A AIComponent used for managing shooting enemies.
    /// </summary>
    public class ShooterAIComponent : AIComponent
    {
        private float _ProjectileSpeed = 0f;
        private float _AttackDelay = 0f;
        private Direction _ShotDirection = Direction.None;
        private AIState _CurrentState = AIState.Searching;
        private DateTime _AttackStarted;
        private Player _TargetedPlayer;

        /// <summary>
        /// Gets or sets the attack delay.
        /// </summary>
        public float AttackDelay
        {
            get { return _AttackDelay; }
            set { _AttackDelay = value; }
        }

        /// <summary>
        /// Gets or sets how fast the projectile will move.
        /// </summary>
        public float ProjectileSpeed
        {
            get { return _ProjectileSpeed; }
            set { _ProjectileSpeed = value; }
        }

        /// <summary>
        /// Gets or sets the shot direction.
        /// Set equal to None if we want it to lock on to the player.
        /// </summary>
        public Direction ShotDirection
        {
            get { return _ShotDirection; }
            set { _ShotDirection = value; }
        }

        protected override void OnAiUpdate(GameTime gameTime)
        {
            //Debugger.OutlineRectangle("AI", this.ReactionBox);
            if (_ShotDirection != Direction.None)
            {
                if ((DateTime.Now - _AttackStarted).TotalSeconds < AttackDelay)
                    return;
                _AttackStarted = DateTime.Now;
                this.Parent.Scene.AddEntity(CreateProjectile(_ShotDirection.UnitVector()));
            }
            else
            {
                //Logic to lock on to the players
                switch (_CurrentState)
                {
                    case AIState.Searching:
                        foreach (var player in ScrollerBase.Instance.Players)
                        {
                            if (ReactionBox.Intersects(player.Character.Location) && !player.Character.IsDisposed)
                            {
                                _CurrentState = AIState.Attacking;
                                //_AttackStarted = DateTime.Now;
                                _TargetedPlayer = player;
                                break;
                            }
                        }
                        break;
                    case AIState.Attacking:
                        if (!ReactionBox.Intersects(_TargetedPlayer.Character.Location) || _TargetedPlayer.Character.IsDisposed)
                            _CurrentState = AIState.Searching;
                        else
                        {
                            if ((DateTime.Now - _AttackStarted).TotalSeconds < AttackDelay)
                                return;
                            _AttackStarted = DateTime.Now;
                            var unitV = Vector2.Normalize(_TargetedPlayer.Character.Center - this.Parent.Center);
                            this.Parent.Scene.AddEntity(CreateProjectile(unitV));
                        }
                        break;
                }
            }
        }

        private Entity CreateProjectile(Vector2 unitV)
        {
            Entity e = new Entity();
            e.Size = new Vector2(16, 16);
            e.Position = Parent.Center - (e.Size / 2);

            var SC = new SpriteComponent();
            SC.TextureName = "Sprites/Misc/bullet";
            SC.Width = 16;
            SC.Height = 16;
            SC.AddAnimation("blast", 0, 0, 24, 20, 3.0f, 1, 32);
            SC.CurrentAnimation = "blast";
            SC.ColorTint = Color.Blue;
            SC.isMirrored = true;
            e.Components.Add(SC);

            var CC = new ClassificationComponent();
            CC.Classification = EntityClassification.Projectile;
            e.Components.Add(CC);

            var PC = new PhysicsComponent();
            PC.GravityCoefficient = 0;
            PC.HorizontalDragCoefficient = 0;
            PC.IsGrounded = false;
            PC.Velocity = unitV * ProjectileSpeed;
            e.Components.Add(PC);

            var PrC = new ProjectileComponent();
            PrC.Classification = EntityClassification.Player | EntityClassification.Enemy;
            PrC.DisposeOnCollision = true;
            PrC.Damage = 1;
            PrC.Shooter = this.Parent;
            e.Components.Add(PrC);

            // Not sure if I want this? 
            // I want the projectile to be destroyed upon collision with tile/player/enemy
            var DOC = new DestroyableObjectComponent();
            e.Components.Add(DOC);

            return e;
        }
    }
}
