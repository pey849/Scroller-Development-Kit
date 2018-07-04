using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scroller.GameStates;
using ScrollerEngine;
using ScrollerEngine.Components;
using ScrollerEngine.Input;
using ScrollerEngine.Engine;
using Microsoft.Xna.Framework.Input;

namespace Scroller
{
    /// <summary>
    /// Provides binds that are specific to Scroller.
    /// Map buttons here.
    /// </summary>
    public class ScrollerBinds
    {
        private Player _Player;
        private GameStateManager _StateManager = ScrollerGame.Instance.GameStateManager;
        private PlayerControlComponent PCC { get { return _Player.Character.GetComponent<PlayerControlComponent>(); } }

        /// <summary>
        /// Creates and returns the binds for the specified player.
        /// </summary>
        public static void CreateBinds(ScrollerPlayer player)
        {
            var binds = new ScrollerBinds(player);
        }

        private ScrollerBinds(ScrollerPlayer player)
        {
            this._Player = player;
            //TODO: will need to create code to manage multiple players.
            switch (_Player.Index)
            {
                case 1:
                    //Add 'controls' here.
                    _Player.InputManager.Assign(c => MovePlayer(c, Direction.Left), true, new InputButton(Keys.Left));
                    _Player.InputManager.Assign(c => MovePlayer(c, Direction.Right), true, new InputButton(Keys.Right));
                    _Player.InputManager.Assign(c => JumpPlayer(c, false), false, new InputButton(Keys.Up));
                    //_Player.InputManager.Assign(c => CrouchPlayer(c), true, new InputButton(Keys.Down));
                    _Player.InputManager.Assign(c => Pause(c), false, new InputButton(Keys.RightShift));
                    //_Player.InputManager.Assign(c => ShootPlayer(c), false, new InputButton(Keys.Space));
                    break;
                case 2:
                    _Player.InputManager.Assign(c => MovePlayer(c, Direction.Left), true, new InputButton(Keys.A));
                    _Player.InputManager.Assign(c => MovePlayer(c, Direction.Right), true, new InputButton(Keys.D));
                    _Player.InputManager.Assign(c => JumpPlayer(c, false), false, new InputButton(Keys.W));
                    //_Player.InputManager.Assign(c => CrouchPlayer(c), true, new InputButton(Keys.S));
                    _Player.InputManager.Assign(c => Pause(c), false, new InputButton(Keys.E));
                    //_Player.InputManager.Assign(c => ShootPlayer(c), false, new InputButton(Keys.C));
                    break;
            }
        }

        private void MovePlayer(BindState state, Direction dir)
        {
            if (state == BindState.Pressed && _StateManager.IsActiveState<SceneManager>())
                PCC.BeginMove(dir);
            else if(_StateManager.ContainsState<SceneManager>())
                PCC.StopMove();
        }

        private void JumpPlayer(BindState state, bool allowMulti)
        {
            if (!_StateManager.IsActiveState<SceneManager>())
                return;
            if (state == BindState.Pressed)
                PCC.Jump(allowMulti);
        }

        private void CrouchPlayer(BindState state)
        {
            if (state == BindState.Pressed && _StateManager.IsActiveState<SceneManager>())
                PCC.Crouch();
            else if(_StateManager.ContainsState<SceneManager>())
                PCC.StopMove();
        }
        
        private void Pause(BindState state)
        {
            if (state == BindState.Pressed)
            {
                if(_StateManager.IsActiveState<SceneManager>())
                    _StateManager.PushState<PauseState>(true);
                else if (_StateManager.IsActiveState<PauseState>())
                    _StateManager.PopState(true);
            }
        }

        private void ShootPlayer(BindState state)
        {
            if (state == BindState.Pressed)
                PCC.Shoot("Sprites/Misc/arrow");
        }

        private void GiveHealth(BindState state)
        {
            if (state == BindState.Pressed)
                PCC.IncreaseHealth();
        }

        private void TakeHealth(BindState state)
        {
            if (state == BindState.Pressed)
                PCC.DecreaseHealth();
        }

    }
}
