using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ScrollerEngine.Components;

namespace ScrollerEngine.Graphics
{
    /// <summary>
    /// Worked on by Mary, Richard
    /// Provides an implementation of a Camera that allows for determining whether to draw an object.
    /// </summary>
    public class Camera
    {
        private Vector2 _Position;
        private Vector2 _Size;
        private float _Zoom = 1.0f;

        /// <summary>
        /// Gets or sets the position of the Camera.
        /// This is the top-left most point that will be within the screen.
        /// </summary>
        public Vector2 Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        /// <summary>
        /// Gets or sets the size of the camera.
        /// </summary>
        public Vector2 Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        /// <summary>
        /// Gets or sets the camera zoom.
        /// </summary>
        public float Zoom
        {
            get { return _Zoom; }
            set { _Zoom = Math.Max(value, 1.0f); }
        }
        
        /// <summary>
        /// Gets a rectangle encompassing the Camera's viewport.
        /// Note: This method contains rounding errors. If precise position needed, then I recommend not using this.
        /// </summary>
        public Rectangle Viewport
        {
            get
            {
                var size = Size / Zoom;
                return new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y);
            }
        }

        /// <summary>
        /// Gets the size of the viewport.
        /// </summary>
        public Vector2 ViewportSize 
        {
            get
            {
                var size = Size / Zoom;
                return new Vector2(size.X, size.Y);
            }
        }

        /// <summary>
        /// Maps the given world coordinates to screen coordinates.
        /// </summary>
        public Vector2 WorldToScreen(Vector2 position)
        {
            return new Vector2(position.X - this.Position.X, position.Y - this.Position.Y);
        }

        /// <summary>
        /// Maps the given rectangle from world coordinates to screen coordinates, keeping the width and height intact.
        /// </summary>
        public Rectangle WorldToScreen(Rectangle Location)
        {
            return new Rectangle(Location.X - (int)this.Position.X, Location.Y - (int)this.Position.Y, Location.Width, Location.Height);
        }

        /// <summary>
        /// Indicates whether the viewport of this Camera contains or intersects the rectangle.
        /// </summary>
        public bool Contains(Rectangle location)
        {
            return this.Viewport.Intersects(location);
        }

        /// <summary>
        /// Indicates whether the Viewport of this Camera contains or intersects the location of the given Entity.
        /// </summary>
        public bool Contains(Entity Entity)
        {
            return this.Viewport.Intersects(Entity.Location);
        }

        /// <summary>
        /// Creates a new Camera at {0, 0}, with {ViewportWidth, ViewportHeight} as the Position and Size.
        /// </summary>
        public Camera()
        {
            this.Position = new Vector2(0, 0);
            this.Size = new Vector2(ScrollerBase.Instance.GraphicsDevice.Viewport.Width, ScrollerBase.Instance.GraphicsDevice.Viewport.Height);
        }

    }
}
