using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ScrollerEngine.Components;
using ScrollerEngine.Graphics;
using ScrollerEngine.Scenes;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Peter, Richard
    /// A component to manage projectiles.
    /// </summary>
    public class ProjectileComponent : DamageCollisionComponent
    {
        private Entity _Shooter;
        private Vector2 _PrevPosition;

        protected PhysicsComponent PC;
        protected PhysicsSystem PS;

        /// <summary>
        /// Gets or sets the Entity who shot this projectile.
        /// </summary>
        [ContentSerializerIgnore]
        public Entity Shooter
        {
            get { return _Shooter; }
            set { _Shooter = value; }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            PS = this.Scene.GetSystem<PhysicsSystem>();
            PC = this.GetDependency<PhysicsComponent>();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            var inflatedLocation = this.Parent.Location;
            inflatedLocation.Inflate(1,1);
            if (_PrevPosition == this.Parent.Position || IsTileLegit(inflatedLocation))
                this.Parent.Dispose();
            _PrevPosition = this.Parent.Position;
        }

        protected override bool OnCollision(Entity Entity, EntityClassification Classification)
        {
            if (Entity == Shooter)
                return false;
            return base.OnCollision(Entity, Classification);
        }

        protected override void OnDispose()
        {
            _Shooter = null;
            base.OnDispose();
        }

        private bool IsTileLegit(Rectangle location)
        {
            Layer layer = null;
            if (PS.IsLocationSolid(location, ref layer))
            {
                if (layer == null)
                    return false;
                if (!layer.TopCollisionOnly)
                    return true;
                else
                {
                    if (PC.VelocityY < 0)
                        return false;
                    else
                        return true;
                }
            }
            return false;
        }
    }
}
