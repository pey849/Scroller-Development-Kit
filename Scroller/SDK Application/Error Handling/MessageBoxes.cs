using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SDK_Application.Error_Handling
{
    /// <summary>
    /// Worked on by Mary
    class MessageBoxes
    {
        /**
    An Error Pop-up Message box to display passed in texts
*/
        public static void Alert_PopUP(string message)
        {
            System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
