using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using ScrollerEngine.Components.Graphics;

namespace ScrollerEngine.Components
{
    
    //NOTE: this is a temp class used as an example for how to create CollisionEvents.
    /// <summary>
    /// Worked on by Richard, Jonathan
    /// </summary>
    public class DisposeComponent : CollisionEventComponent
    {
        protected override bool OnCollision(Entity Entity, EntityClassification Classification)
        {
            //Call 'Dispose' to kill an entity.
            //Note: there are two Disposes. One for Entity and one for Component.
            //      Component Dispose only removes the component.
            //      Entity Dispose removes the entire entity (and all it's components.)
            //Entity.Dispose();
            var hc = Entity.GetComponent<HealthComponent>();
            hc.DamageEntity(9000);
            //Entity.GetComponent<SpriteComponent>().Flicker(5);
            return true;
        }

        public bool IsTest { get; set; }
    }
}
