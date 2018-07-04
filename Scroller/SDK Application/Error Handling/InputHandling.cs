using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace SDK_Application.Error_Handling
{
    /// <summary>
    /// Takes an array of strings, checks if all of them have numbers in them
    /// </summary>
    class InputHandling
    {
        public static bool isNumeric(params TextBox[] Txt)
        {
            foreach (TextBox t in Txt)
            {
                Match m = Regex.Match(t.Text, @"\d*");
                if (!m.Success) 
                    return false; 
                else return true;
            }
            return false;
        }
    }
}
