using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScrollerEngine.Components
{
    class PushCollisionComponent : CollisionEventComponent
    {
        float explosionStrength = 900.0f;

        protected override bool OnCollision(Entity Entity, EntityClassification Classification)
        {
            Debugger.DrawContent(Entity.Name + " and " + this.Parent.Name + " Push!", "#Awesome");
            Vector2 EntityCenter = Entity.Center;
            Vector2 ParentCenter = Parent.Center;

            Vector2 direction = EntityCenter - ParentCenter;
            direction.Normalize();

            direction *= explosionStrength;

            Entity.GetComponent<PhysicsComponent>().Velocity = direction;
            return true;
        }

        public bool IsTest { get; set; }
        protected override void OnUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            if (IsTest != null)
                Debugger.DrawContent("TESTING:", IsTest);
        }
    }
}
