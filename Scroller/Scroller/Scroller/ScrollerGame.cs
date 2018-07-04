using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scroller.GameStates;
using ScrollerEngine;
using ScrollerEngine.Components;
using ScrollerEngine.Engine;
using ScrollerEngine.Input;
using ScrollerEngine.Scenes;
using ScrollerEngine.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Scroller
{
    /// <summary>
    /// Provides the main class for Scroller.
    /// </summary>
    public class ScrollerGame : ScrollerBase
    {
        /// <summary>
        /// Returns the instance of ScrollerGame.
        /// </summary>
        public static new ScrollerGame Instance { get { return (ScrollerGame)ScrollerBase.Instance; } }

        /// <summary>
        /// Creates a new player.
        /// </summary>
        public ScrollerPlayer CreateNewPlayer()
        {
            var playerEntity = ScrollerPlayer.LoadPlayerEntity();
            ScrollerPlayer player = new ScrollerPlayer(playerEntity);
            this.AddPlayer(player);
            ScrollerBinds.CreateBinds(player);
            return player;
        }

        /// <summary>
        /// Creates a new instance of ScrollerGame.
        /// </summary>
        public ScrollerGame()
        {
            PlayerAdded += ScrollerGame_PlayerAdded;
            PlayerRemoved += ScrollerGame_PlayerRemoved;
        }

        void ScrollerGame_PlayerAdded(Player obj)
        {

        }

        void ScrollerGame_PlayerRemoved(Player obj)
        {
            if (obj.Character != null)
                obj.Character.Dispose();
        }

        protected override void Initialize()
        {
            //Creates the Players. 
            CreateNewPlayer();
            CreateNewPlayer();

            CameraManager.FollowPlayer(this.Players.First(), this.Players.ElementAt(1));
            //Add initiation logic here.

            SceneManager sceneManager = new SceneManager();
            this.AddGameState(new TitleScreen());
            this.AddGameState(new HowToPlay());
            this.AddGameState(sceneManager);
            this.AddGameState(new PauseState());
            this.AddGameState(new GameOverState());
            this.AddGameState(new LevelClearState());
            GameStateManager.PushState<TitleScreen>(false);

            //TESTING STUFF BELOW
            //GameStateManager.PushTo(new Type[] { typeof(TitleScreen), typeof(HowToPlay), typeof(SceneManager) }, false);
            //sceneManager.RandomScene();// ChangeScene("ConceptLevel", "");//"Set1");//
        }

        protected override void CreateSystemBinds()
        {
            base.CreateSystemBinds();
            this.SystemInputManager.Assign(StateActions, false, new InputButton(Keys.Enter));
            this.SystemInputManager.Assign(c => GameOver(c, true), false, new InputButton(Keys.RightShift));
            this.SystemInputManager.Assign(c => GameOver(c, false), false, new InputButton(Keys.Enter));
#if DEBUG
            this.SystemInputManager.Assign(CloseGame, false, new InputButton(Keys.Escape));
            this.SystemInputManager.Assign(ResetScene, false, new InputButton(Keys.F5));
            this.SystemInputManager.Assign(ToggleViewEntities, false, new InputButton(Keys.F10));
            //this.SystemInputManager.Assign(ToggleMultiplayer, false, new InputButton(Keys.F9));
            this.SystemInputManager.Assign(ResetSceneAtPosition, false, new InputButton(Keys.F6));
            this.SystemInputManager.Assign(ForceInvincibility, false, new InputButton(Keys.F9));
            
#endif
        }

        private void StateActions(BindState state)
        {
            if (state == BindState.Pressed)
            {
                if (this.GameStateManager.IsActiveState<TitleScreen>())
                    this.GameStateManager.PushState<HowToPlay>(false);
                else if (this.GameStateManager.IsActiveState<PauseState>())
                    this.GameStateManager.PopTo<TitleScreen>(false);
                else if (this.GameStateManager.IsActiveState<LevelClearState>())
                {
                    this.GameStateManager.PopState(true);
                    GetGameState<SceneManager>().RandomScene(false);
                }
                else if (this.GameStateManager.IsActiveState<HowToPlay>())
                {
                    //TODO: Load a scene somehow. Probably at random. 
                    this.GameStateManager.PushState<SceneManager>(false);
                    var sceneManager = GetGameState<SceneManager>();
                    //sceneManager.ReloadScenes("", true);
                    sceneManager.ResetScenes();
                    sceneManager.RandomScene(true);
                    //sceneManager.ChangeScene("TestLevel", "", true);

                    //TODO 2013/06/11: Fix reload timing so that player doesn't end up at the bottom
                }
            }
        }

        private void GameOver(BindState state, bool isContinue)
        {
            if (state == BindState.Pressed)
            {
                var isGameover = this.GameStateManager.IsActiveState<GameOverState>();
                if (!isContinue && isGameover)
                    this.GameStateManager.PopTo<TitleScreen>(false);
                else if (isGameover)
                {
                    this.GameStateManager.OnMidTransition += StateManager_OnMidTransition;
                    this.GameStateManager.PopState(false);
                }
            }
        }
        void StateManager_OnMidTransition()
        {
            ScrollerBase.Instance.GetGameState<SceneManager>().ReloadScenes("", true);
            this.GameStateManager.OnMidTransition -= StateManager_OnMidTransition;
        }

        private void ForceInvincibility(BindState state)
        {
            if (state == BindState.Pressed)
            {
                foreach (var player in this.Players)
                    player.Character.GetComponent<HealthComponent>().IsInvincible = !player.Character.GetComponent<HealthComponent>().IsInvincible;
            }
        }

        private void CloseGame(BindState state)
        {
            if (state == BindState.Pressed)
                this.Exit();
        }

        private void ResetScene(BindState state)
        {
            if (state == BindState.Pressed)
            {
                ScrollerSerializer.ReloadEntities();
                this.GetGameState<SceneManager>().ReloadScenes("", true);
            }
        }

        private void ResetSceneAtPosition(BindState state)
        {
            if (state == BindState.Pressed)
            {
                var oldPosition = Vector2.Zero;
                foreach (var player in this.Players)
                {
                    if (!player.Character.IsDisposed)
                    {
                        oldPosition = player.Character.Position;
                        break;
                    }
                }
                ScrollerSerializer.ReloadEntities();
                this.GetGameState<SceneManager>().ReloadScenes("", true);
                if (oldPosition != Vector2.Zero)
                {
                    foreach (var player in this.Players)
                        player.Character.Position = oldPosition;
                }
            }
        }

        private void ToggleViewEntities(BindState state)
        {
            if (state == BindState.Pressed)
                Debugger.ToggleViewEntities(this.GetGameState<SceneManager>().ActiveScene);
        }

        private void ToggleMultiplayer(BindState state)
        {
            if (state == BindState.Pressed)
            {
                if (this.Players.Count() != 1)
                    this.RemovePlayer(this.Players.Where(p => p.Index == 2).Single());
                else
                {
                    var player1 = this.Players.First();
                    var player2 = CreateNewPlayer();
                    player1.Character.Scene.AddEntity(player2.Character);
                    player2.Character.Position = player1.Character.Position;
                }
            }
        }

    }
}
