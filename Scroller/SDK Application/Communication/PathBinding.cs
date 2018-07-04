using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDK_Application.Communication
{
    /// <summary>
    /// Worked on by Emmanuel
    /// </summary>
    class PathBinding
    {
        public static string XML_Path_Location
        {
            // Goes back a couple directories to where the main project should be located.
            // This 
            get { return "../../../Scroller/Scroller/bin/x86/Debug"; }
        }
    }
}
