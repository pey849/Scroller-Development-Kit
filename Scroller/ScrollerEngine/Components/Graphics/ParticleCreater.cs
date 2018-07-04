using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollerEngine.Components.Graphics
{
    /// <summary>
    /// Worked on by Richard, Emmanuel
    /// A class to create particles at a specified location.
    /// </summary>
    public class ParticleCreator
    {
        private int _SpawnNumber = 0;
        private Rectangle _SpawnArea = Rectangle.Empty;
        private List<Texture2D> _Textures = new List<Texture2D>();
        private bool _IsVaryingX = false;
        private bool _IsVaryingY = false;
        private float _VelocityModifier = 1f;
        private float _AngularVelocityMod = 1f;
        private Color _Color = Color.White;
        private float _Duration = 0f;

        private bool _IsFinished = false;

        private Random _Random = new Random();
        private List<Particle> _Particles = new List<Particle>();
        private int _ParticlesSpawned = 0;

        /// <summary>
        /// Gets whether this particle creator is finished spawning particles.
        /// </summary>
        public bool IsFinished { get { return _IsFinished; } }

        /// <summary>
        /// Gets or sets the number of entities to spawn.
        /// </summary>
        public int SpawnNumber
        {
            get { return _SpawnNumber; }
            set { _SpawnNumber = value; }
        }

        /// <summary>
        /// Gets or sets the general area to spawn the particles.
        /// </summary>
        public Rectangle SpawnArea
        {
            get { return _SpawnArea; }
            set { _SpawnArea = value; }
        }

        /// <summary>
        /// Gets or sets the textures to generate.
        /// </summary>
        public List<Texture2D> Textures
        {
            get { return _Textures; }
            set { _Textures = value; }
        }

        /// <summary>
        /// Gets or sets whether the particles will have positive and negative x velocity.
        /// </summary>
        public bool IsVaryingX
        {
            get { return _IsVaryingX; }
            set { _IsVaryingX = value; }
        }

        /// <summary>
        /// Gets or sets whether the particles will have positive and negative y velocity.
        /// </summary>
        public bool IsVaryingY
        {
            get { return _IsVaryingY; }
            set { _IsVaryingY = value; }
        }

        /// <summary>
        /// Gets or sets the velocity speed modifier.
        /// </summary>
        public float VelocityModifier
        {
            get { return _VelocityModifier; }
            set { _VelocityModifier = value; }
        }

        /// <summary>
        /// Gets or sets the angular velocity of the particle.
        /// </summary>
        public float AngularVelocityModifier
        {
            get { return _AngularVelocityMod; }
            set { _AngularVelocityMod = value; }
        }

        /// <summary>
        /// Gets or sets the particle color.
        /// </summary>
        public Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        /// <summary>
        /// Gets or sets how long these particles should last.
        /// </summary>
        public float Duration
        {
            get { return _Duration; }
            set { _Duration = value; }
        }

        public ParticleCreator(int spawnNumber, Rectangle area, List<Texture2D> textures, bool varyX, bool varyY, float velocityMod, float angVelMod, Color color, float duration)
        {
            this.SpawnNumber = spawnNumber;
            this.SpawnArea = area;
            this.Textures = textures;
            this.IsVaryingX = varyX;
            this.IsVaryingY = varyY;
            this.VelocityModifier = velocityMod;
            this.Color = color;
            this.Duration = duration;
            this.AngularVelocityModifier = angVelMod;
        }

        public void Update(GameTime Time)
        {
            if (_ParticlesSpawned < SpawnNumber)
            {
                int num = Math.Min(SpawnNumber - _Particles.Count, 10);
                for (int i = 0; i < num; i++)
                    _Particles.Add(GenerateNewParticle());
                _ParticlesSpawned += num;
            }
            for (int i = 0; i < _Particles.Count; i++)
            {
                var p = _Particles[i];
                p.Update(Time);
                if (p.IsDead)
                {
                    _Particles.RemoveAt(i);
                    i--;
                }
            }
            if (_ParticlesSpawned == SpawnNumber && _Particles.Count == 0)
                _IsFinished = true;
        }

        public void Draw()
        {
            for (int i = 0; i < _Particles.Count; i++)
                _Particles[i].Draw();
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = Textures[_Random.Next(0, Textures.Count)];
            Vector2 position = GetPosition(SpawnArea);

            Vector2 velocity = VelocityModifier * new Vector2(GetSpeed(IsVaryingX), GetSpeed(IsVaryingY));// * 2 - 1));
            float angle = MathHelper.ToRadians(_Random.Next(0, 360));
            float angularVelocity = AngularVelocityModifier * 0.1f * (float)(_Random.NextDouble() * 2 - 1);

            Color color = Color;
            float size = (float)_Random.NextDouble();

            float ttl = Duration + ((float)_Random.NextDouble() * 2 - 1);
            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        private Vector2 GetPosition(Rectangle area)
        {
            var xpos = _Random.Next(area.X, area.X + area.Width);
            var ypos = _Random.Next(area.Y, area.Y + area.Height);
            return new Vector2(xpos, ypos);
        }

        private float GetSpeed(bool isNegativeOrPositive)
        {
            return (float)((isNegativeOrPositive) ? _Random.NextDouble() * 2 - 1 : _Random.NextDouble());
        }

    }
}
