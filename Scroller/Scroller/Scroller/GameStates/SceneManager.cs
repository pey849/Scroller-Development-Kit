using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ScrollerEngine.Scenes;
using ScrollerEngine;
using ScrollerEngine.Engine;
using ScrollerEngine.Components.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Scroller.GameStates
{
    /// <summary>
    /// A delegate called when a scene is changed.
    /// Note that OldScene may be null if the state just switched to the SceneManager loading a Scene for the first time.
    /// </summary>
    public delegate void SceneChangedDelegate(Scene oldScene, Scene newScene);

    /// <summary>
    /// Provides a GameState that manages Scenes within the game.
    /// </summary>
    public class SceneManager : GameState
    {
        private const string LEVEL_PATH = "Data/Levels/";

        private List<string> AllLevels = new List<string>();
        private Scene _ActiveScene;
        private RespawnComponent RC;

        public override bool BlocksDraw { get { return true; } }
        public override bool BlocksUpdate { get { return true; } }

        /// <summary>
        /// Gets the currently active Scene in the game.
        /// </summary>
        public Scene ActiveScene { get { return _ActiveScene; } }

        /// <summary>
        /// Loads a random scene.
        /// </summary>
        /// <param name="resetPlayers"></param>
        public void RandomScene(bool resetPlayers = false)
        {
            if (AllLevels.Count == 0)
                GetAllLevels();
            Random rand = new Random();
            int levelId = rand.Next(0, AllLevels.Count);
            string sceneName = AllLevels[levelId];
            AllLevels.RemoveAt(levelId);
            ChangeScene(sceneName, "", resetPlayers);
        }

        /// <summary>
        /// Changes the scene to the Scene with the specified name.
        /// Only need to pass the level name (IE: no extension needed).
        /// If the Scene has already been loaded, it will reuse that instance of the scene.
        /// Otherwise, a new Scene will be loaded.
        /// </summary>
        public Scene ChangeScene(string levelName, string entryPoint, bool resetPlayers = false)
        {
            if (_ActiveScene != null)
                _ActiveScene.Dispose();
            Scene scene = Load(levelName);
            this.AddComponent(scene);
            _ActiveScene = scene;
            _ActiveScene.Visible = true;
            _ActiveScene.Enabled = true;

            RC.Reset();
            foreach (ScrollerPlayer player in ScrollerBase.Instance.Players)
            {
                if (player.Character != null)
                {
                    if (resetPlayers)
                        player.ResetCharacter();
                    if (!player.Character.IsDisposed) // Dispose it to transfer to a new Scene.
                        player.Character.Dispose();
                    var position = player.Character.Position;
                    _ActiveScene.AddEntity(player.Character);
                    var entry = GetEntryPoint(entryPoint);
                    if (entry != null)
                        position = entry.Position;
                    player.Character.Position = position; 
                }
            }
            return scene;
        }

        /// <summary>
        /// Reloads all scenes, including the currently active scene.
        /// </summary>
        public void ReloadScenes(string entryPoint, bool resetPlayers)
        {
            string activeName = _ActiveScene.Name;
            foreach (var playerChar in ScrollerGame.Instance.Players)
                if (!playerChar.Character.IsDisposed)
                    playerChar.Character.Dispose();
            _ActiveScene.Dispose();
            _ActiveScene = null;
            ChangeScene(activeName, entryPoint, resetPlayers);
        }

        /// <summary>
        /// Resets the SceneManager.
        /// </summary>
        public void ResetScenes()
        {
            if (_ActiveScene != null)
                _ActiveScene.Dispose();
            _ActiveScene = null;
            GetAllLevels();
        }

        public SceneManager()
            : base()
        {
            RC = new RespawnComponent(this);
            this.AddComponent(RC);
            GetAllLevels();
        }

        private void GetAllLevels()
        {
            AllLevels = new List<string>();
            foreach (var file in Directory.GetFiles(LEVEL_PATH, "*.tmx"))
            {
                var name = file.Split('/').Last().Split('.').First();
                AllLevels.Add(name);
            }
        }

        private Scene Load(string levelname)
        {
            Scene scene = null;
            while (true)
            {
                try
                {
                    LevelData data = LevelData.LoadLevel(LEVEL_PATH + levelname + ".tmx");
                    scene = new Scene(this, data);
                    scene.Initialize();
                    break;
                }
                catch (Exception e1)
                {
                    var message = e1.Message;
                    message = string.Format("An error occured: \n\n{0}\n\nClick 'OK' to try again.", message);
                    if (System.Windows.Forms.MessageBox.Show(message, "Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        Environment.Exit(-1);
                }
            }
            scene.Disposed += scene_Disposed;
            return scene;
        }

        private ScrollerEngine.Components.Entity GetEntryPoint(string entryName)
        {
            if (string.IsNullOrEmpty(entryName))
                return ActiveScene.Entities.Where(e => e.Name.Equals("StartPoint")).SingleOrDefault();
            return ActiveScene.Entities.Where(e => e.Name.Equals(entryName)).SingleOrDefault();
        }

        void scene_Disposed(Scene obj)
        {
            //_ActiveScenes.Remove(obj.Name);
            this.RemoveComponent(obj);
        }

        /// <summary>
        /// A component used to manage respawning.
        /// </summary>
        private class RespawnComponent : GameStateComponent
        {
            private const float RESPAWN_DELAY = 3f;
            
            private Dictionary<string, PlayerInfo> _PlayersToRespawn = new Dictionary<string, PlayerInfo>();
            private bool _AllPlayersDead = false;
            private bool _GameOver = false;
            private DateTime _AllDeadTime;
            public new SceneManager GameState { get { return (SceneManager)base.GameState; } }

            /// <summary>
            /// Resets the players to spawn.
            /// </summary>
            public void Reset()
            {
                _PlayersToRespawn = new Dictionary<string, PlayerInfo>();
                _AllPlayersDead = false;
                _GameOver = false;
            }
            
            public RespawnComponent(GameState state)
                :base(state)
            { }

            protected override void OnUpdate(GameTime gameTime)
            {
                if (_AllPlayersDead)
                {
                    if (_GameOver)
                        return;
                    if ((DateTime.Now - _AllDeadTime).TotalSeconds > RESPAWN_DELAY)
                    {
                        _GameOver = true;
                        //_AllPlayersDead = false;
                        ScrollerGame.Instance.GameStateManager.PushState<GameOverState>(true);
                        //GameState.ReloadScenes("", true);
                    }
                }
                else
                {
                    _AllPlayersDead = true;
                    foreach (ScrollerPlayer player in ScrollerGame.Instance.Players)
                    {
                        if (!player.Character.IsDisposed)
                        {
                            _AllPlayersDead = false;
                            _AllDeadTime = DateTime.Now;
                        }
                        var pi = new PlayerInfo() { Player = player };
                        if (_PlayersToRespawn.ContainsKey(pi.Player.Character.Name))
                            continue;
                        if (pi.Player.Character.IsDisposed)
                            _PlayersToRespawn.Add(pi.Player.Character.Name, pi);
                    }

                    foreach (var playerInfo in _PlayersToRespawn.Values.ToList())
                    {
                        playerInfo.Update(gameTime);
                        if (playerInfo.TimeOfDeathCounter > RESPAWN_DELAY)
                        {
                            _PlayersToRespawn.Remove(playerInfo.Player.Character.Name);
                            playerInfo.Player.ResetCharacter();
                            foreach (var player in ScrollerGame.Instance.Players)
                                if (!player.Character.IsDisposed)
                                    playerInfo.Player.Character.Position = new Vector2(player.Character.Position.X, player.Character.Position.Y - 35);
                            playerInfo.Player.Character.GetComponent<SpriteComponent>().Flicker(3f);
                            GameState.ActiveScene.AddEntity(playerInfo.Player.Character);
                        }
                    }
                }
            }

            protected override void OnDraw(GameTime gameTime)
            {

            }

            private class PlayerInfo
            {
                public ScrollerPlayer Player { get; set; }
                public float TimeOfDeathCounter { get; private set; }

                public void Update(GameTime time)
                {
                    TimeOfDeathCounter += time.GetTimeScalar();
                }
            }
        }

    }

}
