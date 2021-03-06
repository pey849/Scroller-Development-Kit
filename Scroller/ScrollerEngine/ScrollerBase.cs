﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ScrollerEngine.Components;
using ScrollerEngine.Engine;
using ScrollerEngine.Graphics;
using ScrollerEngine.Input;

namespace ScrollerEngine
{
    /// <summary>
    /// Worked on by Peter, Richard, Jonathan
    /// Provides the main engine to run the game.
    /// The engine class does not derive from Game, meaning when using ScrollerEngine the Game class is not used directly.
    /// Instead, ScrollerEngine wraps Game class, forwarding methods to it as needed.
    /// </summary>
    public abstract class ScrollerBase : IDisposable
    {
        /// <summary>
        /// Gets an event called when a new Player is added to the game.
        /// </summary>
        public event Action<Player> PlayerAdded;

        /// <summary>
        /// Gets an event called when a Player is removed from the game.
        /// </summary>
        public event Action<Player> PlayerRemoved;

        private static ScrollerBase _Instance;
        private List<Player> _Players;
        private ScrollerInternalGame _Game;
        private GraphicsDeviceManager _GraphicsManager;
        private SpriteBatch _SpriteBatch;
        private GameStateManager _GameStateManager;
        private InputManager _SystemInputManager;
        private CameraManager _CameraManager;
        private float _TimeSinceFPSUpdate = 0;
        private int _CurrentFrames;
        private int _FPS;
        private TimeSpan _MaxStepDuration = TimeSpan.FromMilliseconds(25f);
        private TimeSpan _FrameSkipDuration = TimeSpan.FromMilliseconds(1000);

        /// <summary>
        /// Returns the singleton instance of ScrollerBase.
        /// </summary>
        public static ScrollerBase Instance { get { return _Instance; } }

        /// <summary>
        /// Gets the players that are currently participating in the game.
        /// </summary>
        public IEnumerable<Player> Players { get { return _Players; } }

        /// <summary>
        /// Gets the GraphicsDeviceManager being used for the game.
        /// </summary>
        public GraphicsDeviceManager GraphicsManager { get { return _GraphicsManager; } }

        /// <summary>
        /// Gets the GraphicsDevice being used for the game.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get { return _Game.GraphicsDevice; } }

        /// <summary>
        /// Gets the Game.
        /// </summary>
        public Game Game { get { return _Game; } }

        /// <summary>
        /// Gets the global SpriteBatch that may be used for rendering.
        /// </summary>
        public SpriteBatch SpriteBatch { get { return _SpriteBatch; } }

        /// <summary>
        /// Gets the ContentManager used for loading assets.
        /// </summary>
        public ContentManager GlobalContent { get { return _Game.Content; } }

        /// <summary>
        /// Gets the GameStateManager used by the engine.
        /// </summary>
        public GameStateManager GameStateManager { get { return _GameStateManager; } }

        /// <summary>
        /// Gets the InputManager used by the overall game.
        /// </summary>
        public InputManager SystemInputManager { get { return _SystemInputManager; } }

        /// <summary>
        /// Gets the camera manager.
        /// </summary>
        public CameraManager CameraManager { get { return _CameraManager; } }

        /// <summary>
        /// Gets the number of updates calls that occurred in the last second.
        /// </summary>
        public int FPS { get { return _FPS; } }

        /// <summary>
        /// Gets or sets the maximum amount of time that should pass in a single update.
        /// If a greater amount than this value, but less than FrameSkipDuration occurs since the last frame, it is split into multiple steps.
        /// This property only applies for Update. Draw is still called only once.
        /// </summary>
        public TimeSpan MaxStepDuration
        {
            get { return _MaxStepDuration; }
            set { _MaxStepDuration = value; }
        }

        /// <summary>
        /// Gets or sets the maximum amount of time that a frame should simulate without being discarded.
        /// If greater than this value passes since the last frame, this frame call is discarded (both Update and Draw).
        /// </summary>
        public TimeSpan FrameSkipDuration
        {
            get { return _FrameSkipDuration; }
            set { _FrameSkipDuration = value; }
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Run()
        {
            this._Game.Run();
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        public void Exit()
        {
            this._Game.Exit();
        }

        /// <summary>
        /// Registers the given GameComponent as a GlobalComponent.
        /// </summary>
        public void RegisterGlobalComponent(IGameComponent component)
        {
            component.Initialize();
            _Game.Components.Add(component);
        }

        /// <summary>
        /// Unregisters the given GameComponent, causing it to no longer be active.
        /// </summary>
        public void RemoveGlobalComponent(IGameComponent component)
        {
            bool result = _Game.Components.Remove(component);
            if (!result)
                throw new ArgumentException("The given component was not registered.");
        }

        /// <summary>
        /// Adds a GameState. 
        /// Throws an exception if the same type is added.
        /// </summary>
        protected void AddGameState(GameState state)
        {
            if (this._GameStateManager.StateCollection.Contains(state) || this._GameStateManager.StateCollection.Contains(state.GetType()))
                throw new Exception("A GameState of type '" + state.GetType().ToString() + "' already exists.");
            this._GameStateManager.StateCollection.Add(state);
        }

        /// <summary>
        /// Gets the GameState specified by type. 
        /// Throws an exception if nothing is found.
        /// </summary>
        public T GetGameState<T>() where T : GameState
        {
            var gameState = this._GameStateManager.StateCollection[typeof(T)];
            if (gameState == null)
                throw new Exception("GameStateCollection does not contain GameState of type: " + typeof(T).ToString());
            return (T)gameState;
        }

        /// <summary>
        /// Adds the specified new player to the game.
        /// </summary>
        public void AddPlayer(Player Player)
        {
            if (Players.Contains(Player))
                throw new ArgumentException("The given Player was already part of the game.");
            this._Players.Add(Player);
            this.RegisterGlobalComponent(Player.InputManager);
            if (this.PlayerAdded != null)
                this.PlayerAdded(Player);
        }

        /// <summary>
        /// Removes the given Player from the game.
        /// </summary>
        public void RemovePlayer(Player Player)
        {
            bool removed = _Players.Remove(Player);
            if (!removed)
                throw new ArgumentException("The given Player was not registered with the game engine.");
            this.RemoveGlobalComponent(Player.InputManager);
            if (this.PlayerRemoved != null)
                this.PlayerRemoved(Player);
        }

        /// <summary>
        /// Creates a new instance of the ScrollerBase.
        /// Only one instance of ScrollerBase may exist at any time.
        /// </summary>
        public ScrollerBase()
        {
            if (Interlocked.Exchange(ref _Instance, this) != null)
                throw new InvalidOperationException("Only one instance of ScrollerBase may exist at any time");
            this._Players = new List<Player>();
            this._Game = new ScrollerInternalGame(this);
        }

        /// <summary>
        /// Disposes the engine, exiting the game.
        /// </summary>
        void IDisposable.Dispose()
        {
            _Game.Dispose();
        }

        private void OnInitialize()
        {
            //TODO: Create a thing to read from a .ini file.
            this._GraphicsManager.SynchronizeWithVerticalRetrace = false;
            this._GraphicsManager.PreferredBackBufferWidth = 800;
            this._GraphicsManager.PreferredBackBufferHeight = 600;
            this._GraphicsManager.IsFullScreen = false;
            this._GraphicsManager.ApplyChanges();

            this.Game.InactiveSleepTime = TimeSpan.FromMilliseconds(1);
            this.Game.IsMouseVisible = false;
            this._SpriteBatch = new SpriteBatch(GraphicsDevice);
            this._GameStateManager = new GameStateManager(_Game);
            this.RegisterGlobalComponent(this._GameStateManager);
            this._SystemInputManager = new InputManager(null);
            this.RegisterGlobalComponent(this._SystemInputManager);
            this.CreateSystemBinds();
            this._CameraManager = new Graphics.CameraManager();
            this.RegisterGlobalComponent(this._CameraManager);

            ScrollerSerializer.ReloadEntities();

            this.Initialize();

#if DEBUG
            this.Game.IsMouseVisible = true;
            this.RegisterGlobalComponent(new Debugger(this.Game, "Fonts/DebugFont", "Debug/Outline"));
#endif
        }

        /// <summary>
        /// Initializes system binds.
        /// </summary>
        protected virtual void CreateSystemBinds()
        { 
        
        }

        /// <summary>
        /// Initializes any logic required for the game.
        /// </summary>
        protected abstract void Initialize();

        private class ScrollerInternalGame : Game
        {
            private ScrollerBase _Engine;

            public ScrollerInternalGame(ScrollerBase engine)
            {
                this._Engine = engine;
                Content.RootDirectory = "Content";
                var graphicsManager = new GraphicsDeviceManager(this);
                ScrollerBase.Instance._GraphicsManager = graphicsManager;
                this.IsFixedTimeStep = false;
                this.TargetElapsedTime = TimeSpan.FromMilliseconds(1f);
                
            }

            protected override void Initialize()
            {
                base.Initialize();
                _Engine.OnInitialize();
            }

            protected override void Update(GameTime gameTime)
            {
                if (gameTime.ElapsedGameTime >= _Engine.FrameSkipDuration)
                {
                    Thread.Sleep(5);
                    return;
                }
                TimeSpan remaining = gameTime.ElapsedGameTime;
                while (remaining > TimeSpan.Zero)
                {
                    TimeSpan delta = TimeSpan.FromTicks(Math.Min(_Engine.MaxStepDuration.Ticks, remaining.Ticks));
                    GameTime stepTime = new GameTime(gameTime.TotalGameTime.Subtract(remaining), delta);
                    remaining = remaining.Subtract(delta);
                    base.Update(gameTime);
                }
            }

            protected override void Draw(GameTime gameTime)
            {
                if (gameTime.ElapsedGameTime >= _Engine.FrameSkipDuration)
                    return;
                var instance = ScrollerBase.Instance;
                instance._TimeSinceFPSUpdate += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                instance._CurrentFrames++;
                if (instance._TimeSinceFPSUpdate > 1000)
                {
                    instance._FPS = Instance._CurrentFrames;
                    instance._TimeSinceFPSUpdate -= 1000;
                    instance._CurrentFrames = 0;
                }

                base.Draw(gameTime);
//#if DEBUG
//                //Shows Fps
//                var font = ScrollerBase.Instance.GlobalContent.Load<SpriteFont>("Fonts/DebugFont");
//                var SpriteBatch = ScrollerBase.Instance.SpriteBatch;
//                var currentVel = instance._Players[0].Character.GetComponent<PhysicsComponent>().Velocity;
//                SpriteBatch.Begin();
//                var height = this._Engine.GraphicsManager.PreferredBackBufferHeight - 40;
//                SpriteBatch.DrawString(font, "FPS: " + this._Engine.FPS.ToString(), new Vector2(5,height), Color.White);
//                SpriteBatch.End();    
//#endif
            }

        }

    }
}
