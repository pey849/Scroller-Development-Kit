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
    /// A state for pausing the game.
    /// </summary>
    public class PauseState : GameState
    {
        public override bool BlocksDraw { get { return false; } }
        public override bool BlocksUpdate { get { return true; } }

        public PauseState()
            :base ()
        {
            this.AddComponent(new PauseStateComponent(this));
        }

        private class PauseStateComponent : GameStateComponent
        {
            private SpriteFont _Font;
            private Texture2D _paused;
            private int _height, _width;
            private Rectangle _rect;

            public PauseStateComponent(GameState state) 
                : base(state)
            {
                _Font = ScrollerGame.Instance.GlobalContent.Load<SpriteFont>("Fonts/DebugFont");
                _paused = ScrollerGame.Instance.GlobalContent.Load<Texture2D>("States/paused");
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
                //SpriteBatch.DrawString(_Font, "Paused \n Press Enter to return to title screen \n Press RightShift or E to continue", new Vector2(100f), Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                SpriteBatch.Draw(_paused, _rect, Color.White);
                SpriteBatch.End();
            }
        }
    }

}
