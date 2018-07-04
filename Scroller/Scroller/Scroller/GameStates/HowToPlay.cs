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
    /// <summary>
    /// A GameState to show how to play our game.
    /// </summary>
    public class HowToPlay : GameState
    {
        public override bool BlocksDraw { get { return true; } }
        public override bool BlocksUpdate { get { return true; } }

        public HowToPlay()
            :base ()
        {
            this.AddComponent(new HowToPlayComponent(this));
        }

        private class HowToPlayComponent : GameStateComponent
        {
            private SpriteFont _Font;
            private Texture2D _howTo;
            private int _height, _width;
            private Rectangle _rect;

            public HowToPlayComponent(GameState state) 
                : base(state)
            {
                _Font = ScrollerGame.Instance.GlobalContent.Load<SpriteFont>("Fonts/DebugFont");
                _howTo = ScrollerGame.Instance.GlobalContent.Load<Texture2D>("States/howTo");
                _height = ScrollerGame.Instance.GraphicsDevice.Viewport.Height;// PresentationParameters.BackBufferHeight;
                _width = ScrollerGame.Instance.GraphicsDevice.Viewport.Width;// PresentationParameters.BackBufferWidth;

                _rect = new Rectangle(0, 0, _width, _height);
            }

            protected override void OnUpdate(GameTime gameTime)
            {
            }

            protected override void OnDraw(GameTime gameTime)
            {
                //string message = "How To Play:";
                //message += "\n Player 1 (Red):";
                //message += "\n      Left/Right Arrow Keys to move left/right.";
                //message += "\n      Up Arrow key to jump.\n";
                //message += "\n Player 2 (Green):";
                //message += "\n      A/D Keys to move left/right.";
                //message += "\n      W Key to jump.";
                var SpriteBatch = ScrollerGame.Instance.SpriteBatch;
                SpriteBatch.Begin();
                //SpriteBatch.DrawString(_Font, message, new Vector2(100), Color.Yellow);
                SpriteBatch.Draw(_howTo, _rect, Color.White);
                SpriteBatch.End();
            }
        }
    }
}
