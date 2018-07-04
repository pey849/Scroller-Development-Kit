using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using SDK_Application.Error_Handling;

namespace SDK_Application.Image_Processing
{
    /// <summary>
    /// Worked on by Emmanuel, Peter, Mary
    /// </summary>
    class SpritePreviewer
    {
        Bitmap _img;
        int _Instance_Width;
        int _Instance_Height;
        int _numberOfInstances;
        //List<Bitmap> Image_List;
        //List<BitmapSource> Preview_List;

        public SpritePreviewer(Bitmap Img, int Instance_Width, int Instance_Height, int n)
        {
            _img = Img;
            _Instance_Height = Instance_Height;
            _Instance_Width = Instance_Width;
            _numberOfInstances = n;
        }

        void AddSequence(int row, int column, string Seq_name)
        {
            //TODO if seq name exists, add it to the sequence pattern to traverse through 
            // Otherwise make another seq named the seq_name. 
        }

        void RemoveSequence(int row, int column, string Seq_name)
        {
            // check if exists
            // if exists, remove
            // else return error
        }

        /// <summary>
        /// Get the Rectangle of the next frame to play, and draw it in the port.
        /// </summary>
        /// <param name="currentAnimIndex"></param>
        /// <param name="imageYIndexTextBox"></param>
        /// <param name="imageXIndexTextBox"></param>
        /// <param name="numberOfFrames"></param>
        /// <param name="SpritesPerRowTextBox"></param>
        /// <param name="FileNameTextBox"></param>
        /// <param name="SpritesPerColumnTextBox"></param>
        /// <param name="Test_Img"></param>
        /// <returns></returns>
        public static int PlayNextFrame(int currentAnimIndex, TextBox imageYIndexTextBox, 
            TextBox imageXIndexTextBox, TextBox numberOfFrames,
            TextBox SpritesPerRowTextBox, TextBox FileNameTextBox,
            TextBox SpritesPerColumnTextBox, System.Windows.Controls.Image Test_Img)
        {
            currentAnimIndex++;

            try
            {
                System.Drawing.Image imgsrc = System.Drawing.Image.FromFile(FileNameTextBox.Text);
                int Yindex = Convert.ToInt16(imageYIndexTextBox.Text);
                int Xindex = Convert.ToInt16(imageXIndexTextBox.Text);
                int spriteHeight = imgsrc.Height / (Convert.ToInt16(SpritesPerColumnTextBox.Text));
                int spriteWidth = imgsrc.Width / (Convert.ToInt16(SpritesPerRowTextBox.Text));
                int totalFrames = Convert.ToInt16(numberOfFrames.Text);

                if (currentAnimIndex >= totalFrames)
                {
                    currentAnimIndex = 0;
                }

                int animationWidth = (Xindex + currentAnimIndex) * spriteWidth;
                int X = animationWidth % imgsrc.Width;
                int Y = (Yindex + (animationWidth / imgsrc.Width)) * spriteHeight;

                Test_Img.Source = SpritePreviewer.ImgViewAtN(imgsrc, X, Y, spriteWidth, spriteHeight);
                if (Test_Img.Source == null)
                {
                    //calling a method to throw a pop up to alert user that 
                    //their sprite specification is not right
                    MessageBoxes.Alert_PopUP("Image Specification Incorrect."
                        + "\n" + "Please check image requirements."
                        + "\n" + "(Sprite per row * Sprite width) = Image Height"
                        + "\n" + "(Sprite per column  * Sprite width) = Image Width");
                }
            }
            catch { }
            return currentAnimIndex;
        }

        public static Rectangle GetViewPort(int Instance_Width, int Instance_height, int row_index, int column_index)
        {
            return new Rectangle();
        }

        public static BitmapSource ImgViewAtN(System.Drawing.Image imgsrc, int X, int Y, int spriteWidth, int spriteHeight)
        {
                Bitmap imgdst = new Bitmap(spriteWidth, spriteHeight);
                // Bitmap drawing_surface = new Bitmap(500, 500);
                Graphics GFX = Graphics.FromImage(imgdst);
                GFX.DrawImage(imgsrc, new System.Drawing.RectangleF(0, 0, imgdst.Width, imgdst.Height),
                    new System.Drawing.RectangleF(X, Y, spriteWidth, spriteHeight),
                    GraphicsUnit.Pixel);
                // Create Image Element
                //System.Windows.Controls.Image myImage = new System.Windows.Controls.Image();
                //myImage.Width = 200;

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    imgdst.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(imgdst.Width, imgdst.Height));
                return bs;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public static BitmapImage ReturnBitmapFile(String fileName)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(fileName, UriKind.RelativeOrAbsolute);
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.CreateOptions = BitmapCreateOptions.None;
            img.EndInit();

            return img;
        }

       
    }
}
