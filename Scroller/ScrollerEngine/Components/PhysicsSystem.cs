using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ScrollerEngine.Components.Graphics;
using ScrollerEngine.Scenes;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Jonathan, Richard
    /// Provides a System used to manage physics for Entities that have a PhysicsComponent.
    /// </summary>
    public class PhysicsSystem : SceneSystem
    {
        private float _Gravity = 20000;
        private float _HorizontalDrag = 2000;
        private HashSet<CollisionInfo> _PreviousCollisions = new HashSet<CollisionInfo>();

        /// <summary>
        /// Gets or sets the force of gravity.
        /// This number is currently fairly arbitrary, and should be tweaked as appropriate.
        /// </summary>
        public float Gravity
        {
            get { return _Gravity; }
            set { _Gravity = value; }
        }

        /// <summary>
        /// Gets or sets how much to slow each Entity down in the horizontal direction each frame.
        /// This is essentially the force of Gravity, but horizontally, and applies both when grounded and when not.
        /// </summary>
        public float HorizontalDrag
        {
            get { return _HorizontalDrag; }
            set { _HorizontalDrag = value; }
        }

        /// <summary>
        /// Indicates if a solid tile exists at the given location, or if that location is beneath the map and thus considered solid as well.
        /// </summary>
        public bool IsLocationSolid(Vector2 location)
        {
            foreach (var layer in Scene.Layers)
            {
                if (layer.IsSolid && layer.GetTileAtPosition(location) != null)
                    return true;
            }
            if (location.Y >= Scene.MapSize.Y - Scene.TileSize.Y)
                return true;
            return false;
        }

        /// <summary>
        /// Indicates if a solid tile exists at the given location, or if that location is beneath the map and thus considered solid as well.
        /// </summary>
        public bool IsLocationSolid(Rectangle location)
        {
            foreach (var layer in Scene.Layers)
            {
                if (layer.IsSolid && layer.GetTileAtPosition(location) != null)
                    return true;
            }
            if (location.Y >= Scene.MapSize.Y - Scene.TileSize.Y)
                return true;
            return false;
        }

        /// <summary>
        /// Indicates if a solid tile exists at the given location, or if that location is beneath the map and thus considered solid as well.
        /// Returns the Layer that was found.
        /// </summary>
        public bool IsLocationSolid(Rectangle location, ref Layer layerCollided)
        {
            foreach (var layer in Scene.Layers)
            {
                if (layer.IsSolid && layer.GetTileAtPosition(location) != null)
                {
                    layerCollided = layer;
                    return true;
                }
            }
            if (location.Y >= Scene.MapSize.Y - Scene.TileSize.Y)
                return true;
            return false;
        }

        /// <summary>
        /// Returns all Entities that overlap the specified location.
        /// </summary>
        public IEnumerable<Entity> GetEntitiesAtLocation(Rectangle Location)
        {
            return Scene.Entities.Where(c => c.Location.Intersects(Location));
        }
        
        protected override void OnUpdate(GameTime gameTime)
        {
            PerformDynamicCollision(gameTime);
            PerformStaticCollision(gameTime);
        }

        private void PerformStaticCollision(GameTime gameTime)
        {
            foreach (var PC in this.GetFilteredComponents<PhysicsComponent>())
            {
                var parent = PC.Parent;
                //update y
                if (!PC.IsGrounded)
                {
                    if (PC.VelocityY <= PC.TerminalVelocity) 
                        PC.VelocityY += Gravity * gameTime.GetTimeScalar() * PC.GravityCoefficient;
                }
                //update x
                float currSign = Math.Sign(PC.VelocityX);
                PC.VelocityX += -currSign * HorizontalDrag * PC.HorizontalDragCoefficient * gameTime.GetTimeScalar();
                if (Math.Sign(PC.VelocityX) != currSign)
                    PC.VelocityX = 0;

                var positionDelta = PC.Velocity * gameTime.GetTimeScalar();
                //first, handle falling. (vertical collision)
                if (PC.VelocityY >= 0 && CheckStaticCollision(PC, new Vector2(0, (parent.Size.Y / 2) + positionDelta.Y), (tile) => new Vector2(parent.Position.X, tile.Location.Top - parent.Size.Y)))
                {
                    PC.VelocityY = 0;
                    positionDelta.Y = 0;
                    PC.IsGrounded = true;
                }
                else
                {
                    //otherwise we're not grounded and check for collision above.
                    PC.IsGrounded = false;
                    if (PC.VelocityY < 0 && CheckStaticCollision(PC, new Vector2(0, (-parent.Size.Y / 2) + positionDelta.Y), (tile) => new Vector2(parent.Position.X, tile.Location.Bottom)))
                    {
                        PC.VelocityY = 0;
                        positionDelta.Y = 0;
                    }
                }

                //now handle horizontal collision
                if (PC.VelocityX >= 0.001f && CheckStaticCollision(PC, new Vector2((parent.Size.X / 2) + positionDelta.X + parent.CollisionBufferX, 0), (tile) => new Vector2(tile.Location.Left - parent.Size.X - parent.CollisionBufferX, parent.Position.Y)))
                {
                    positionDelta.X = 0;
                    PC.VelocityX = 0;
                }
                else if (PC.VelocityX <= 0.001f && CheckStaticCollision(PC, new Vector2((-parent.Size.X/ 2) + positionDelta.X - parent.CollisionBufferX, 0), (tile) => new Vector2(tile.Location.Right + parent.CollisionBufferX, parent.Position.Y)))
                {
                    positionDelta.X = 0;
                    PC.VelocityX = 0;
                }

                parent.Position += positionDelta;
              //  parent.Y = Math.Max(0, parent.Y);
                parent.X = Math.Max(-this.Scene.TileSize.X / 2, parent.X);
                parent.X = Math.Min(this.Scene.MapSize.X - Scene.TileSize.X, parent.X);
                if (parent.Y > this.Scene.MapSize.Y - this.Scene.TileSize.Y)
                {
                    parent.Y = this.Scene.MapSize.Y - this.Scene.TileSize.Y;
                    PC.VelocityY = 0;
                    PC.IsGrounded = true;
                }
                else if (parent.Y < 0)
                {
                    parent.Y = 0;
                    PC.VelocityY = 0;
                }
            }
        }

        private bool CheckStaticCollision(PhysicsComponent pc, Vector2 offset, Func<Tile, Vector2> locationAdjustor)
        {
            var tile = GetSolidTile(pc, offset);
            if (tile != null)
            {
                var adjustedPosition = locationAdjustor(tile);
                pc.Parent.Position = adjustedPosition;
                return true;
            }
            return false;
        }

        private Tile GetSolidTile(PhysicsComponent pc, Vector2 offset)
        {
            Vector2 offsetLocation = pc.Parent.Position + pc.Parent.Size / 2 + offset;
            Rectangle offsetRectangle = new Rectangle(
                (int)(offset.Y != 0 ? pc.Parent.Position.X - pc.Parent.CollisionBufferX : offsetLocation.X),
                (int)(offset.X != 0 ? pc.Parent.Position.Y : offsetLocation.Y),
                (int)(offset.Y == 0 ? 0 : pc.Parent.Size.X + (pc.Parent.CollisionBufferX * 2) - 1),
                (int)(offset.X == 0 ? 0 : pc.Parent.Size.Y - 1)
                );
            // THe X point of the rectangle pushes the left edge inwards by 1 CollisionBuffer.
            // That means the right side of the rectangle is now off by two CollisionBuffers, 
            // hence the subtraction of (CollisionBuffer * 2)

            foreach (var layer in Scene.Layers.Where(c => c.IsSolid))
            {
                Tile tile = layer.GetTileAtPosition(offsetRectangle);//offsetLocation);//
                if (tile != null)
                {
                    if (layer.TopCollisionOnly && tile.Location.Top < pc.Parent.Location.Bottom)
                        continue;
                    return tile;
                }
            }
            return null;
        }

        private void PerformDynamicCollision(GameTime gameTime)
        {
            var collisions = new HashSet<CollisionInfo>();
            var allComponents = GetFilteredComponents<PhysicsComponent>();
            foreach (var first in allComponents)
            {
                foreach (var second in allComponents)
                {
                    if (second == first)
                        continue;
                    if (CheckDynamicCollision(first, second))
                        collisions.Add(new CollisionInfo() { First = first, Second = second });
                }
            }

            var currentCollisions = new HashSet<CollisionInfo>();
            foreach (var collision in collisions)
            {
                if(!_PreviousCollisions.Contains(collision))
                {
                    if (!collision.First.IsDisposed && !collision.Second.IsDisposed)
                        collision.First.NotifyCollision(collision.Second);
                    //TODO: possible to implement momentum when colliding with entities and so on.
                }
                currentCollisions.Add(collision);
            }

            _PreviousCollisions = currentCollisions;
        }

        private bool CheckDynamicCollision(PhysicsComponent first, PhysicsComponent second)
        {
            var firstRect = first.Parent.Location;
            var secondRect = second.Parent.Location;

            SpriteComponent SC1 = first.SC;
            SpriteComponent SC2 = second.SC;

            if (SC1 != null && SC1.HurtBox != Rectangle.Empty)
                firstRect = SC1.HurtBox;
            if (SC2 != null && SC2.HurtBox != Rectangle.Empty)
                secondRect = SC2.HurtBox;

            return firstRect.Intersects(secondRect);
        }

        private struct CollisionInfo
        {
            public PhysicsComponent First;
            public PhysicsComponent Second;

            public override int GetHashCode()
            {
                return First.GetHashCode() ^ Second.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                CollisionInfo ci = (CollisionInfo)obj;
                return ci.First == First && ci.Second == Second;
            }
        }
    }
}
