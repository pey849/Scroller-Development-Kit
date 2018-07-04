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
    /// Used to manage a single row animation from a sprite map.
    /// Basically a bunch of glorified Rectangles.
    /// </summary>
    public class AnimationFrame : ICloneable
    {
        public Rectangle _RectFrame;
        private int _SpriteMapWidth;
        private int _LoopCount = 0;
        private int _FrameCount = 1;
        private int _CurrentFrame = 0;
        private float _FrameLength = 0.2f;
        private float _FrameTimer = 0.0f;
        private string _NextAnimation = null;

        /* 
         * Hitboxes are the collisions that hit other entities
         * Hurtboxes are the collisions that are hit by other entities
         */ 
        public List<Rectangle> _CollisionHurtboxes = new List<Rectangle>();
        public List<Rectangle> _CollisionHitboxes = new List<Rectangle>();

        /// <summary>
        /// Returns the width of the spritemap. 
        /// Used for calculating which animation to play,
        /// especially if they are spread across multiple rows.
        /// </summary>
        public int SpriteMapWidth
        {
            get { return _SpriteMapWidth; }
            set { _SpriteMapWidth = value; }
        }

        /// <summary>
        /// A counter that keeps track of how many times the animation has looped
        /// </summary>
        public int LoopCount
        {
            get { return _LoopCount; }
            set { _LoopCount = value; }
        }

        /// <summary>
        /// The number of frames the animation will last.
        /// </summary>
        public int FrameCount
        {
            get { return (int)MathHelper.Clamp(_FrameCount, 1, _FrameCount); }
            set { _FrameCount = value; }
        }

        /// <summary>
        /// The index of the frame being current played, starting from 0.
        /// </summary>
        public int CurrentFrame
        {
            get { return _CurrentFrame; }
            set { _CurrentFrame= (int)MathHelper.Clamp(value, 0, _FrameCount - 1); }
        }

        /// <summary>
        /// How long each frame will last in seconds.
        /// </summary>
        public float FrameLength
        {
            get { return _FrameLength; }
            set { _FrameLength = value; }
        }

        /// <summary>
        /// A counter that increments based on time passed.
        /// </summary>
        public float FrameTimer
        {
            get { return _FrameTimer; }
            set { _FrameTimer = value; }
        }
        
        /// <summary>
        /// The name of the next animation to play.
        /// Examples would be if the current animation is "Attack1", the next animation would be "idle" or "Attack2"
        /// </summary>
        public String NextAnimation
        {
            get { return _NextAnimation; }
            set { _NextAnimation = value; }
        }

        /// <summary>
        /// Returns the width of the frame. Cannot be altered.
        /// </summary>
        public int FrameWidth
        {
            get { return _RectFrame.Width; }
        }

        /// <summary>
        /// Same as FrameWidth, but for height. 
        /// Reason it cannot be altered is that all frames in one animation are considered to be the same size.
        /// The hitboxes do not have to match 1:1 with the frame size
        /// </summary>
        public int FrameHeight
        {
            get { return _RectFrame.Height; }
        }

        /// <summary>
        /// Return the rectangle of the current frame being played
        /// </summary>
        public Rectangle CurrentFrameRectangle
        {
            get
            {
                int lengthX = _RectFrame.X + (_RectFrame.Width * _CurrentFrame);
                int x = lengthX % _SpriteMapWidth; // hardcoded for now, width of sprite map
                int y = lengthX / _SpriteMapWidth;

                return new Rectangle(x,
                    _RectFrame.Y + (y * _RectFrame.Height),
                    _RectFrame.Width,
                    _RectFrame.Height);
            }
        }

        /// <summary>
        /// Return the rectangle of a specific frame
        /// </summary>
        public Rectangle IndexedFrameRectangle(int frameIndex)
        {
            int lengthX = _RectFrame.X + (_RectFrame.Width * frameIndex);
            int x = lengthX % _SpriteMapWidth; // hardcoded for now, width of sprite map
            int y = lengthX / _SpriteMapWidth;

            return new Rectangle(x,
                _RectFrame.Y + (y * _RectFrame.Height),
                _RectFrame.Width,
                _RectFrame.Height);
        }

        /// <summary>
        /// Return the Rectangle of the current Hurtbox
        /// </summary>
        public Rectangle CurrentHurtBox
        {
            get 
            {
                if (_CollisionHurtboxes.Count == 0)
                    return new Rectangle();
 
                return _CollisionHurtboxes.ElementAt(CurrentFrame); 
            }
        }

        /// <summary>
        /// Constructor 0 for an animation. The other constructors have more options
        /// </summary>
        public AnimationFrame()
        {
            _RectFrame = new Rectangle(0, 0, 64, 64);
            _FrameCount = 1;
            _SpriteMapWidth = 32;
        }

        /// <summary>
        /// Constructor 1 for an animation. The other constructors have more options
        /// </summary>
        public AnimationFrame(Rectangle frame, int frameCount, int spriteMapWidth)
        {
            _RectFrame = frame;
            _FrameCount = frameCount;
            _SpriteMapWidth = spriteMapWidth;
        }

        /// <summary>
        /// Constructor 2 for an animation
        /// </summary>
        public AnimationFrame(int x, int y, int width, int height, int frameCount, int spriteMapWidth)
        {
            _RectFrame = new Rectangle(x, y, width, height);
            _FrameCount = frameCount;
            _SpriteMapWidth = spriteMapWidth;
        }

        /// <summary>
        /// Constructor 3 for an animation
        /// </summary>
        public AnimationFrame(int x, int y, int width, int height, int frameCount, int spriteMapWidth, float frameLength)
        {
            _RectFrame = new Rectangle(x, y, width, height);
            _FrameCount = frameCount;
            _SpriteMapWidth = spriteMapWidth;
            _FrameLength = frameLength;
            
        }

        /// <summary>
        /// Constructor 4 for an animation
        /// </summary>
        public AnimationFrame(int x, int y, int width, int height, int frameCount, int spriteMapWidth, float frameLength, string nextAnim)
        {
            _RectFrame = new Rectangle(x, y, width, height);
            _FrameCount = frameCount;
            _SpriteMapWidth = spriteMapWidth;
            _FrameLength = frameLength;
            _NextAnimation = nextAnim;
        }

        public void IntializeAutomaticHurtBoxes(Texture2D spriteMap)
        {
            Color[] imageData = new Color[FrameWidth * FrameHeight];
            Color[,] imageData2D = new Color[FrameWidth, FrameHeight];

            for (int loopIndex = 0; loopIndex < _FrameCount; loopIndex++)
            {
                Rectangle frame = IndexedFrameRectangle(loopIndex);

                spriteMap.GetData(0, frame, imageData, 
                    0, imageData.Length);

                // Create an automatic bounding box around the frame of single animation
                // The bounding box is made as small as possible without overlapping any pixels
                // It will ignore transparent pixels.

                // Get x, y, width, and height
                int x = frame.Width;
                int y = frame.Height;
                int width = 1;
                int height = 1;

                for (int i = 0; i < FrameWidth; i++)
                    for (int j = 0; j < FrameHeight; j++)
                        imageData2D[i, j] = imageData[i + j * FrameWidth];

                bool breakOuterLoop = false;

                // scan from left to right, starting at the top. Find the leftmost pixel
                for (int xIndex = 0; xIndex < FrameWidth; xIndex++)
                {
                    for (int yIndex = 0; yIndex < FrameHeight; yIndex++)
                    {
                        if (imageData2D[xIndex, yIndex] != Color.Transparent)
                        {
                            x = xIndex;
                            breakOuterLoop = true;
                            break;
                        }
                    }

                    if (breakOuterLoop)
                        break;
                }

                breakOuterLoop = false;

                // scan from top to down, starting at the left. Find the topmost pixel
                for (int yIndex = 0; yIndex < FrameHeight; yIndex++)
                {
                    for (int xIndex = 0; xIndex < FrameWidth; xIndex++)
                    {
                        if (imageData2D[xIndex, yIndex] != Color.Transparent)
                        {
                            y = yIndex;
                            breakOuterLoop = true;
                            break;
                        }
                    }

                    if (breakOuterLoop)
                        break;
                }

                breakOuterLoop = false;

                // scan from bottom to top, starting at the right. Find the bottom-most pixel
                for (int yIndex = FrameHeight - 1; yIndex > 0; yIndex--)
                {
                    for (int xIndex = FrameWidth - 1; xIndex > 0; xIndex--)
                    {
                        if (imageData2D[xIndex, yIndex] != Color.Transparent)
                        {
                            height = FrameHeight - y - (FrameHeight - yIndex);
                            breakOuterLoop = true;
                            break;
                        }
                    }

                    if (breakOuterLoop)
                        break;
                }

                breakOuterLoop = false;

                // scan from bottom to top, starting at the right. Find the bottom-most pixel
                for (int xIndex = FrameWidth - 1; xIndex > 0; xIndex--)
                {
                    for (int yIndex = FrameHeight - 1; yIndex > 0; yIndex--)
                    {
                        if (imageData2D[xIndex, yIndex] != Color.Transparent)
                        {
                            width = FrameWidth - x - (FrameWidth - xIndex);
                            breakOuterLoop = true;
                            break;
                        }
                    }

                    if (breakOuterLoop)
                        break;
                }

                Rectangle Hurtbox = new Rectangle(x, y, width, height);
                _CollisionHurtboxes.Add(Hurtbox);
            }
        }

        public void Update(GameTime gametime)
        {
            _FrameTimer += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (_FrameTimer > _FrameLength)
            {
                _CurrentFrame = (_CurrentFrame + 1) % FrameCount;
                if (_CurrentFrame == 0)
                    _LoopCount = (int)MathHelper.Min(_LoopCount + 1, int.MaxValue);

                _FrameTimer = 0.0f;
            }
        }

        public object Clone()
        {
            return new AnimationFrame(this._RectFrame.X, this._RectFrame.Y,
                                      this._RectFrame.Width, this._RectFrame.Height,
                                      this._FrameCount, this._SpriteMapWidth, 
                                      this._FrameLength, this._NextAnimation);
        }

    }
}
