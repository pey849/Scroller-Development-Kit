using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ScrollerEngine.Components;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Richard, Jonathan
    /// A CollisionEventComponent that causes damage when an Entity collides with it.
    /// </summary>
    public class DamageCollisionComponent : CollisionEventComponent
    {
        private int _Damage = 0;

        /// <summary>
        /// Gets or sets the amount of damage to deal.
        /// </summary>
        public int Damage
        {
            get { return _Damage; }
            set { _Damage = value; }
        }

        protected override bool OnCollision(Entity Entity, EntityClassification Classification)
        {
            var hc = Entity.GetComponent<HealthComponent>();
            if (hc == null)
                return false;
            hc.DamageEntity(Damage);
            return true;
        }
    }
}
