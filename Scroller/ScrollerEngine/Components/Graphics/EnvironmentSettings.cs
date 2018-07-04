using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollerEngine.Components.Graphics
{
    /// <summary>
    /// Worked on by Mary, Richard, Emmanuel
    /// A class to manage environment settings.
    /// </summary>
    public class EnvironmentSettings
    {
        private bool _IsEnabled = false;
        private bool _InitParticles = false;
        private int _MaxParticles = 0;
        private int _SpawnCount = 0;
        private List<string> _Textures = new List<string>();
        private Direction _SpawnSide = Direction.Up;
        private Vector2 _VelocityModifier = new Vector2(1f);
        private bool _IsVaryingX = true;
        private bool _IsVaryingY = false;
        private float _InitialAngle = 0f;
        private bool _HasAngularVelocity = true;
        private Color _Color = Color.White;

        /// <summary>
        /// Gets or sets whether to use environmental effects.
        /// </summary>
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; }
        }

        /// <summary>
        /// Gets or sets whether to have the particles already added to the scene.
        /// </summary>
        public bool InitParticles
        {
            get { return _InitParticles; }
            set { _InitParticles = value; }
        }

        /// <summary>
        /// Gets or sets the maximum allowed particles at a time.
        /// </summary>
        public int MaxParticles
        {
            get { return _MaxParticles; }
            set { _MaxParticles = value; }
        }

        /// <summary>
        /// Gets or sets the number of particles to spawn each time update is called.
        /// </summary>
        public int SpawnCount
        {
            get { return _SpawnCount; }
            set { _SpawnCount = value; }
        }

        /// <summary>
        /// Gets or sets the textures to use for this environment.
        /// </summary>
        public List<string> Textures
        {
            get { return _Textures; }
            set { _Textures = value; }
        }

        /// <summary>
        /// Gets or sets the side from which the particles spawn from.
        /// </summary>
        public Direction SpawnSide
        {
            get { return _SpawnSide; }
            set { _SpawnSide = value; }
        }

        /// <summary>
        /// Gets or sets the unit vector for the velocity.
        /// </summary>
        public Vector2 VelocityModifier
        {
            get { return _VelocityModifier; }
            set { _VelocityModifier = value; }
        }

        /// <summary>
        /// Gets or sets whether the X velocity is negative or positive.
        /// </summary>
        public bool IsVaryingX
        {
            get { return _IsVaryingX; }
            set { _IsVaryingX = value; }
        }

        /// <summary>
        /// Gets or sets whether the Y velocity is negative or positive.
        /// </summary>
        public bool IsVaryingY
        {
            get { return _IsVaryingY; }
            set { _IsVaryingY = value; }
        }

        /// <summary>
        /// Gets or sets the initial angle of the particles
        /// </summary>
        public float InitialAngle
        {
            get { return _InitialAngle; }
            set { _InitialAngle = value; }
        }

        /// <summary>
        /// Gets or sets whether the particles rotate.
        /// </summary>
        public bool HasAngularVelocity
        {
            get { return _HasAngularVelocity; }
            set { _HasAngularVelocity = value; }
        }

        /// <summary>
        /// Gets or sets the color of the particles.
        /// </summary>
        public Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }
    }
}
