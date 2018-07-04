using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ScrollerEngine.Components
{

    /// <summary>
    /// Worked on by Emmanuel, Peter, Richard
    /// Called when the location of an Entity changes (either the position or the size).
    /// </summary>
    public delegate void EntityLocationChangeDelegate(Entity Entity, Vector2 OldValue, Vector2 NewValue);

    /// <summary>
    /// Represents a single entity in the game.
    /// This class only provides a basic position within the world, leaving the remainder of game logic to be provided by Components.
    /// </summary>
    public class Entity : SceneObject
    {
        /// <summary>
        /// Gets an event called when the position of this entity changes.
        /// </summary>
        public event EntityLocationChangeDelegate PositionChanged;

        /// <summary>
        /// Gets an event called when the size of this entity changes.
        /// </summary>
        public event EntityLocationChangeDelegate SizeChanged;

        private Vector2 _Position;
        private Vector2 _Size;
        private ComponentCollection _Components;
        private Vector2 _CollisionBuffer;

        /// <summary>
        /// Gets the Components that this Entity contains.
        /// </summary>
        public ComponentCollection Components { 
            get { return _Components; }
            set { _Components = value; }
        }

        /// <summary>
        /// Gets or sets the size of this entity, in world space units.
        /// </summary>
        public Vector2 Size
        {
            get { return _Size; }
            set
            {
                if (_Size == value)
                    return;
                Vector2 Old = _Size;
                _Size = value;
                if (SizeChanged != null)
                    SizeChanged(this, Old, _Size);
            }
        }

        /// <summary>
        /// Gets or sets the position of this entity, in world space units.
        /// </summary>
        [ContentSerializerIgnore]
        public Vector2 Position
        {
            get { return _Position; }
            set
            {
                if (_Position == value)
                    return;
                Vector2 Old = _Position;
                _Position = value;
                if (PositionChanged != null)
                    PositionChanged(this, Old, _Position);
            }
        }

        /// <summary>
        /// Gets the X coordinate of this Player.
        /// This translates calls to the Position property, and is simply short-hand for it.
        /// </summary>
        [ContentSerializerIgnore]
        public float X
        {
            get { return _Position.X; }
            set { Position = new Vector2(value, Position.Y); }
        }

        /// <summary>
        /// Gets the Y coordinate of this Player.
        /// This translates calls to the Position property, and is simply short-hand for it.
        /// </summary>
        [ContentSerializerIgnore]        
        public float Y
        {
            get { return _Position.Y; }
            set { Position = new Vector2(Position.X, value); }
        }

        /// <summary>
        /// Gets the Y buffer to shrink the top AND bottom of the Rectangle by.
        /// Generally not too useful, would intefere with feet/falling collision.
        /// </summary>
        [ContentSerializerIgnore] 
        public float CollisionBufferY
        {
            get { return _CollisionBuffer.Y; }
            set { _CollisionBuffer = new Vector2(_CollisionBuffer.X, value); }
        }

        /// <summary>
        /// Gets the x buffer to shrink the left AND right of the Rectangle by.
        /// Generally useful, as the Location hitbox usually doesn't match the size of sprites.
        /// </summary>
        [ContentSerializerIgnore] 
        public float CollisionBufferX
        {
            get { return _CollisionBuffer.X; }
            set { _CollisionBuffer = new Vector2(value, _CollisionBuffer.Y); }
        }

        public Vector2 CollisionBuffer
        {
            get { return _CollisionBuffer; }
            set
            {
                if (_CollisionBuffer == value)
                    return;
                else
                    _CollisionBuffer = value;
            }
        }

        /// <summary>
        /// Gets the combined position and size of this entity.
        /// Note that there is potential for rounding errors due to the conversion from float to int.
        /// </summary>
        public Rectangle Location
        {
            get 
            { 
                Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
                rect.Inflate((int)CollisionBufferX, (int)CollisionBufferY);
                return rect;
            }
        }

        /// <summary>
        /// Gets the center of this entity.
        /// </summary>
        public Vector2 Center { get { return new Vector2(Location.Center.X, Location.Center.Y); } }

        /// <summary>
        /// Returns the first component with the specified type.
        /// This is simply a shortcut for (T)Components[typeof(T)].
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        public T GetComponent<T>() where T : Component
        {
            var Component = this.Components[typeof(T)];
            return (T)Component;
        }

        /// <summary>
        /// Creates a new Entity with no Components assigned.
        /// </summary>
        public Entity()
            :base()
        {
            this._Components = new ComponentCollection(this);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            foreach (var Component in this.Components)
                Component.Initialize(Scene);
        }

        /// <summary>
        /// Handles any updating of this Entity. The default implementation simply updates child Components.
        /// This is called automatically by the Scene.
        /// </summary>
        protected override void OnUpdate(GameTime Time)
        {
            for (int i = 0; i < this.Components.Count; i++)
                if (!Components[i].IsDisposed)
                    Components[i].Update(Time);
        }

        /// <summary>
        /// Draws this Entity. The default implementation simply renders all child Components.
        /// This is called automatically by the Scene.
        /// </summary>
        protected override void OnDraw()
        {
            foreach (var Component in this.Components)
                if (!Component.IsDisposed)
                    Component.Draw();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            foreach (var Component in this.Components)
                Component.Dispose();
        }

        public override string ToString()
        {
            return this.Name + " (" + this.Components.Count + " Component(s))";
        }


    }
}
