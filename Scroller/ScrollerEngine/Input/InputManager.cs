using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ScrollerEngine.Input
{
    /// <summary>
    /// Worked on by Peter, Richard, Emmanuel
    /// Provides a global component that handles for input associated with a given player and either controller or keyboard.
    /// </summary>
    public class InputManager : GameComponent
    {
        private Player _Player;
        private KeyboardState PreviousKS;
        private GamePadState PreviousGS;
        private List<Bind> _Binds = new List<Bind>();

        /// <summary>
        /// Gets the player that this InputManager handles input for.
        /// </summary>
        public Player Player { get { return _Player; } }
        
        /// <summary>
        /// Gets all of the active binds for this InputManager.
        /// </summary>
        public IEnumerable<Bind> Binds { get { return _Binds; } }

        /// <summary>
        /// Assigns a bind with the specified action, whether it is multi-invoked, and which buttons it is mapped to.
        /// </summary>
        public void Assign(Action<BindState> action, bool multiInvoke, params InputButton[] buttons) 
        {
            Bind bind = new Bind(this, action, multiInvoke, buttons);
            this._Binds.Add(bind);
        }
        
        /// <summary>
        /// Creates a new InputManager for the given Player.
        /// </summary>
        public InputManager(Player player) 
            : base(ScrollerBase.Instance.Game)
        {
            this._Player = player;
            this.PreviousGS = GamePad.GetState(PlayerIndex.One);
            this.PreviousKS = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            //TODO: Might need to do checks for multiplayer. Ex: when a new player is about to jump in, we don't want
            //      the other binds to activate, just check that a button was pressed and add that player.

            //Dont allow button presses while transitioning. 
            if (ScrollerBase.Instance.GameStateManager.IsTransitioning)
                return;
            var pIndex = ((this.Player == null) ? 0 : this.Player.Index - 1);
            if (pIndex < 0)
                return;
            var playerIndex = PlayerIndex.One + pIndex;
            KeyboardState KS = Keyboard.GetState();
            GamePadState GS = GamePad.GetState(playerIndex);
            foreach (var Bind in this._Binds)
            {
                foreach (var Button in Bind.Buttons)
                {
                    bool pressedNow = IsButtonPressed(Button, KS, GS);
                    bool pressedBefore = IsButtonPressed(Button, PreviousKS, PreviousGS);
                    if (pressedNow && (!pressedBefore || Bind.MultiInvoke))
                        Bind.Invoke(BindState.Pressed);
                    else if (!pressedNow && pressedBefore)
                        Bind.Invoke(BindState.Released);
                }
            }

            PreviousKS = KS;
            PreviousGS = GS;

            base.Update(gameTime);
        }

        private bool IsButtonPressed(InputButton Button, KeyboardState KS, GamePadState GS)
        {
            switch (Button.Type)
            {
                case InputMethod.Key:
                    return KS.IsKeyDown(Button.Key);
                case InputMethod.Button:
                    return GS.IsButtonDown(Button.Button);
                default:
                    throw new ArgumentOutOfRangeException("Button.Type");
            }
        }
        
        private void RegisterBind(Bind bind)
        {
            this._Binds.Add(bind);
        }

        private void RemoveBind(Bind bind)
        {
            bool result = this._Binds.Remove(bind);
            if (!result)
                throw new KeyNotFoundException();
        }
    }
}
