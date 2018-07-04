using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrollerEngine.Components;
using ScrollerEngine.Components.Graphics;
using ScrollerEngine.Engine;
using ScrollerEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollerEngine.Scenes
{
    /// <summary>
    /// Worked on by Peter, Richard, Jonathan
    /// Provides access to a scene that contains the Entities and Systems for the specifed level.
    /// </summary>
    public class Scene : GameStateComponent, IDisposable
    {
        /// <summary>
        /// Gets an event called when this Scene is disposed.
        /// </summary>
        public event Action<Scene> Disposed;

        /// <summary>
        /// An event raised when an Entity is added to this Scene, called after the Entity is initialized.
        /// </summary>
        public event Action<Entity> ObjectAdded;

        /// <summary>
        /// An event raised when an Entity is removed from this Scene, called before the Entity is disposed.
        /// </summary>
        public event Action<Entity> ObjectRemoved;

        private LevelData _Data;
        private List<SceneSystem> _Systems = new List<SceneSystem>();
        private bool _IsDisposed;
        private bool _IsInitialized;
        protected LinkedList<Entity> _Entities = new LinkedList<Entity>();

        /// <summary>
        /// Gets the name of this Scene.
        /// </summary>
        public string Name { get { return _Data.Name; } }

        /// <summary>
        /// Indicates if this Scene has been disposed of.
        /// Unlike Entities and Components, a Scene may only be disposed once.
        /// </summary>
        public bool IsDisposed { get { return _IsDisposed; } }

        /// <summary>
        /// Indicates if this Scene has been initialized.
        /// Unlike Entities and Components, a Scene may only be initialized once.
        /// </summary>
        public bool IsInitialized { get { return _IsInitialized; } }

        /// <summary>
        /// Gets the size of each tile within this map, in world space coordinates.
        /// </summary>
        public Vector2 TileSize { get { return _Data.TileSize; } }

        /// <summary>
        /// Gets the size of this map, in world space coordinates.
        /// </summary>
        public Vector2 MapSize { get { return _Data.MapSize; } }

        /// <summary>
        /// Returns the number of tiles present in the map.
        /// </summary>
        public Vector2 TilesInMap { get { return _Data.MapSize / _Data.TileSize; } }
        
        /// <summary>
        /// Gets the entities.
        /// </summary>
        public IEnumerable<Entity> Entities { get { return _Entities; } }
        
		/// <summary>
		/// Gets the layers present in this Scene.
		/// </summary>
        public IEnumerable<Layer> Layers { get { return _Data.Layers; } }

        /// <summary>
        /// Gets the map rectangle size.
        /// </summary>
        public Rectangle MapRect { get { return new Rectangle(0, 0, (int)MapSize.X, (int)MapSize.Y); } }

        /// <summary>
        /// Gets the SceneSystems.
        /// </summary>
        public IEnumerable<SceneSystem> Systems { get { return this._Systems; } }
        
        /// <summary>
        /// Adds the given Entity to this Scene.
        /// </summary>
        public void AddEntity(Entity entity)
        {
            if (entity.Scene != null)
                throw new ArgumentException("This Entity is already part of a different scene.");
            this._Entities.AddLast(entity);
            if (IsInitialized)
                entity.Initialize(this);
            if (this.ObjectAdded != null)
                this.ObjectAdded(entity);
            entity.Disposed += entity_Disposed;
        }

        /// <summary>
        /// Adds the specified System to be part of this Scene.
        /// </summary>
        public void AddSystem(SceneSystem system)
        {
            if (IsInitialized)
                system.Initialize(this);
            system.Disposed += system_Disposed;
            this._Systems.Add(system);
        }

        /// <summary>
        /// Removes the specified System from this Scene.
        /// </summary>
        public void RemoveSystem(SceneSystem system)
        {
            _Systems.Remove(system);
            system.Dispose();
        }

        /// <summary>
        /// Returns the first System that matches or is derived from the specified type, or null if none match.
        /// </summary>
        public T GetSystem<T>() where T : SceneSystem
        {
            var type = typeof(T);
            foreach (var system in _Systems)
                if (system.GetType() == type || system.GetType().IsSubclassOf(type))
                    return (T)system;
            return null;
        }

        /// <summary>
        /// Gets a list of entities based on name. 
        /// Returns an empty list if no entity is found. 
        /// Set 'contains' if you want to find all entities that contain the 'name'.
        /// </summary>
        public IEnumerable<Entity> GetEntity(string name, bool contains)
        {
            return this._Entities.Where(e => contains ? e.Name.Contains(name) : e.Name.Equals(name));
        }

        /// <summary>
        /// Returns the smallest entity which contains the given point, or null if none were found to be located there.
        /// </summary>
        public Entity GetEntityAtPosition(Point position)
        {
            return Entities.Where(c => c.Location.Contains(position)).OrderBy(c => c.Location.Width * c.Location.Height).FirstOrDefault();
        }

        public Scene(GameState state, LevelData data)
            : base(state) 
        {
            this._Data = data;
            foreach (var entity in this._Data.DynamicObjects)
                AddEntity(entity);
            AddSystem(new PhysicsSystem());
            AddSystem(new ParticleSystem());
        }

        public virtual void Initialize()
        {
            if(_IsInitialized)
                throw new InvalidOperationException("Unable to initialize a Scene multiple times.");
            _IsInitialized = true;
            foreach (var entity in _Entities)
            {
                try
                {
                    entity.Initialize(this);
                }
                catch (MissingDependencyException e1)
                {
                    throw new Exception("Scene: " + this.Name + "\nEntity: " + entity.Name + "\n\n\t" + e1.Message);
                    //System.Windows.Forms.MessageBox.Show("Critical Error: \n\nScene: " + this.Name + "\nEntity: " + entity.Name + "\n\n\t" + e1.Message + "\n\n The Game will close now.");
                    //Environment.Exit(-1);
                }
            }
            foreach (var system in _Systems)
                system.Initialize(this);
           
        }

        /// <summary>
        /// Disposes of this Scene, removing all remaining Entities and Systems.
        /// Unlike SceneObjects, a Scene may not be initialized after being disposed of.
        /// </summary>
        public virtual void Dispose()
        {
            if (_IsDisposed)
                throw new InvalidOperationException("Unable to dispose a Scene multiple times.");
            _IsDisposed = true;
            for (var node = _Entities.First; node != null; node = node.Next)
                node.Value.Dispose();
            if (this.Disposed != null)
                this.Disposed(this);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            //A Hack to fix that bug where things go through the ground.
            if (gameTime.ElapsedGameTime.TotalMilliseconds > 40)
                return;
            for (var node = _Entities.First; node != null; node = node.Next)
                if (!node.Value.IsDisposed)
                    node.Value.Update(gameTime);
            foreach (var system in _Systems)
                if (!system.IsDisposed)
                    system.Update(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            var SpriteBatch = ScrollerBase.Instance.SpriteBatch;
            var GraphicsDevice = ScrollerBase.Instance.GraphicsDevice;

            //TODO: Make it in a way to manage shaders, if needed.
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();

            var startTile = CameraManager.Active.Position / TileSize;
            var endTile = (CameraManager.Active.Position + CameraManager.Active.ViewportSize) / TileSize;

            int startX = Math.Max((int)startTile.X, 0);
            int startY = Math.Max((int)startTile.Y, 0);
            int endX = Math.Min((int)(endTile.X + 0.5f), (int)TilesInMap.X - 1);
            int endY = Math.Min((int)(endTile.Y + 0.5f), (int)TilesInMap.Y - 1);

            foreach (var layer in Layers)
            {
                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        Tile tile = layer.GetTile(x, y);
                        if (tile == null)
                            continue;
                        var screenCoords = CameraManager.Active.WorldToScreen(new Vector2(tile.Location.X, tile.Location.Y));
                        SpriteBatch.Draw(tile.Texture, new Rectangle((int)screenCoords.X, (int)screenCoords.Y, tile.Location.Width, tile.Location.Height), tile.SourceRect, Color.White);

                    }
                }

                if (layer.IsSolid && layer.TopCollisionOnly)
                {
                    foreach (var e in this._Entities)
                        e.Draw();
                }
            }

            foreach (var system in this._Systems)
                system.Draw();

            SpriteBatch.End();
        }
        
        void system_Disposed(SceneObject obj)
        {
            _Systems.Remove((SceneSystem)obj);
        }
        
        void entity_Disposed(SceneObject obj)
        {
            Entity entity = (Entity)obj;
            entity.Disposed -= entity_Disposed;
            if (this.ObjectRemoved != null)
                this.ObjectRemoved(entity);
            _Entities.Remove(entity);
        }
    }
}
