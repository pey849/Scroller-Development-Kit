using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrollerEngine;
using ScrollerEngine.Components;
using ScrollerEngine.Components.Graphics;
using Microsoft.Xna.Framework;

namespace Scroller
{
    /// <summary>
    /// Provides an implementation of Player to be used in Scroller.
    /// </summary>
    public class ScrollerPlayer : Player
    {
        /// <summary>
        /// Creates a new ScrollerPlayer.
        /// </summary>
        public ScrollerPlayer(Entity character) : base(character) { }

        /// <summary>
        /// Loads a new Entity that can be used for the player.
        /// </summary>
        public static Entity LoadPlayerEntity()
        {
            var playerEntity = ScrollerSerializer.CreateEntity("Player"); //Might make this variable?
            playerEntity.Name = "Player" + (ScrollerBase.Instance.Players.Count() + 1).ToString();
            var sc = playerEntity.GetComponent<SpriteComponent>();
            sc.ColorTint = ScrollerGame.Instance.Players.Count() == 0 ? Color.Red : Color.Green;
            return playerEntity;
        }

        /// <summary>
        /// Resets this player's character to the default one loaded from LoadPlayerEntity.
        /// </summary>
        public void ResetCharacter()
        {
            var name = this.Character.Name;
            var colorTint = this.Character.GetComponent<SpriteComponent>().ColorTint;
            this.Character = LoadPlayerEntity();
            this.Character.Name = name;
            this.Character.GetComponent<SpriteComponent>().ColorTint = colorTint;
        }

    }
}
