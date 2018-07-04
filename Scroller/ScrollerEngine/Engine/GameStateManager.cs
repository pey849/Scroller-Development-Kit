using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrollerEngine;

namespace ScrollerEngine.Engine
{
    /// <summary>
    /// Worked on by Mary, Richard, Emmanuel, Peter
    /// A delegate the is used for transition events.
    /// </summary>
    public delegate void TransitionDelegate();

    /// <summary>
    /// A delegate which occurs when a game state is popped or pushed.
    /// </summary>
    public delegate void GameStateChangeDelegate(GameState oldState, GameState newState);

    /// <summary>
    /// Provides a handler used to manage GameStates.
    /// </summary>
    public class GameStateManager : DrawableGameComponent
    {
        /// <summary>
        /// Triggered when a game state change occurs.
        /// </summary>
        public GameStateChangeDelegate OnGameStateChange;
        /// <summary>
        /// Triggered when the transition is about to start from the middle.
        /// </summary>
        public TransitionDelegate OnMidTransition;

        private GameStateCollection _StateCollection = new GameStateCollection();
        private LinkedList<GameState> _ActiveStates = new LinkedList<GameState>();

        private RenderTarget2D _TransitionMask;
        private float _TransitionValue = 0f;
        private TransitionMode _TransitionMode;
        private TransitionState _TransitionState;
        private float _TransitionModifier = 2f;
        private LinkedListNode<GameState> _PoppedState;

        /// <summary>
        /// Gets or sets whether the GameStateManager is transitioning between states.
        /// </summary>
        public bool IsTransitioning { get; private set; }

        /// <summary>
        /// Gets the GameStateCollection.
        /// </summary>
        public GameStateCollection StateCollection { get { return _StateCollection; } }

        /// <summary>
        /// Gets the current state.
        /// </summary>
        public GameState GetCurrentState()
        {
            return this._ActiveStates.Last();
        }

        /// <summary>
        /// Gets whether a specific state based on the type is the active one.
        /// </summary>
        public bool IsActiveState<T>() where T : GameState
        {
            var current = GetCurrentState();
            return current is T;
        }

        /// <summary>
        /// Gets whether a state of type T exists in the stack.
        /// </summary>
        public bool ContainsState<T>() where T : GameState
        {
            foreach (var state in this._ActiveStates)
            {
                if (state is T)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Pushes the specified GameState to the top of the stack.
        /// </summary>
        public void PushState<T>(bool ignoreTransition, float transitionModifier = 2) where T : GameState
        {
            var state = this._StateCollection[typeof(T)];
            if (state == null)
                throw new ArgumentException("The specified GameState was not found.");
            _TransitionModifier = transitionModifier;
            if (!ignoreTransition)
            {
                if (this._ActiveStates.Count == 0)
                    BeginTransition(TransitionState.MidTransition, TransitionMode.Pushed);
                else
                    BeginTransition(TransitionState.StartTransition, TransitionMode.Pushed);
            }
            if (OnGameStateChange != null)
                OnGameStateChange(this._ActiveStates.Last == null ? null : this._ActiveStates.Last.Value, state);
            this._ActiveStates.AddLast(state);
        }

        /// <summary>
        /// Pushes all the states specified, showing the last one in that list.
        /// </summary>
        public void PushTo(Type[] states, bool ignoreTransition, float transitionModifier = 2)
        {
            _TransitionModifier = transitionModifier;
            if (!ignoreTransition)
            {
                if (this._ActiveStates.Count == 0)
                    BeginTransition(TransitionState.MidTransition, TransitionMode.Pushed);
                else
                    BeginTransition(TransitionState.StartTransition, TransitionMode.Pushed);
            }
            if (OnGameStateChange != null)
                OnGameStateChange(this._ActiveStates.Last == null ? null : this._ActiveStates.Last.Value, _StateCollection[states.Last()]);
            for (int i = 0; i < states.Length; i++)
                this._ActiveStates.AddLast(_StateCollection[states[i]]);
        }

        /// <summary>
        /// Clears off the current GameState, activating the previous state and returning the state that was removed.
        /// </summary>
        public GameState PopState(bool ignoreTransition, float transitionModifier = 2)
        {
            if (this._ActiveStates.Count == 1)
                throw new InvalidOperationException("Cannot pop the last state in the stack.");
            _TransitionModifier = transitionModifier;
            var Last = this._ActiveStates.Last;
            this._ActiveStates.RemoveLast();
            if (!ignoreTransition)
                BeginTransition(TransitionState.StartTransition, TransitionMode.Popped);
            if (OnGameStateChange != null)
                OnGameStateChange(Last.Value, this._ActiveStates.Last.Value);
            _PoppedState = Last;
            return Last.Value;
        }

        /// <summary>
        /// Pops until the specified GameState type is found.
        /// An exception is thrown if no GameState of that type is found.
        /// Returns the first item popped.
        /// </summary>
        public GameState PopTo<T>(bool ignoreTransition, float transitionModifier = 2) where T : GameState
        {
            if (this._ActiveStates.Count == 1)
                throw new InvalidOperationException("Cannot pop the last state in the stack.");
            _TransitionModifier = transitionModifier;
            LinkedList<GameState> list = new LinkedList<GameState>();
            var Last = this._ActiveStates.Last;
            while (true)
            {
                var Next = this._ActiveStates.Last;
                if (Next.Value is T)
                    break;
                this._ActiveStates.RemoveLast();
                list.AddFirst(Next);
                if (this._ActiveStates.Count == 0)
                    throw new Exception(string.Format("A state of type '{0}' was not found."));
            }
            if (OnGameStateChange != null)
                OnGameStateChange(Last.Value, this._ActiveStates.Last.Value);
            if (!ignoreTransition)
                BeginTransition(TransitionState.StartTransition, TransitionMode.Popped);
            _PoppedState = list.Last;
            return Last.Value;
        }

        /// <summary>
        /// Creates a new GameStateManager. This is done by the engine.
        /// </summary>
        public GameStateManager(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();
            _TransitionMask = new RenderTarget2D(ScrollerBase.Instance.GraphicsDevice, ScrollerBase.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth,
                                                            ScrollerBase.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight);
            GraphicsDevice.SetRenderTarget(_TransitionMask);
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetRenderTarget(null);
        }

        public override void Update(GameTime gameTime)
        {
            //update the transition stuff.
            if (this.IsTransitioning)
            {
                _TransitionValue += gameTime.GetTimeScalar() * _TransitionModifier;
                switch (_TransitionState)
                {
                    case TransitionState.StartTransition:
                        if (_TransitionValue >= 0f)
                        {
                            _TransitionState = TransitionState.MidTransition;
                            if (OnMidTransition != null)
                                OnMidTransition();
                        }
                        break;
                    case TransitionState.MidTransition:
                        if (_TransitionValue >= 1f)
                        {
                            IsTransitioning = false;
                            _TransitionState = TransitionState.NoTransition;
                        }
                        break;
                }
            }

            // While this could be optimized in many ways, it probably doesn't need to be. Same for Draw.
            var UpdateHead = FirstReversed(c => c.BlocksUpdate);
            for (var Node = UpdateHead; Node != null; Node = Node.Next)
            {
                foreach (var Component in Node.Value.Components.OrderBy(c => c.UpdateOrder))
                {
                    if (Component.Enabled)
                        ((IUpdateable)Component).Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Manages most rendering for the engine, drawing any currently active states.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            //RenderReversed(States.Last, gameTime);
            base.Draw(gameTime);

            if (!IsTransitioning)
                RenderReversed(_ActiveStates.Last, gameTime);
            else if (IsTransitioning)
            {
                //Creates the transition effects
                switch (_TransitionMode)
                {
                    case TransitionMode.Pushed:
                        var last = _ActiveStates.Last;
                        if (_ActiveStates.Count > 1 && _TransitionState == TransitionState.StartTransition)
                            RenderComponents(last.Previous.Value, gameTime);
                        else
                            RenderComponents(last.Value, gameTime);
                        break;
                    case TransitionMode.Popped:
                        if (_TransitionState == TransitionState.StartTransition)
                            RenderPoppedReversed(_PoppedState, gameTime);
                        else
                            RenderReversed(_ActiveStates.Last, gameTime);
                        break;
                }
                //Draw the transition mask with the specified alpha value.
                var SpriteBatch = ScrollerBase.Instance.SpriteBatch;
                SpriteBatch.Begin();
                SpriteBatch.Draw(_TransitionMask, Vector2.Zero, Color.White * Math.Abs(Math.Abs(_TransitionValue) - 1));
                SpriteBatch.End();
            }

        }

        private void BeginTransition(TransitionState State, TransitionMode Mode)
        {
            if (IsTransitioning)
                return;
            _TransitionMode = Mode;
            _TransitionState = State;
            switch (State)
            {
                case TransitionState.NoTransition:
                    _TransitionValue = 1f;
                    IsTransitioning = false;
                    break;
                case TransitionState.StartTransition:
                    _TransitionValue = -1f;
                    IsTransitioning = true;
                    break;
                case TransitionState.MidTransition:
                    _TransitionValue = 0f;
                    IsTransitioning = true;
                    break;
            }

        }

        private LinkedListNode<GameState> FirstReversed(Func<GameState, bool> fun, LinkedListNode<GameState> Tail = null)
        {
            if (Tail == null)
                Tail = this._ActiveStates.Last;
            for (var Node = Tail; Node != null; Node = Node.Previous)
                if (fun(Node.Value))
                    return Node;
            return _ActiveStates.First;
        }

        private void RenderReversed(LinkedListNode<GameState> Tail, GameTime Time)
        {
            var RenderHead = FirstReversed(c => c.BlocksDraw);
            for (var Node = RenderHead; Node != null; Node = Node.Next)
                RenderComponents(Node.Value, Time);
        }

        private void RenderPoppedReversed(LinkedListNode<GameState> Tail, GameTime Time)
        {
            var RenderHead = FirstReversed(c => c.BlocksDraw, Tail);
            if (_ActiveStates.Count != 1)
                RenderHead = FirstReversed(c => c.BlocksDraw);
            for (var Node = RenderHead; Node != null; Node = Node.Next)
                RenderComponents(Node.Value, Time);

        }
        //Components as in GameStateComponents
        private void RenderComponents(GameState State, GameTime Time)
        {
            foreach (var Component in State.Components.OrderBy(c => c.DrawOrder))
            {
                if (Component.Visible)
                    ((IDrawable)Component).Draw(Time);
            }
        }

        /// <summary>
        /// Indicates the current transition state or where to start transition.
        /// </summary>
        private enum TransitionState
        {
            /// <summary>
            /// Indicates no transition.
            /// </summary>
            NoTransition,
            /// <summary>
            /// Indicates to start the full transition progress (IE: fade out, fade in.)
            /// </summary>
            StartTransition,
            /// <summary>
            /// Indicates to start at the middle of a transition.
            /// </summary>
            MidTransition
        }

        /// <summary>
        /// Indicates the type of transition.
        /// </summary>
        private enum TransitionMode
        {
            /// <summary>
            /// A new GameState was pushed to the top of the stack.
            /// </summary>
            Pushed,
            /// <summary>
            /// A GameState was popped from the stack.
            /// </summary>
            Popped
        }
    }
}
