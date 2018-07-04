using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDK_Application.Error_Handling
{
    /// <summary>
    /// Worked on by Peter
    /// </summary>
    class ImageHandling
    {
        public static bool ImgIsValid(string fileName)
        {
            string fileExtension = "";
            //get the file extension (last 4 characters)
            for (int i = fileName.Length - 4; i < fileName.Length; i++)
            {
                fileExtension += fileName[i];
            }

            //check the if the last 4 characters are the right extension
            if (fileExtension.Equals(".png"))
            {
                return true;
            }

            System.Windows.Forms.MessageBox.Show("File must be of .png extension.");
            return false;
        }

    }
}
