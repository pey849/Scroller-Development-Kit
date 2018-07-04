using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScrollerEngine.Engine
{
    /// <summary>
    /// Worked on by Mary, Richard, Emmanuel
    /// A class to represent a single state, or screen, in the game. For example, the main menu, the game itself, or the paused screen would all be it's own GameState.
    /// GameStates may be pushed on top of each other, and may or may not cover the whole screen. 
    /// </summary>
    public abstract class GameState
    {
        private GameComponentCollection _Components = new GameComponentCollection();

        /// <summary>
        /// Gets whether this GameState should prevent any previous states from updating.
        /// </summary>
        public abstract bool BlocksUpdate { get; }

        /// <summary>
        /// Gets whether this GameState should prevent any previous states from drawing.
        /// </summary>
        public abstract bool BlocksDraw { get; }

        /// <summary>
        /// Gets all of the components present for this GameState.
        /// </summary>
        public IEnumerable<GameStateComponent> Components { 
            get { return _Components.Select(c => (GameStateComponent)c); } 
        }

        /// <summary>
        /// Adds the given component to this GameState.
        /// </summary>
        public void AddComponent(GameStateComponent component)
        {
            if (_Components.Contains(component))
                throw new ArgumentException("Component was already part of this GameState");
            this._Components.Add(component);
        }

        /// <summary>
        /// Removes the given component from this GameState.
        /// </summary>
        public void RemoveComponent(GameStateComponent component)
        {
            bool result = this._Components.Remove(component);
            if (!result)
                throw new ArgumentException("Component was not in this GameState.");
        }

        /// <summary>
        /// Returns the first GameStateComponent in this collection that can be casted to the specified type, null if not found.
        /// </summary>
        public T GetComponent<T>() where T : GameStateComponent
        {
            foreach (var c in _Components)
            {
                T casted = c as T;
                if (casted != null)
                    return casted;
            }
            return null;
        }

    }
}
