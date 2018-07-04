using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScrollerEngine.Engine
{
    /// <summary>
    /// Worked on by Mary, Richard, Emmanuel
    /// Provides a single system being used for a specific GameStateComponent.
    /// This class acts as a replacement to GameComponents, with ScrollerEngine features built in.
    /// </summary>
    public abstract class GameStateComponent : IGameComponent, IDrawable, IUpdateable
    {
        private GameState _GameState;
        private bool _Visible = true;
        private bool _Enabled = true;
        private int _DrawOrder;
        private int _UpdateOrder;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Gets the GameState that this component belongs to.
        /// </summary>
        public GameState GameState { get { return _GameState; } }

        /// <summary>
        /// Gets or sets the order in which this component should be drawn, relative to other components for this GameState.
        /// A higher order is drawn later.
        /// </summary>
        public int DrawOrder
        {
            get { return _DrawOrder; }
            set 
            {
                if (_DrawOrder == value)
                    return;
                _DrawOrder = value;
                if (this.DrawOrderChanged != null)
                    this.DrawOrderChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets or sets the order in which this component should be updated, relative to the other components for this GameState.
        /// A higher order is updated later.
        /// </summary>
        public int UpdateOrder
        {
            get { return _UpdateOrder; }
            set
            {
                if (_UpdateOrder == value)
                    return;
                _UpdateOrder = value;
                if (this.UpdateOrderChanged != null)
                    this.UpdateOrderChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets or sets whether this component should be displayed, assuming this GameState is visible.
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                if (value == _Visible)
                    return;
                _Visible = value;
                if (this.VisibleChanged != null)
                    this.VisibleChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets or sets whether this component should be updated, assuming this GameState is not paused.
        /// </summary>
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                if (value == _Enabled)
                    return;
                _Enabled = value;
                if (this.EnabledChanged != null)
                    this.EnabledChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Creates a new GameStateComponent for the given State, but does not add it to the State's component list.
        /// </summary>
        public GameStateComponent(GameState state)
        {
            this._GameState = state;
        }

        /// <summary>
        /// Explicitly defines the IGameComponent Initialize.
        /// This method should not be used by user code, and as such is sealed off.
        /// Instead, handle initialization in the constructor.
        /// </summary>
        void IGameComponent.Initialize() { }

        /// <summary>
        /// Dispatches a call to Draw to the implementation of GameStateComponent.
        /// Used for interacting with the Game class. This method should not be used by user code.
        /// </summary>
        void IDrawable.Draw(GameTime gameTime)
        {
            this.OnDraw(gameTime);
        }

        /// <summary>
        /// Dispatches a call to Update to the implementation of GameStateComponent.
        /// Used for interacting with the Game class. This method should not be used by user code.
        /// </summary>
        void IUpdateable.Update(GameTime gameTime)
        {
            this.OnUpdate(gameTime);
        }

        /// <summary>
        /// Called when this component should be redrawn.
        /// </summary>
        protected abstract void OnDraw(GameTime gameTime);

        /// <summary>
        /// Called when this component should be updated.
        /// </summary>
        protected abstract void OnUpdate(GameTime gameTime); 
    }
}
