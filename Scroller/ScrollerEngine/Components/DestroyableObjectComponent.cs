using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ScrollerEngine;
using ScrollerEngine.Components.Graphics;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Richard, Jonathan
    /// A Component to manage objects that can die.
    /// </summary>
    public class DestroyableObjectComponent : Component
    {
        private const int MINIMUM_NUM_PARTICLES = 4;
        private ParticleSystem PS;

        private List<Texture2D> _Textures = new List<Texture2D>();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            this._Textures.Add(ScrollerBase.Instance.GlobalContent.Load<Texture2D>("Sprites/Misc/Cloud1"));
            this._Textures.Add(ScrollerBase.Instance.GlobalContent.Load<Texture2D>("Sprites/Misc/Cloud2"));
            
            PS = this.Parent.Scene.GetSystem<ParticleSystem>();

            HealthComponent HC = this.Parent.GetComponent<HealthComponent>();
            if (HC != null)
                HC.Died += HC_Died;

            this.Parent.Disposed += Parent_Disposed;
        }

        void Parent_Disposed(SceneObject obj)
        {
            int area = (int)(this.Parent.Size.X * this.Parent.Size.Y);
            int numParticles = MINIMUM_NUM_PARTICLES + area / (32 * 32);
            List<Texture2D> textures = _Textures.ToList();
            PS.GenerateParticles(numParticles, this.Parent.Location, textures, true, true, 0.1f, 0.2f, Color.White, 1f);
        }

        void HC_Died(HealthComponent obj)
        {
            Parent.Dispose();
        }
    }
}
