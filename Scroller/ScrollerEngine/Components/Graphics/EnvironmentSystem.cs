using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrollerEngine.Graphics;

namespace ScrollerEngine.Components.Graphics
{
    /// <summary>
    /// Worked on by Richard, Emmanuel
    /// A system for adding environmental effects such as rain, snow, etc, to levels.
    /// </summary>
    public class EnvironmentSystem : SceneSystem
    {
        private Random _Random = new Random();
        private List<Particle> _Particles;
        private List<Texture2D> _Textures;

        /// <summary>
        /// Gets the settings used for this system.
        /// </summary>
        public EnvironmentSettings Settings { get; private set; }

        public EnvironmentSystem(EnvironmentSettings settings)
            : base()
        {
            Settings = settings;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _Particles = new List<Particle>();
            _Textures = new List<Texture2D>();
            foreach (var s in Settings.Textures)
                _Textures.Add(ScrollerBase.Instance.GlobalContent.Load<Texture2D>("Environments/" + s.Trim()));

            InitiateParticles();
        }

        protected override void OnUpdate(GameTime Time)
        {
            if (!Settings.IsEnabled)
                return;

            base.OnUpdate(Time);

            if (_Particles.Count < Settings.MaxParticles)
            {
                for (int i = 0; i < Settings.SpawnCount; i++)
                    _Particles.Add(GenerateNewParticle());
            }
            for (int i = 0; i < _Particles.Count; i++)
            {
                var p = _Particles[i];
                p.Update(Time);
                if (!this.Scene.MapRect.Contains(p.Position))
                {
                    p.Position = GetPosition();
                    //_Particles.RemoveAt(i);
                    //i--;
                }
            }
        }

        protected override void OnDraw()
        {
            if (!Settings.IsEnabled)
                return;

            base.OnDraw();
            for (int i = 0; i < _Particles.Count; i++)
                 _Particles[i].Draw();
        }

        private void InitiateParticles()
        {
            if (!Settings.InitParticles)
                return;

            //An Example of how to fill the level with particles at the start.
            for (int i = 0; i < Settings.MaxParticles; i++)
            {
                var position = new Vector2(_Random.Next(0, (int)this.Scene.MapSize.X), _Random.Next(0, (int)this.Scene.MapSize.Y));
                _Particles.Add(GenerateNewParticle(position));
            }
        }

        private Particle GenerateNewParticle(Vector2 position)
        {
            var particle = GenerateNewParticle();
            particle.Position = position;
            return particle;
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = _Textures[_Random.Next(0, _Textures.Count)];
            Vector2 position = GetPosition();

            Vector2 velocity = Settings.VelocityModifier * new Vector2(GetSpeed(Settings.IsVaryingX), GetSpeed(Settings.IsVaryingY));// * 2 - 1));
            float angle = Settings.InitialAngle;
            float angularVelocity = (Settings.HasAngularVelocity) ? 0.1f * (float)(_Random.NextDouble() * 2 - 1) : 0f;

            Color color = Settings.Color;
            float size = (float)_Random.NextDouble();
            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, 0f); //TTL is not used in the environmentsystem.
        }

        private Vector2 GetPosition()
        {
            Vector2 pos = new Vector2();
            switch (Settings.SpawnSide)
            {
                case Direction.Up:
                    pos = new Vector2(_Random.Next(0, (int)this.Scene.MapSize.X), 0);
                    break;
                case Direction.Right:
                    pos = new Vector2(this.Scene.MapSize.X, _Random.Next(0, (int)this.Scene.MapSize.Y));
                    break;
                case Direction.Down:
                    pos = new Vector2(_Random.Next(0, (int)this.Scene.MapSize.X), (int)this.Scene.MapSize.Y);
                    break;
                case Direction.Left:
                    pos = new Vector2(0, _Random.Next(0, (int)this.Scene.MapSize.Y));
                    break;
            }

            return pos;
        }

        private float GetSpeed(bool isNegativeOrPositive)
        {
            return (float)((isNegativeOrPositive) ? _Random.NextDouble() * 2 - 1 : _Random.NextDouble());
        }

    }
}
