using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrollerEngine;
using ScrollerEngine.Components;
using Microsoft.Xna.Framework.Content;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Emmanuel, Peter, Richard
    /// Provides basic health for entities.
    /// </summary>
    public class HealthComponent : Component
    {
        /// <summary>
        /// Gets an event called when the health of this Component runs out.
        /// </summary>
        public event Action<HealthComponent> Died;

        private int _MaxHealth = 100;
        private int _CurrentHealth = 100;
        private bool _IsImmortal = false;
        private bool _IsInvincibile = false;
        private bool _IsDead = false;

        /// <summary>
        /// Gets whether this entity has died.
        /// </summary>
        public bool IsDead { get { return _IsDead; } }
        
        protected PhysicsComponent PC;

        /// <summary>
        /// The healthiest that a entity should be, barring special rules
        /// </summary>
        [ContentSerializerIgnore]
        public int CurrentHealth
        {
            get { return _CurrentHealth; }
            private set {
                if (_IsDead && value > 0)
                    return;
                _CurrentHealth = (int)Math.Min(MaxHealth, Math.Max(value, 0));
                if (_CurrentHealth == 0)
                {
                    _IsDead = true;
                    if (this.Died != null)
                        this.Died(this);
                }
            }
        }

        /// <summary>
        /// If the entity died and were resurrected, this would be the health they start at.
        /// </summary>
        public int MaxHealth
        {
            get { return _MaxHealth; }
            set {
                if (_IsDead && value > 0)
                    return;
                _MaxHealth = value;
                _CurrentHealth = _MaxHealth;
            }
        }

        /// <summary>
        /// Gets or sets whether this entity is immortal
        /// </summary>
        [ContentSerializerIgnore]
        public bool IsImmortal
        {
            get { return _IsImmortal; }
            set { _IsImmortal = value; }
        }

        /// <summary>
        /// Gets or sets whether this entity is invincible.
        /// </summary>
        public bool IsInvincible
        {
            get { return _IsInvincibile; }
            set { _IsInvincibile = value; }
        }

        /// <summary>
        /// Health above 0 and being alive are two different things. What if I want to make a zombie?
        /// </summary>
        public bool HealthAboveZero
        {
            get { return (_CurrentHealth > 0); }
        }

        /// <summary>
        /// If a negative int is given, the entity will be healed instead.
        /// </summary>
        public void DamageEntity(int dmg)
        {
            if (IsInvincible)
                return;
            if(!IsImmortal)
                CurrentHealth -= dmg;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            PC = this.GetDependency<PhysicsComponent>();
            this.Parent.Disposed += Parent_Disposed;
        }

        protected override void OnUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.OnUpdate(gameTime);
        }

        void Parent_Disposed(SceneObject obj)
        {
            this.Died = null;
            this.Parent.Disposed -= Parent_Disposed;
        }
    }
}