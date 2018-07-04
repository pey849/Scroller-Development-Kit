using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ScrollerEngine.Components;
using ScrollerEngine.Components.Graphics;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Peter, Richard
    /// </summary>
    public class ProjectileEntityComponent : NewEntityComponent
    {
        public float _ProjectileSpeed;

        [ContentSerializerIgnore]
        public Vector2 _ProjectileVelocity = Vector2.Zero;
        [ContentSerializerIgnore]
        public Vector2 _Target = Vector2.Zero;

        private SpriteComponent SpC;

        /// <summary>
        /// A bullet is initialized here. It'll go towards wherever the _Target is.
        /// </summary>
        /// <param name="textureName"></param>
        public void CreateEntity(string textureName)
        {
            Entity e = new Entity();
            e.Size = new Vector2(64, 16);
            e.Position = Parent.Center - (e.Size / 2);

            var SC = new SpriteComponent();
            SC.TextureName = textureName;
            SC.Width = 32;
            SC.Height = 16;
            SC.AddAnimation("blast", 0, 0, 96, 12, 3.0f, 1, 32);
            SC.CurrentAnimation = "blast";
            SC.ColorTint = Color.LightBlue;
            SC.isMirrored = true;

            var CC = new ClassificationComponent();
            CC.Classification = EntityClassification.Projectile;

            var PC = new PhysicsComponent();
            PC.GravityCoefficient = 0;
            PC.HorizontalDragCoefficient = 0;
            PC.IsGrounded = false;

            _Target = Parent.Center;
            if (SpC.currentFacingDirection == Direction.Right)
            {
                Vector2 temp = Parent.Center;
                temp.X += e.Size.X / 2;
                e.Position = temp;
                _Target.X += 1;
            }
            else
            {
                Vector2 temp = Parent.Center;
                temp.X -= e.Size.X/2 + SpC.Width;
                e.Position = temp;
                _Target.X -= 1;
            }

            _ProjectileVelocity = -(Parent.Center - _Target);
            _ProjectileVelocity.Normalize();
            PC.Velocity = _ProjectileVelocity * _ProjectileSpeed;

            var PrC = new ProjectileComponent();
            PrC.Classification = EntityClassification.Enemy | EntityClassification.Player;
            PrC.DisposeOnCollision = true;
            PrC.Damage = 9000;
            PrC.Shooter = this.Parent;

            // Not sure if I want this? 
            // I want the projectile to be destroyed upon collision with tile/player/enemy
            var DOC = new DestroyableObjectComponent();

            e.Components.Add(SC);
            e.Components.Add(CC);
            e.Components.Add(PC);
            e.Components.Add(PrC);
            e.Components.Add(DOC);

            _Entities.Add(e);
            _EntityCount++;
        }

        /* Create x number of entities, all with the same properties */
        public override void CreateMultipleEntities(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateEntity();
            }
        }

        public void ShootBullet(string bulletName)
        {
            if (!Parent.IsDisposed)
            {
                CreateEntity(bulletName);
                AddEntityToScene();
            }
            
        }

        protected override void OnInitialize()
        {
            SpC = this.GetDependency<SpriteComponent>();
            base.OnInitialize();
        }

    }
}