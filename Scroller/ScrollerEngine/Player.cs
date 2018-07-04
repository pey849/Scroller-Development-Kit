using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrollerEngine.Input;
using ScrollerEngine.Components;

namespace ScrollerEngine
{
    /// <summary>
    /// Worked on by Peter, Richard
    /// Indicates one of the players in the game. A player may or may not be in the game, and they may or may not be controlling a character at this time.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Called when the character for this Player is changed with the new Entity that it was changed to.
        /// </summary>
        public event Action<Entity> CharacterChanged;

        private Entity _Character;
        private InputManager _InputManager;

        /// <summary>
        /// Gets the InputManager being used for this player.
        /// </summary>
        public InputManager InputManager { get { return _InputManager; } }

        /// <summary>
        /// Gets or sets the character that this player is controlling.
        /// </summary>
        public Entity Character
        {
            get { return _Character; }
            set
            {
                if (_Character == value)
                    return;
                _Character = value;
                if (CharacterChanged != null)
                    CharacterChanged(_Character);
            }
        }

        /// <summary>
        /// Gets the index of this player, from 1 to N where N is the number of players in the game.
        /// If the player is not registered with the game engine, the result is -1.
        /// </summary>
        public int Index
        {
            get
            {
                // TODO: What a hacky implementation.
                // But we want the game itself to create the Player, so they can substitute their own Player class that handles things like what their character is and such.
                // And we don't want the game to manually manage player indexes.
                // NOTE: Haven't actually tested this so potential problems involved with this. Ex: adding and removing Players would change the index. But if we don't remove 
                //      them then it would be fine. They just won't appear in game.
                int Result = ScrollerBase.Instance.Players.ToList().IndexOf(this);
                return Result >= 0 ? Result + 1 : -1;
            }
        }

        /// <summary>
        /// Creates a new Player with an InputManager attached to it.
        /// </summary>
        public Player(Entity Character)
        {
            this._InputManager = new InputManager(this);
            this.Character = Character;
        }
    }
}
