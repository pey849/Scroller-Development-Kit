using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ScrollerEngine.Components;
using ScrollerEngine.Scenes;

namespace ScrollerEngine.Graphics
{
    /// <summary>
    /// Worked on by Mary, Richard, Peter
    /// A global component to manage the camera used in game.
    /// </summary>
    public class CameraManager : GameComponent
    {
        private const float MAX_BOUNDARY_RANGE = 0f;

        private static Camera _Active;
        private Player _Player;
        private Player _Player2;

        /// <summary>
        /// Gets the current active camera.
        /// </summary>
        public static Camera Active { get { return _Active; } }

        /// <summary>
        /// Sets the camera to the player to follow.
        /// </summary>
        public void FollowPlayer(Player player, Player player2)
        {
            this._Player = player;
            this._Player2 = player2;
        }

        public CameraManager()
            : base(ScrollerBase.Instance.Game)
        {
            _Active = new Camera();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //TODO: Need to do it the way we had planned. Currently follows the player.
            if (_Player == null)
                return;
            var activeScene = (this._Player.Character.Scene != null) ? this._Player.Character.Scene : this._Player2.Character.Scene;
            if (activeScene == null)
                return;

            //var camPos = _Player.Character.Position - (Active.ViewportSize / 2);
            var camPos = ((_Player.Character.Position + _Player2.Character.Position) / 2) - (Active.ViewportSize / 2);
            Active.Position = CheckCameraConstraints(camPos, activeScene);
        }

        private Vector2 CheckCameraConstraints(Vector2 camPos, Scene scene)
        {
            Vector2 position = camPos;
            position.X = Math.Max(position.X, MAX_BOUNDARY_RANGE);
            position.Y = Math.Max(position.Y, MAX_BOUNDARY_RANGE);
            position.X = Math.Min(position.X, scene.MapSize.X - Active.ViewportSize.X - MAX_BOUNDARY_RANGE);
            position.Y = Math.Min(position.Y, scene.MapSize.Y - Active.ViewportSize.Y - MAX_BOUNDARY_RANGE);
            return position;
        }


    }
}
