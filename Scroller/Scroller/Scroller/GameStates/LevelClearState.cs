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
    /// A game state for level cleared event.
    /// </summary>
    public class LevelClearState : GameState
    {
        public override bool BlocksDraw { get { return false; } }
        public override bool BlocksUpdate { get { return true; } }

        public LevelClearState()
            : base()
        {
            this.AddComponent(new LevelClearStateComponent(this));
        }

        private class LevelClearStateComponent : GameStateComponent
        {
            private SpriteFont _Font;
            private Texture2D _levelClear;
            private int _height, _width;
            private Rectangle _rect;

            public LevelClearStateComponent(GameState state)
                : base(state)
            {
                _Font = ScrollerGame.Instance.GlobalContent.Load<SpriteFont>("Fonts/DebugFont");
                _levelClear = ScrollerGame.Instance.GlobalContent.Load<Texture2D>("States/LEVELCLEAR");
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
                //SpriteBatch.DrawString(_Font, "Level Clear \n Press Enter to return go to next level.", new Vector2(100f), Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                SpriteBatch.Draw(_levelClear, _rect, Color.White);
                SpriteBatch.End();
            }
        }
    }
}
