using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrollerEngine.Components.Graphics;
using ScrollerEngine.Graphics;
using ScrollerEngine.Components;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Peter, Richard
    /// A component to allow the player to perform an action such as pull a switch, etc.
    /// </summary>
    public class PlayerActionComponent : Component
    {
        private bool _ActionFound = false;
        private Texture2D _NotificationTexture;
        protected PhysicsSystem PS;

        protected override void OnInitialize()
        {
            PS = this.Scene.GetSystem<PhysicsSystem>();
            _NotificationTexture = _NotificationTexture = ScrollerBase.Instance.GlobalContent.Load<Texture2D>("Sprites/Misc/ActionMark");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            bool found = false;
            foreach (var e in PS.GetEntitiesAtLocation(this.Parent.Location))
            {
                if (e == this.Parent)
                    continue;

                found = true;
                break;
            }
            _ActionFound = found;
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            if (!_ActionFound)
                return;
            var rect = this.Parent.Location;
            var textureRect = _NotificationTexture.Bounds;
            var sourceRect = new Rectangle(rect.Center.X - textureRect.Width / 2, rect.Top - (textureRect.Height + 5), textureRect.Width, textureRect.Height);
            ScrollerBase.Instance.SpriteBatch.Draw(_NotificationTexture, CameraManager.Active.WorldToScreen(sourceRect), null, Color.Yellow);
        }

    }
}
