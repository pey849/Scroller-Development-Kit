using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollerEngine.Scenes
{
    /// <summary>
    /// Worked on by Richard, Jonathan
    public class Shader
    {
        private Effect _ShaderEffect = ScrollerBase.Instance.GlobalContent.Load<Effect>("Shaders/Effect1");
        private Texture2D _MaskTexture;

        public Effect Effect { get { return _ShaderEffect; } }
        public Texture2D MaskTexture { get { return _MaskTexture; } }

        public Shader(string maskTexture)
        {
            this._MaskTexture = ScrollerBase.Instance.GlobalContent.LoadTexture2D(maskTexture);
        }



    }
}
