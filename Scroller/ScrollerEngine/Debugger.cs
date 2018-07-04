using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ScrollerEngine;
using ScrollerEngine.Scenes;
using ScrollerEngine.Graphics;
using ScrollerEngine.Components;

namespace System
{
    /// <summary>
    /// Worked on by Richard, Jonathan
    /// A global debugger designed specifically for Scroller.
    /// </summary>
    public class Debugger : DrawableGameComponent
    {
        private static Dictionary<int, Dictionary<string, object>> PagedContent = new Dictionary<int, Dictionary<string, object>>();
        private static Dictionary<string, DebugRectangle> OutlinedObjects = new Dictionary<string, DebugRectangle>();
        public static Color FontColor = Color.White;

        private static SpriteFont _Font;
        private static Texture2D _Texture;
        private static Scene _ViewActiveScene = null;

        private int _CurrentPage = 0;

        KeyboardState _LastKeyPressed;
        KeyboardState _Keypressed;

        /// <summary>
        /// Draws a single line of text.
        /// </summary>
        /// <param name="display">The name of the property to display.</param>
        /// <param name="content">The actually data to display.</param>
        /// <param name="page">Which page to put it in.</param>
        public static void DrawContent(string display, object content, int page = 0)
        {
            if (!PagedContent.ContainsKey(page))
                PagedContent.Add(page, new Dictionary<string, object>());

            if (PagedContent[page].ContainsKey(display))
                PagedContent[page][display] = content;
            else
                PagedContent[page].Add(display, content);
        }

        /// <summary>
        /// Adds an object to outline.
        /// </summary>
        /// <param name="id">The id for this object.</param>
        /// <param name="rect">The rectangle  to outline.</param>
        public static void OutlineRectangle(string id, Rectangle rect)
        {
            if (OutlinedObjects.ContainsKey(id))
                OutlinedObjects[id] = new DebugRectangle(id, rect);
            else
                OutlinedObjects.Add(id, new DebugRectangle(id, rect));
        }


        /// <summary>
        /// Resets the debugger.
        /// </summary>
        public static void Reset()
        {
            PagedContent.Clear();
        }

        /// <summary>
        /// Causes the debugger to break if the condition is true. 
        /// It's like putting a break point except with an if statement.
        /// </summary>
        public static void BreakIf(bool condition)
        {
#if DEBUG
            if(condition)
                System.Diagnostics.Debugger.Break();
#endif 
        }

        /// <summary>
        /// Toggles whether to draw a rectangle around and entities.
        /// </summary>
        public static void ToggleViewEntities(Scene scene)
        {
            _ViewActiveScene = _ViewActiveScene == null ? scene : null;
        }

        public Debugger(Game game, string fontLocation, string textureLocation)
            : base(game)
        {
            _Font = ScrollerBase.Instance.GlobalContent.Load<SpriteFont>(fontLocation);
            _Texture = ScrollerBase.Instance.GlobalContent.Load<Texture2D>(textureLocation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HandleKeypressed();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var SpriteBatch = ScrollerBase.Instance.SpriteBatch;
            SpriteBatch.Begin();

            string debugHeader = string.Format("Debugger:: Page: {0}", _CurrentPage.ToString());
            //Draw text
            if (PagedContent.ContainsKey(_CurrentPage))
            {
                int i = 1;
                foreach (var item in PagedContent[_CurrentPage])
                {
                    string s = string.Format("{0}: {1}", item.Key, item.Value.ToString());
                    SpriteBatch.DrawString(_Font, s, new Vector2(0, i * _Font.LineSpacing), FontColor);
                    i++;
                }
            }
            //Draw Entities.
            if (_ViewActiveScene != null)
            {
                if (_ViewActiveScene.IsDisposed)
                    _ViewActiveScene = null;
                else
                {
                    var outlineTex = ScrollerBase.Instance.GlobalContent.Load<Texture2D>("Debug/Outline");
                    foreach (var entity in _ViewActiveScene.Entities.ToArray())
                    {
                        SpriteBatch.DrawString(_Font, entity.Name, CameraManager.Active.WorldToScreen(entity.Position + new Vector2(0, -16f)), Color.Black, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                        SpriteBatch.Draw(outlineTex, CameraManager.Active.WorldToScreen(entity.Location), null, Color.White);
                    }
                }
            }

            foreach (var rect in OutlinedObjects.Values)
                rect.Draw();

            //draw header
           // ScrollerBase.Instance.SpriteBatch.DrawString(_Font, debugHeader, new Vector2(), FontColor);

            ScrollerBase.Instance.SpriteBatch.End();
        }

        private void HandleKeypressed()
        {
            _LastKeyPressed = _Keypressed;
            _Keypressed = Keyboard.GetState();

            if (IsKeyPressed(Keys.OemTilde))
            {
                _CurrentPage++;
                if (_CurrentPage >= PagedContent.Count)
                    _CurrentPage = 0;
            }
        }

        private bool IsKeyPressed(Keys key)
        {
            return _Keypressed.IsKeyDown(key) && _LastKeyPressed.IsKeyUp(key);
        }

        private class DebugRectangle
        {
            public string Id { get; set; }
            public Rectangle LSide { get; set; }
            public Rectangle TSide { get; set; }
            public Rectangle RSide { get; set; }
            public Rectangle BSide { get; set; }

            public DebugRectangle(string id, Rectangle rect)
            {
                this.Id = id;
                this.LSide = new Rectangle(rect.Left, rect.Top, 2, rect.Height);
                this.TSide = new Rectangle(rect.Left, rect.Top, rect.Width, 2);
                this.RSide = new Rectangle(rect.Right, rect.Top, 2, rect.Height);
                this.BSide = new Rectangle(rect.Left, rect.Bottom, rect.Width, 2);
            }

            public void Update(GameTime Time) { }

            public void Draw()
            {
                Vector2 textPos = new Vector2(TSide.X, TSide.Y - 17);
                ScrollerBase.Instance.SpriteBatch.DrawString(_Font, Id, ScrollerEngine.Graphics.CameraManager.Active.WorldToScreen(textPos), Color.Red, 0f, new Vector2(), 0.7f, SpriteEffects.None, 0f);
                var SpriteBatch = ScrollerBase.Instance.SpriteBatch;
                Rectangle spriteSize = new Rectangle(16, 0, 2, 2);
                SpriteBatch.Draw(_Texture, ScrollerEngine.Graphics.CameraManager.Active.WorldToScreen(LSide), spriteSize, Color.Red);
                SpriteBatch.Draw(_Texture, ScrollerEngine.Graphics.CameraManager.Active.WorldToScreen(TSide), spriteSize, Color.Red);
                SpriteBatch.Draw(_Texture, ScrollerEngine.Graphics.CameraManager.Active.WorldToScreen(RSide), spriteSize, Color.Red);
                SpriteBatch.Draw(_Texture, ScrollerEngine.Graphics.CameraManager.Active.WorldToScreen(BSide), spriteSize, Color.Red);
            }
        }
    }


}
