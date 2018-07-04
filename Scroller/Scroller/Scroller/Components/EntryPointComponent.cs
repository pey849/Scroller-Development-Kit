using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scroller.GameStates;
using ScrollerEngine;
using ScrollerEngine.Components;
using Microsoft.Xna.Framework.Content;

namespace Scroller.Components
{
    /// <summary>
    /// A Collision component used to change the scene (level).
    /// </summary>
    public class EntryPointComponent : CollisionEventComponent
    {
        private bool _IsEnabled = true;
        private string _NextScene = "";

        /// <summary>
        /// Gets or sets whether this is actually enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; }
        }

        /// <summary>
        /// DEPRECIATED: Gets or sets scene to transition to.
        /// This should only be set through Tiled.
        /// </summary>
        [ContentSerializerIgnore]
        public string NextScene
        {
            get { return _NextScene; }
            set { _NextScene = value; }
        }

        protected override bool OnCollision(Entity Entity, EntityClassification Classification)
        {
            if (!IsEnabled)
                return false;
            //TODO: WIll need to make it randomize if NextScene is not set.
            //if (string.IsNullOrEmpty(NextScene))
            //    return false;
            //ScrollerGame.Instance.GameStateManager.PushState<LevelClearState>(true);
            var sceneManager = ScrollerGame.Instance.GetGameState<SceneManager>();
            sceneManager.RandomScene(false);
            //sceneManager.ChangeScene(NextScene, "", false);
            return true;
        }
    }
}
