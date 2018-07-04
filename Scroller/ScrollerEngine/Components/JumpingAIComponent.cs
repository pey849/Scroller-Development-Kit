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
    /// Worked on by Emmanuel, Peter, Richard
    /// A AI component to force a enemy to jump and move.
    /// </summary>
    public class JumpingAIComponent : AIComponent
    {
        private float _JumpDelay = 0f;
        private Vector2 _OldPosition;
        private bool _PreviousGroundedState = false;
        private float _JumpDelayTimer = 0f;

        /// <summary>
        /// Gets or sets the delay before jumping.
        /// </summary>
        public float JumpDelay
        {
            get { return _JumpDelay; }
            set { _JumpDelay = value; }
        }

        protected override void OnAiUpdate(GameTime Time)
        {
            //initial case. Move in direction of player.
            if (!MC.IsMoving)
                MC.BeginMove(this.Parent.GetDirectionWRTEntity(ScrollerBase.Instance.Players.First().Character));
            //hit a wall, reverse
            if (_OldPosition.X == this.Parent.Position.X)
                MC.BeginMove(MC.CurrentDirection.Reverse());

            if (!_PreviousGroundedState && PC.IsGrounded)
                _JumpDelayTimer = JumpDelay;

            if (_JumpDelayTimer > 0)
                _JumpDelayTimer -= Time.GetTimeScalar();
            else if (_JumpDelayTimer < 0)
            {
                _JumpDelayTimer = 0;
                MC.Jump(false);
            }

            _PreviousGroundedState = PC.IsGrounded;
            _OldPosition = this.Parent.Position;
        }
    }
}
