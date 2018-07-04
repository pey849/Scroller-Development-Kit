using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrollerEngine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Mary, Richard, Emmanuel
    /// Provides the base class for an object that is part of a Scene, such as a System, Component, or Entity.
    /// </summary>
    public abstract class SceneObject
    {
		/// <summary>
		/// Gets an event called every time this object is disposed of.
		/// </summary>
		public event Action<SceneObject> Disposed;

		/// <summary>
		/// Gets an event called every time this object is initialized.
		/// </summary>
		public event Action<SceneObject> Initialized;

        private string _Name;
        private bool _IsInitialized;
        private bool _IsDisposed;
        private Scene _Scene;
        private static int NextObjectID = 0;

        /// <summary>
        /// Indicates if this object has been disposed of.
        /// </summary>
        [ContentSerializerIgnore]
        public bool IsDisposed { get { return _IsDisposed; } }

        /// <summary>
        /// Indicates if this object has already been initialized.
        /// </summary>
        [ContentSerializerIgnore]
        public bool IsInitialized { get { return _IsInitialized; } }

        /// <summary>
        /// Gets the Scene that this SceneObject is part of, or null if the SceneObject is not currently attached to a Scene.
        /// </summary>
        [ContentSerializerIgnore]
        public Scene Scene { get { return _Scene; } }

        /// <summary>
        /// Gets or sets the name of this object. 
        /// Changing the name of an object after it's initialized is not allowed.
        /// </summary>
        [ContentSerializerIgnore]
        public virtual string Name
        {
            get { return _Name; }
            set
            {
                if (IsInitialized)
                    throw new InvalidOperationException("Unable to change the name of an object after it's initialized.");
                _Name = value;
            }
        }

        /// <summary>
        /// Initializes this SceneObject to the specified Scene.
        /// This is called when the SceneObject is added to the Scene, or if the SceneObject's parent is already initialized, is called immediately.
        /// </summary>
        public void Initialize(Scene scene)
        {
            if (_IsInitialized)
                throw new InvalidOperationException("Unable to initialize a SceneObject that has already been initialized.");
            this._IsInitialized = true;
            this._IsDisposed = false;
            this._Scene = scene;
            OnInitialize(); 
            if (this.Initialized != null)
                this.Initialized(this);
        }

        /// <summary>
        /// Advances this object by the specified period of time.
        /// This is called only once per frame.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (_IsDisposed || !_IsInitialized)
                throw new InvalidOperationException("Unable to update an object that is either disposed or not initialized.");
            OnUpdate(gameTime);
        }

        /// <summary>
        /// Renders this object.
        /// Note that this may be called multiple times per frame.
        /// </summary>
        public void Draw()
        {
            if (_IsDisposed || !_IsInitialized)
                throw new InvalidOperationException("Unable to update an object that is either disposed or not initialized.");
            OnDraw();
        }

        /// <summary>
        /// Disposes of this object, removing it from the Scene if it has been initialized.
        /// It is possible for a SceneObject to be reinitialized after being Disposed if added to a different Scene.
        /// In this case, Initialize will be called again with the new Scene.
        /// </summary>
        public void Dispose()
        {
            if (_IsDisposed)
                throw new ObjectDisposedException("Unable to dispose of an already disposed object without a call to Initialize between the Disposes..", (Exception)null);
            this._IsDisposed = true;
            this._IsInitialized = false;
            OnDispose();
            if (this.Disposed != null)
                this.Disposed(this);
            this._Scene = null;
        }

        public SceneObject()
        {
            int CurrID = System.Threading.Interlocked.Increment(ref NextObjectID);
            this._Name = Name ?? (this.GetType().Name + CurrID);
        }

        /// <summary>
        /// Called when this object is initialized, either for the first time or after a call to Dispose.
        /// </summary>
        protected virtual void OnInitialize() { }
        
        /// <summary>
        /// Called when this object needs to be updated by the specified amount of time.
        /// </summary>
        protected virtual void OnUpdate(GameTime gameTime) { }

        /// <summary>
        /// Called when this object needs to be rendered.
        /// </summary>
        protected virtual void OnDraw() { }

        /// <summary>
        /// Called when this object is disposed.
        /// An object may be disposed of without currently being initialized, and may be disposed of again each time it's reinitialized.
        /// </summary>
        protected virtual void OnDispose() { }

        public override string ToString()
        {
            return this.Name + " (" + this.GetType().Name + ")";
        }
    }
}
