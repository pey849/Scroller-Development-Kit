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
    /// A state to show game over stuff. 
    /// </summary>
    public class GameOverState: GameState
    {
        public override bool BlocksDraw { get { return false; } }
        public override bool BlocksUpdate { get { return true; } }

        public GameOverState()
            :base ()
        {
            this.AddComponent(new GameOverStateComponent(this));
        }

        private class GameOverStateComponent : GameStateComponent
        {
            private SpriteFont _Font;
            private Texture2D _over;
            private int _height, _width;
            private Rectangle _rect;

            public GameOverStateComponent(GameState state) 
                : base(state)
            {
                _Font = ScrollerGame.Instance.GlobalContent.Load<SpriteFont>("Fonts/DebugFont");
                _over = ScrollerGame.Instance.GlobalContent.Load<Texture2D>("States/gameover");
                _height = ScrollerGame.Instance.GraphicsDevice.Viewport.Height;// PresentationParameters.BackBufferHeight;
                _width = ScrollerGame.Instance.GraphicsDevice.Viewport.Width;// PresentationParameters.BackBufferWidth;

                _rect = new Rectangle(0, 0, _width, _height);
            }

            protected override void OnUpdate(GameTime gameTime)
            {
            }

            protected override void OnDraw(GameTime gameTime)
            {
                var SpriteBatch = ScrollerGame.Instance.SpriteBatch;
                SpriteBatch.Begin();
                //SpriteBatch.DrawString(_Font, "Game Over \n Press RightShift to retry \n Press Enter to go to Title Screen", new Vector2(150f), Color.Yellow);
                SpriteBatch.Draw(_over, _rect, Color.White);
                SpriteBatch.End();
            }
        }
    }
}
