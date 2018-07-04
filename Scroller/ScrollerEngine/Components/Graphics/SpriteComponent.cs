using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ScrollerEngine.Graphics;

namespace ScrollerEngine.Components.Graphics
{
    /// <summary>
    /// Worked on by Mary, Richard
    /// Used to manage the sprites and animation for an Entity.
    /// </summary>
    public class SpriteComponent : Component
    {
        //NOTE: This is all temp; Someone will need to devise a Sprite/Animation method thing.
        //A better one then we have now.

        private Texture2D _pixelDebug;
        private Texture2D _Texture;
        private string _TextureName = "";
        private Color _Color = Color.White;
        public Dictionary<string, AnimationFrame> faAnimations = new Dictionary<string, AnimationFrame>();
        protected Vector2 _LastPosition;
        private string _CurrentAnimation = null;
        private int _Width = 1;
        private int _Height = 1;
        private bool isAnimating = true;
        public bool isMirrored = false;
        
        [ContentSerializerIgnore]
        public Direction currentFacingDirection = Direction.Right;

        private float _FlickerDuration = 0f;
        private DateTime _FlickerActivationTime;
        private bool _IsFlicking = false;
        private float _FlickerTimer;
        private const float FLICKER_RATE = 0.05f;

        /// <summary>
        /// Gets the texture file/sprite map.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return _Texture; }
            set { _Texture = value; }
        }

        /// <summary>
        /// Gets or sets the texture to load.
        /// </summary>
        public string TextureName
        {
            get { return _TextureName; }
            set { _TextureName = value; }
        }

        /// <summary>
        /// Gets or sets the Color of the sprite.
        /// </summary>
        public Color ColorTint
        {
            get { return _Color; }
            set { _Color = value; }
        }

        /// <summary>
        /// Return the width of a single animation frame.
        /// </summary>
        [ContentSerializerIgnore]
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        /// <summary>
        /// Return the height of a single animation frame.
        /// </summary>
        [ContentSerializerIgnore]
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        /// <summary>
        /// Return the bounding box surrounding the sprite frame.
        /// </summary>
        [ContentSerializerIgnore]
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)Parent.Position.X, (int)Parent.Position.Y, Width, Height); }
        }

        /// <summary>
        /// The AnimationFrame that is playing
        /// </summary>
        [ContentSerializerIgnore]
        public AnimationFrame CurrentAnimationFrame
        {
            get 
            { 
                if (!string.IsNullOrEmpty(_CurrentAnimation))
                    return faAnimations[_CurrentAnimation];
                else
                    return null;
            }
        }

        [ContentSerializerIgnore]
        public string CurrentAnimation
        {
            get { return _CurrentAnimation; }
            set
            {
                if (faAnimations.ContainsKey(value))
                {
                    _CurrentAnimation = value;
                    faAnimations[_CurrentAnimation].CurrentFrame = 0;
                    faAnimations[_CurrentAnimation].LoopCount = 0;
                }
            }
        }

        [ContentSerializerIgnore]
        public Rectangle HurtBox
        {
            get 
            { 
                Rectangle hurtbox = CurrentAnimationFrame.CurrentHurtBox;
                if (hurtbox != Rectangle.Empty)
                {
                    hurtbox.Y += (int)Parent.Position.Y;
                    hurtbox.X += (int)Parent.Position.X;
                }

                return hurtbox;
            }
        }

        /// <summary>
        /// Causes the entity to flicker and sets them immortal, if they have HealthComponent.
        /// </summary>
        public void Flicker(float duration)
        {
            _FlickerDuration = duration;
            _FlickerActivationTime = DateTime.Now;
            var hc = this.Parent.GetComponent<HealthComponent>();
            if (hc != null)
                hc.IsImmortal = true;
        }

        protected override void OnInitialize()
        {
            this._Texture = ScrollerBase.Instance.GlobalContent.LoadTexture2D(_TextureName);
            this._pixelDebug = ScrollerBase.Instance.GlobalContent.LoadTexture2D("Sprites/pixel");
            
            _Width = 64;
            _Height = 64;

            if(!faAnimations.ContainsKey("spritemap"))
                AddAnimation("spritemap", 0, 0, _Texture.Width, _Texture.Height, 0.2f, 1, 1);
            CurrentAnimation = "spritemap";
            //GetAnimationWithName("spritemap").IntializeAutomaticHurtBoxes(_Texture);

            base.OnInitialize();
        }

        public void InitializeDefaultAnimations()
        {
            TextureName = "Sprites/platformer_sprites_pixelized_mirrored";
            Texture2D tempTexture = ScrollerBase.Instance.GlobalContent.LoadTexture2D(_TextureName);
            _Width = 64;
            _Height = 64;
            int mapWidth = tempTexture.Width;
            AddAnimation("walk_right", 256, 0, _Width, _Height, 0.08f, 8, mapWidth);
            AddAnimation("stand_right", 0, 0, _Width, _Height, 0.1f, 4, mapWidth);
            AddAnimation("crouch_right", 64, 320, _Width, _Height, 0.1f, 1, mapWidth);
            AddAnimation("jump_up_right", 256, 320, _Width, _Height, 0.1f, 1, mapWidth);
            AddAnimation("jump_down_right", 448, 320, _Width, _Height, 0.1f, 1, mapWidth);

            AddAnimation("walk_left", 256, 0 + 576, _Width, _Height, 0.08f, 8, 512);
            AddAnimation("stand_left", 0, 0 + 576, _Width, _Height, 0.1f, 4, 512);
            AddAnimation("crouch_left", 64, 320 + 576, _Width, _Height, 0.1f, 1, 512);
            AddAnimation("jump_up_left", 256, 320 + 576, _Width, _Height, 0.1f, 1, 512);
            AddAnimation("jump_down_left", 448, 320 + 576, _Width, _Height, 0.1f, 1, 512);

            CurrentAnimation = "stand_right";

            GetAnimationWithName("stand_right").IntializeAutomaticHurtBoxes(tempTexture);
            GetAnimationWithName("walk_right").IntializeAutomaticHurtBoxes(tempTexture);
            GetAnimationWithName("crouch_right").IntializeAutomaticHurtBoxes(tempTexture);
            GetAnimationWithName("jump_up_right").IntializeAutomaticHurtBoxes(tempTexture);
            GetAnimationWithName("jump_down_right").IntializeAutomaticHurtBoxes(tempTexture);
            GetAnimationWithName("stand_left").IntializeAutomaticHurtBoxes(tempTexture);
            GetAnimationWithName("walk_left").IntializeAutomaticHurtBoxes(tempTexture);
            GetAnimationWithName("crouch_left").IntializeAutomaticHurtBoxes(tempTexture);
            GetAnimationWithName("jump_up_left").IntializeAutomaticHurtBoxes(tempTexture);
            GetAnimationWithName("jump_down_left").IntializeAutomaticHurtBoxes(tempTexture);
        }

        public void AddAnimation(string name, int X, int Y, int width, int height, float frameLength, int frameCount, int spriteMapWidth)
        {
            if (faAnimations.ContainsKey(name))
                return;

            faAnimations.Add(name, new AnimationFrame(X, Y, width, height, frameCount, spriteMapWidth, frameLength));
        }

        public void AddAnimation(string name, int X, int Y, int width, int height, float frameLength, string nextAnim, int frameCount, int spriteMapWidth)
        {
            if (faAnimations.ContainsKey(name))
                return;

            faAnimations.Add(name, new AnimationFrame(X, Y, width, height, frameCount, spriteMapWidth, frameLength, nextAnim));
        }

        public void RemoveAnimationWithName(string name)
        {
            if (faAnimations.ContainsKey(name))
            {
                faAnimations.Remove(name);
            }
        }

        protected AnimationFrame GetAnimationWithName(string name)
        {
            if (faAnimations.ContainsKey(name))
                return faAnimations[name];
            else
                return null;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (isAnimating)
            {
                if (CurrentAnimationFrame == null)
                {
                    if (faAnimations.Count > 0)
                    {
                        string[] keys = new string[faAnimations.Count];
                        faAnimations.Keys.CopyTo(keys, 0);
                        CurrentAnimation = keys[0];
                    }
                    else
                        return;
                }

                CurrentAnimationFrame.Update(gameTime);

                if (!string.IsNullOrEmpty(CurrentAnimationFrame.NextAnimation))
                {
                    if (CurrentAnimationFrame.LoopCount > 0)
                        CurrentAnimation = CurrentAnimationFrame.NextAnimation;
                }
            }

            var pcc = Parent.GetComponent<PlayerControlComponent>();
            if (pcc != null)
            {
                if (pcc.prevDirection == Direction.Right)
                    currentFacingDirection = Direction.Right;
                else if (pcc.prevDirection == Direction.Left)
                    currentFacingDirection = Direction.Left;
            }
            else
            {
                var pc = Parent.GetComponent<PhysicsComponent>();
                if (pc != null)
                {
                    if (pc.VelocityX > 0)
                        currentFacingDirection = Direction.Right;
                    else if (pc.VelocityX < 0)
                        currentFacingDirection = Direction.Left;
                }
            }

            base.OnUpdate(gameTime);

            //Creates that flicking effect.
            if (_FlickerDuration <= 0)
                _IsFlicking = false;
            else
            {
                if ((DateTime.Now - _FlickerActivationTime).TotalSeconds > _FlickerDuration)
                {
                    _FlickerDuration = 0f;
                    var hc = this.Parent.GetComponent<HealthComponent>();
                    if (hc != null)
                        hc.IsImmortal = false;
                }
                else
                {
                    _FlickerTimer += gameTime.GetTimeScalar();
                    if (_FlickerTimer > FLICKER_RATE)
                    {
                        _IsFlicking = !_IsFlicking;
                        _FlickerTimer = 0f;
                    }
                }
            }
        }

        protected override void OnDraw()
        {
            if (_IsFlicking)
                return;
            base.OnDraw();

            var spriteBatch = ScrollerBase.Instance.SpriteBatch;
            var Parent = this.Parent;
            SpriteEffects test = new SpriteEffects();

            /* Recommend that sprite map simply have the left AND right animations, instead of utilising this mirroring function */
            if (currentFacingDirection == Direction.Left && isMirrored)
                test = SpriteEffects.FlipHorizontally;

            //Uncomment the line below to view dynamic hurtboxes.
            //spriteBatch.Draw(_pixelDebug, CameraManager.Active.WorldToScreen(HurtBox), Color.Blue);

            //Debugger.DrawContent("Hurtbox", HurtBox);
            var screenPos = CameraManager.Active.WorldToScreen(Parent.Position);
            var positionRect = new Rectangle((int)screenPos.X, (int)screenPos.Y, (int)Parent.Size.X, (int)Parent.Size.Y); // so that the sprites scale with size.
            spriteBatch.Draw(Texture,
                positionRect,
                CurrentAnimationFrame.CurrentFrameRectangle,
                ColorTint,
                0.0f, 
                Vector2.Zero,
                test,
                0.0f);
        }

    }
}
