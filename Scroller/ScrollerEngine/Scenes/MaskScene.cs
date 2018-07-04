using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrollerEngine.Components;
using ScrollerEngine.Components.Graphics;
using ScrollerEngine.Engine;
using ScrollerEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollerEngine.Scenes
{
    //NOTE: This is all test. Not actually optimized as you can see from the drastic FPS drop.
    /// <summary>
    /// Worked on by Richard
    /// </summary>
    public class MaskScene : Scene
    {
        private Shader _Shader;
        private List<RenderTarget2D> _RenderedObjects = new List<RenderTarget2D>();

        public MaskScene(GameState state, LevelData data)
            : base(state, data)
        {
            var eSettings = new EnvironmentSettings()
            {
                IsEnabled = true,
                InitParticles = false,
                MaxParticles = 500,
                SpawnCount = 10,
                Textures = new List<string>() { "Snowflake1", "Snowflake2" },
                SpawnSide = Direction.Up,
                VelocityModifier = new Vector2(0.2f),
                IsVaryingX = true,
                IsVaryingY = false,
                InitialAngle = 0f,
                HasAngularVelocity = true,
                Color = Color.White
            };

            this.AddSystem(new EnvironmentSystem(eSettings));

            this._Shader = new Shader("Shaders/Masks/TestMask");
            var GraphicsDevice = ScrollerBase.Instance.GraphicsDevice;
            foreach (var layer in this.Layers)
                this._RenderedObjects.Add(new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferWidth));
            this._RenderedObjects.Add(new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferWidth));
            this._RenderedObjects.Add(new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferWidth));
            this._RenderedObjects.Add(new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferWidth));
            
        }
        
        protected override void OnDraw(GameTime gameTime)
        {
            var SpriteBatch = ScrollerBase.Instance.SpriteBatch;
            var GraphicsDevice = ScrollerBase.Instance.GraphicsDevice;
            int Counter = 0;

            var startTile = CameraManager.Active.Position / TileSize;
            var endTile = (CameraManager.Active.Position + CameraManager.Active.ViewportSize) / TileSize;

            int startX = Math.Max((int)startTile.X, 0);
            int startY = Math.Max((int)startTile.Y, 0);
            int endX = Math.Min((int)(endTile.X + 0.5f), (int)TilesInMap.X - 1);
            int endY = Math.Min((int)(endTile.Y + 0.5f), (int)TilesInMap.Y - 1);

            float c = 0.1f;
            foreach (var layer in Layers)
            {
                GraphicsDevice.SetRenderTarget(_RenderedObjects[Counter], Color.Transparent);
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        Tile tile = layer.GetTile(x, y);
                        if (tile == null)
                            continue;

                        //if (layer.IsSolid && layer.TopCollisionOnly)
                        //{
                        //    _Shader.Effect.Parameters["_Color"].SetValue(new Vector4(0.4f, 0.4f, 0.4f, 1f));
                        //    _Shader.Effect.CurrentTechnique.Passes[3].Apply();
                        //}
                        var screenCoords = CameraManager.Active.WorldToScreen(new Vector2(tile.Location.X, tile.Location.Y));
                        SpriteBatch.Draw(tile.Texture, new Rectangle((int)screenCoords.X, (int)screenCoords.Y, tile.Location.Width, tile.Location.Height), tile.SourceRect, Color.White);
                    }
                }
                SpriteBatch.End();
                Counter++;
                c += 0.15f;

                if (layer.IsSolid && layer.TopCollisionOnly)
                {
                    GraphicsDevice.SetRenderTarget(_RenderedObjects[Counter], Color.Transparent);
                    SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    foreach (var e in this._Entities)
                        if(CameraManager.Active.Contains(e))
                            e.Draw();
                    SpriteBatch.End();
                    Counter++;

                    GraphicsDevice.SetRenderTarget(_RenderedObjects[Counter], Color.Transparent);
                    SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    float i = 0;
                    foreach (var p in ScrollerBase.Instance.Players)
                    {
                        p.Character.Draw();
                        i += 1f;
                    }
                    SpriteBatch.End();
                    Counter++;
                }
            }

            GraphicsDevice.SetRenderTarget(_RenderedObjects[Counter], Color.Transparent);
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            foreach (var system in this.Systems)
                system.Draw();
            SpriteBatch.End();
            Counter++;
            
            //var mask = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferWidth);
            //GraphicsDevice.SetRenderTarget(mask, Color.Transparent);
            //SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            //SpriteBatch.Draw(_Shader.MaskTexture, new Rectangle(0, 0, (int)CameraManager.Active.Size.X, (int)CameraManager.Active.Size.Y), null, Color.White);
            //SpriteBatch.End();
            //dont increment counter here

            GraphicsDevice.SetRenderTarget(null, Color.DarkSlateBlue);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            //_Shader.Effect.Parameters["Mask"].SetValue(mask);c
            //_Shader.Effect.CurrentTechnique.Passes[1].Apply();
            float sad = 0f;
            foreach (var render in _RenderedObjects)
            {
                _Shader.Effect.Parameters["Color"].SetValue(new Vector4(1 - sad));
                _Shader.Effect.CurrentTechnique.Passes[3].Apply();
                SpriteBatch.Draw(render, Vector2.Zero, Color.White);
                sad += 0.1f;
            }
            SpriteBatch.End();

            //mask.Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var render in _RenderedObjects)
                render.Dispose();
        }

    }
}
