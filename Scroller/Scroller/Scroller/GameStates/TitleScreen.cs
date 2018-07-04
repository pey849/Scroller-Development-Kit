using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrollerEngine;
using ScrollerEngine.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Scroller.GameStates
{
    //TODO: Pretty this up #ISuckAtMakingThingsLookNice

    /// <summary>
    /// Creates a GameState that represents the Title Screen of our game.
    /// </summary>
    public class TitleScreen : GameState
    {
        public override bool BlocksDraw { get { return true; } }
        public override bool BlocksUpdate { get { return true; } }

        public TitleScreen()
        {
            this.AddComponent(new TitleScreenComponent(this));
        }

        //TODO: Make a check for multiplayer or not.

        private class TitleScreenComponent : GameStateComponent
        {
            private SpriteFont _Font;
            private Texture2D _title;
            private int _height, _width;
            private Rectangle _rect;

            public TitleScreenComponent(GameState state)
                : base(state)
            {
                _Font = ScrollerGame.Instance.GlobalContent.Load<SpriteFont>("Fonts/DebugFont");
                _title = ScrollerGame.Instance.GlobalContent.Load<Texture2D>("States/title");
                _height = ScrollerGame.Instance.GraphicsDevice.Viewport.Height;// PresentationParameters.BackBufferHeight;
                _width = ScrollerGame.Instance.GraphicsDevice.Viewport.Width;// PresentationParameters.BackBufferWidth;

                _rect = new Rectangle(0, 0, _width, _height);
            }

            protected override void OnUpdate(Microsoft.Xna.Framework.GameTime gameTime)
            {

            }

            protected override void OnDraw(Microsoft.Xna.Framework.GameTime gameTime)
            {
                //var tempMessage = "Scroller";
                //tempMessage += "\n\nPress Enter to play.";
                //tempMessage += "\n\n(Content is currently temporary. \nAll of the game logic \nto support everything\nis completed though.)";
                
                var SpriteBatch = ScrollerGame.Instance.SpriteBatch;
                SpriteBatch.Begin();
                //SpriteBatch.DrawString(_Font, tempMessage, new Vector2(100f), Color.Orange);
                SpriteBatch.Draw(_title, _rect, Color.White);
                SpriteBatch.End();
            }
        }
    }

}