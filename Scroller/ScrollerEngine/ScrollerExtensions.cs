using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ScrollerEngine.Components;

namespace ScrollerEngine
{
    /// <summary>
    /// Worked on by Peter, Richard, Jonathan
    /// Provides extensions used with ScrollerEngine.
    /// </summary>
    public static class ScrollerExtensions
    {
        /// <summary>
        /// Gets a scalar to multiply any movements by to make them based per-second.
        /// </summary>
        public static float GetTimeScalar(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Sets the render target and clears it with the specified color.
        /// </summary>
        public static void SetRenderTarget(this GraphicsDevice graphicsDevice, RenderTarget2D target, Color clearColor)
        {
            graphicsDevice.SetRenderTarget(target);
            graphicsDevice.Clear(clearColor);
        }

        /// <summary>
        /// Loads a Texture2D either from within the game (i.e. from ScrollerContent) or externally (i.e. From your desktop).
        /// If loaded externally, file must be absolute (example, C:\\Some\\Folder\\picture.png).
        /// Remarks: Not thoroughly tested. So if some goes wrong, do it the old way.
        /// </summary>
        public static Texture2D LoadTexture2D(this ContentManager content, string file)
        {
            if (!Path.IsPathRooted(file))
                try
                {
                    return content.Load<Texture2D>(file);
                }
                catch
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                        return Texture2D.FromStream(ScrollerBase.Instance.GraphicsDevice, fs);
                }
            else
                using (FileStream fs = new FileStream(file, FileMode.Open))
                return Texture2D.FromStream(ScrollerBase.Instance.GraphicsDevice, fs); 
            //TODO: Implement some kind of caching system if performance needs it.
        }

        /// <summary>
        /// Determines whether this rectangle contains the specified point.
        /// </summary>
        public static bool Contains(this Rectangle rectangle, Vector2 position)
        {
            return rectangle.Contains(new Point((int)position.X, (int)position.Y));
        }

        /// <summary>
        /// Gets the center of the texture.
        /// </summary>
        public static Vector2 GetCenter(this Texture2D texture)
        {
            return new Vector2(texture.Width / 2, texture.Height / 2);
        }

        /// <summary>
        /// Gets the direction with respect to an object.
        /// Returns Left or Right.
        /// </summary>
        public static Direction GetDirectionWRTEntity(this Entity wrt, Entity obj)
        {
            var dir = wrt.Location.Center.X - obj.Location.Center.X;
            return (dir < 0) ? Direction.Right : Direction.Left;
        }

        /// <summary>
        /// Returns the sign corresponding to this direction.
        /// That is, for Up and Left this would return -1, and for Down and Right this would return 1.
        /// For None, this returns 0.
        /// </summary>
        public static int GetSign(this Direction Direction)
        {
            if (Direction == Direction.Left || Direction == Direction.Up)
                return -1;
            if (Direction == Direction.Right || Direction == Direction.Down)
                return 1;
            return 0;
        }

        /// <summary>
        /// Reverses the direction.
        /// </summary>
        public static Direction Reverse(this Direction Direction)
        {
            switch (Direction)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
            }
            return Direction.None;
        }

        /// <summary>
        /// Gets the unit vector corresponding to the direction. 
        /// </summary>
        public static Vector2 UnitVector(this Direction direction)
        {
            Vector2 unitV = new Vector2();

            if (direction == Direction.Right || direction == Direction.Left)
                unitV.X = direction.GetSign();
            if (direction == Direction.Up || direction == Direction.Down)
                unitV.Y = direction.GetSign();
            unitV.Normalize();
            return unitV;
        }
    }
}
