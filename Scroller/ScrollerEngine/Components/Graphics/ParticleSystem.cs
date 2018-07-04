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
    /// A SceneSystem used for drawing special effects.
    /// </summary>
    public class ParticleSystem : SceneSystem
    {
        private List<ParticleCreator> _ParticlesToSpawn;

        /// <summary>
        /// Generates particles with the specified variables.
        /// </summary>
        public void GenerateParticles(int spawnNumber, Rectangle area, List<Texture2D> textures, bool varyX, bool varyY, float velocityMod, float angVelMod, Color color, float duration)
        {
            var pc = new ParticleCreator(spawnNumber, area, textures, varyX, varyY, velocityMod, angVelMod, color, duration);
            _ParticlesToSpawn.Add(pc);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _ParticlesToSpawn = new List<ParticleCreator>();
        }

        protected override void OnUpdate(GameTime Time)
        {
            base.OnUpdate(Time);
            for (int i = 0; i < _ParticlesToSpawn.Count; i++)
            {
                var pc = _ParticlesToSpawn[i];
                pc.Update(Time);
                if (pc.IsFinished)
                {
                    _ParticlesToSpawn.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            for (int i = 0; i < _ParticlesToSpawn.Count; i++)
                _ParticlesToSpawn[i].Draw();
        }
    }
}
