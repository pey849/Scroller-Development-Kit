using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScrollerEngine;
//using ScrollerEngine.Input;
using ScrollerEngine.Engine;

namespace Scroller.GameStates
{
    public class Menu : GameState
    { 
        public override bool BlocksDraw
        {
            get { return true; }
        }

        public override bool BlocksUpdate
        {
            get { return true; }
        }
        public Menu()
        {
            ScrollerGame.Instance.Game.Window.Title = "Scroller";
            this.AddComponent(new TestComponent1(this));
        }

    }

    public class TestComponent1 : GameStateComponent
    {
        public TestComponent1(GameState state)
            : base(state) 
        {}

        float _height = ScrollerGame.Instance.GraphicsDevice.Viewport.Height;
        float _width = ScrollerGame.Instance.GraphicsDevice.Viewport.Width;

        TimeSpan timer = TimeSpan.Zero;
        protected override void OnUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;
            if (timer.TotalSeconds > 2)
            {
                timer = TimeSpan.Zero;
                ScrollerGame.Instance.GameStateManager.PushState<SceneManager>(false);
            }
        }

        protected override void OnDraw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScrollerGame.Instance.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Red);
            ScrollerGame.Instance.SpriteBatch.Begin();
            ScrollerGame.Instance.SpriteBatch.DrawString(ScrollerGame.Instance.GlobalContent.Load<SpriteFont>("Fonts/DebugFont"), 
                "MENU",
                new Vector2(_width/2, _height/2),
                Color.White);
            ScrollerGame.Instance.SpriteBatch.End();
        }
    }
}
