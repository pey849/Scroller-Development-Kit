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
    /// Worked on by Richard
    /// The basic AI component. Enemy will move left to right with the optional to stay on the platform.
    /// </summary>
    public class BasicAIComponent : AIComponent
    {
        private bool _ReverseAtEdge = false;
        private Vector2 _PrevPosition;

        /// <summary>
        /// Gets or sets whether this entity will reverse at edges. 
        /// </summary>
        public bool ReverseAtEdge
        {
            get { return _ReverseAtEdge; }
            set { _ReverseAtEdge = value; }
        }

        protected override void OnAiUpdate(GameTime gameTime)
        {
            //The initial case which cases the entity to move towards the player.
            if (!MC.IsMoving)
                MC.BeginMove(this.Parent.GetDirectionWRTEntity(ScrollerBase.Instance.Players.First().Character));
            //if it's stuck, reverse.
            if (_PrevPosition == this.Parent.Position)
                MC.BeginMove(MC.CurrentDirection.Reverse());
            else if (ReverseAtEdge && PC.IsGrounded)
            {
                var deltaX = 5;
                var xpos = (MC.CurrentDirection == Direction.Left) ? this.Parent.Location.Left - deltaX : this.Parent.Location.Right + deltaX;
                var ypos = this.Parent.Location.Bottom + 5;
                var tilePosition = new Vector2(xpos, ypos);
                if (!PS.IsLocationSolid(tilePosition))
                    MC.BeginMove(MC.CurrentDirection.Reverse());
            }
            _PrevPosition = this.Parent.Position;
        }
    }
}
