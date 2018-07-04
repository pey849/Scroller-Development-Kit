using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ScrollerEngine;
using ScrollerEngine.Graphics;
using ScrollerEngine.Components.Graphics;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Richard
    /// A base component for enemy AIs.
    /// </summary>
    public abstract class AIComponent : Component
    {
        private bool _IsAiEnabled = true;
        private bool _UpdateOnlyOnScreen = false;
        private Vector2 _ReactionRange = Vector2.Zero;
        private Vector2 _Offset = Vector2.Zero;
        private EntityClassification _EntitiesToSearchFor = EntityClassification.Any;

        protected MovementComponent MC;
        protected PhysicsComponent PC;
        protected PhysicsSystem PS;

        /// <summary>
        /// Gets or sets whether the ai is enabled.
        /// </summary>
        public bool IsAiEnabled
        {
            get { return _IsAiEnabled; }
            set { _IsAiEnabled = value; }
        }

        /// <summary>
        /// Gets or sets whether if this AIComponent should update when it is visible on screen. 
        /// </summary>
        public bool UpdateOnlyOnScreen
        {
            get { return _UpdateOnlyOnScreen; }
            set { _UpdateOnlyOnScreen = value; }
        }

        /// <summary>
        /// Gets or sets the reaction range with respect to the center of the entity.
        /// </summary>
        public Vector2 ReactionRange
        {
            get { return _ReactionRange; }
            set { _ReactionRange = value; }
        }

        /// <summary>
        /// Gets or sets how much to offset the reaction rectangle.
        /// </summary>
        public Vector2 Offset
        {
            get { return _Offset; }
            set { _Offset = value; }
        }

        /// <summary>
        /// Gets or sets which entities to search for.
        /// </summary>
        public EntityClassification EntitiesToSearchFor
        {
            get { return _EntitiesToSearchFor; }
            set { _EntitiesToSearchFor = value; }
        }

        /// <summary>
        /// Gets the reaction box
        /// </summary>
        protected virtual Rectangle ReactionBox
        {
            get
            {
                var center = this.Parent.Center;
                var dirSign = MC.CurrentDirection.GetSign();
                var rectPos = new Vector2((center.X + (dirSign * Offset.X)) - ReactionRange.X / 2, (center.Y + Offset.Y) - ReactionRange.Y / 2);
                return new Rectangle((int)rectPos.X, (int)rectPos.Y, (int)ReactionRange.X, (int)ReactionRange.Y);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            MC = this.GetDependency<MovementComponent>();
            PC = this.GetDependency<PhysicsComponent>();
            PS = this.Scene.GetSystem<PhysicsSystem>();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            if (!IsAiEnabled)
                return;
            if (UpdateOnlyOnScreen)
            {
                if (!CameraManager.Active.Contains(this.Parent))
                    return;
            }
            OnAiUpdate(gameTime);
        }

        protected abstract void OnAiUpdate(GameTime gameTime);
    }
}
