using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ScrollerEngine;
using ScrollerEngine.Components.Graphics;


namespace SDK_Application.Controls
{
    public static class HitboxAnalyzer
    {
        public static void IntializeAutomaticHurtBoxes(Bitmap spriteMap, AnimationFrame aFrame)
        {
            Color[,] imageData2D = new Color[aFrame.FrameWidth, aFrame.FrameHeight];

            for (int loopIndex = 0; loopIndex < aFrame.FrameCount; loopIndex++)
            {
                // Get the Rectangle that represents the single frame of animation being played.
                Microsoft.Xna.Framework.Rectangle frame = aFrame.IndexedFrameRectangle(loopIndex);

                // Create an automatic bounding box around the frame of single animation
                // The bounding box is made as small as possible without overlapping any pixels
                // It will ignore transparent pixels.

                // Intialise the variables that will be used to make the Rectangle.
                int x = frame.Width;
                int y = frame.Height;
                int width = 1;
                int height = 1;


                // Fill in a 2D array of colors, it'll make it easier to map coordinates to each pixel.
                // This array represents one FRAME of animation, not the entire animation or the sprite map.
                for (int i = 0; i < frame.Width; i++)
                    for (int j = 0; j < frame.Height; j++)
                        imageData2D[i, j] = spriteMap.GetPixel(frame.Left + i, frame.Top + j);

                bool breakOuterLoop = false;

                // Scan a column of pixels. If all pixels are transparent, move one pixel right and try again.
                // Find the left-most non-transparent pixel.
                for (int xIndex = 0; xIndex < frame.Width; xIndex++)
                {
                    for (int yIndex = 0; yIndex < frame.Height; yIndex++)
                    {
                        if (imageData2D[xIndex, yIndex].A != 0)
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

                // Scan a row of pixels. If a color pixel is not found, move a pixel down and try again.
                // Find the topmost non-transparent pixel
                for (int yIndex = 0; yIndex < frame.Height; yIndex++)
                {
                    for (int xIndex = 0; xIndex < frame.Width; xIndex++)
                    {
                        if (imageData2D[xIndex, yIndex].A != 0)
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
                for (int yIndex = frame.Height - 1; yIndex > 0; yIndex--)
                {
                    for (int xIndex = frame.Width - 1; xIndex > 0; xIndex--)
                    {
                        if (imageData2D[xIndex, yIndex].A != 0)
                        {
                            height = frame.Height - y - (frame.Height - yIndex);
                            breakOuterLoop = true;
                            break;
                        }
                    }

                    if (breakOuterLoop)
                        break;
                }

                breakOuterLoop = false;

                // scan from bottom to top, starting at the right. Find the bottom-most pixel
                for (int xIndex = frame.Width - 1; xIndex > 0; xIndex--)
                {
                    for (int yIndex = frame.Height - 1; yIndex > 0; yIndex--)
                    {
                        if (imageData2D[xIndex, yIndex].A != 0)
                        {
                            width = frame.Width - x - (frame.Width - xIndex);
                            breakOuterLoop = true;
                            break;
                        }
                    }

                    if (breakOuterLoop)
                        break;
                }

                Microsoft.Xna.Framework.Rectangle Hurtbox = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
                aFrame._CollisionHurtboxes.Add(Hurtbox);
            }
        }

    }
}
