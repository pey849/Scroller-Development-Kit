using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrollerEngine;
using ScrollerEngine.Graphics;

namespace ScrollerEngine.Components.Graphics
{
    /// <summary>
    /// Worked on by Richard, Emmanuel
    /// An class used to represent a particle.
    /// </summary>
    public class Particle
    {
        private DateTime _SpawnStarted;
        private float _OriginalSize = 0f;

        /// <summary>
        /// Gets the texture to draw.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets this particles current velocity.
        /// </summary>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        /// Gets the particles rotational angle.
        /// </summary>
        public float Angle { get; private set; }

        /// <summary>
        /// Gets the rotation speed.
        /// </summary>
        public float AngularVelocity { get; private set; }

        /// <summary>
        /// Gets the color.
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public float Size { get; private set; }

        /// <summary>
        /// Gets the Time To Live (TTL).
        /// </summary>
        public float TTL { get; private set; }

        /// <summary>
        /// Gets whether this is dead. (finished updating)
        /// </summary>
        public bool IsDead { get; private set; }
        
        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, float ttl)
        {
            this.Texture = texture;
            this.Position = position;
            this.Velocity = velocity;
            this.Angle = angle;
            this.AngularVelocity = angularVelocity;
            this.Color = color;
            this.Size = size;
            this.TTL = ttl;
            this.IsDead = false;
            _SpawnStarted = DateTime.Now;
            _OriginalSize = this.Size;
        }

        public void Update(GameTime Time)
        {
            Position += Velocity;
            Angle += AngularVelocity;

            var relativeTime = (DateTime.Now - _SpawnStarted).TotalSeconds;
            if (TTL > 0)
                Size = _OriginalSize * ((TTL - (float)relativeTime) / TTL);
            if (relativeTime > TTL)
                IsDead = true;
        }

        public void Draw()
        {
            if (!CameraManager.Active.Viewport.Contains(Position))
                return;
            ScrollerBase.Instance.SpriteBatch.Draw(Texture, CameraManager.Active.WorldToScreen(Position), null, Color, Angle, Texture.GetCenter(), Size, SpriteEffects.None, 0f);
        }
    }
}
