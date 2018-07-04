using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollerEngine.Input
{
    /// <summary>
    /// Worked on by Peter, Richard, Emmanuel
    /// Indicates what input method is being used; either a keyboard, or a controller.
    /// </summary>
    public enum InputMethod : int
    {
        /// <summary>
        /// No input method has been set.
        /// </summary>
        None = 0,
        /// <summary>
        /// The input method is a keyboard.
        /// </summary>
        Key = 1,
        /// <summary>
        /// The input method is a controller.
        /// </summary>
        Button = 2,
    }
}
