using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace SDK_Application.Controls
{
    /// <summary>
    /// Worked on by Richard, Mary, Emmanuel, Jonathan, Peter 
    /// This class needs to do conversion via reflection
    /// </summary>
    class Casting
    {
        /// <summary>
        /// Converts Textbox to color
        /// </summary>
        /// <param name="tx"></param>
        /// <returns></returns>
        public static Color FromTextToColor(TextBox tx)
        {
            // {R:255 G:255 B:255 A:255}
            Match m = Regex.Match(tx.Text,
            @"(?i:\{)R:(?<Red>\d{1,3}) G:(?<Green>\d{1,3}) B:(?<Blue>\d{1,3}) A:(?<Alpha>\d{1,3})\}?");
            if (m.Success)
            {
                return new Color
                    (
                    int.Parse(m.Groups["Red"].Value),
                    int.Parse(m.Groups["Green"].Value),
                    int.Parse(m.Groups["Blue"].Value),
                    int.Parse(m.Groups["Alpha"].Value)
                    );
            }
            else
            {
                Error_Handling.MessageBoxes.Alert_PopUP("STOP IT! (FromTextToColor Failure)");
                return Color.Black;
            }
        }

        public static Vector2 FromTextToVector(TextBox tx)
        {
            // {X:255 Y:255}
            Match m = Regex.Match(tx.Text,
            @"(?i:\{)X:(?<X>\d{1,20}) Y:(?<Y>\d{1,20})\}?");
            if (m.Success)
            {
                return new Vector2
                    (
                    float.Parse(m.Groups["X"].Value),
                    float.Parse(m.Groups["Y"].Value)
                    );
            }
            else
            {
                Error_Handling.MessageBoxes.Alert_PopUP("STOP IT! (FromTextToVector Failure)");
                return Vector2.Zero;
            }
        }

    }


}
