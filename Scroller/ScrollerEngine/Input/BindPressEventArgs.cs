using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollerEngine.Input
{
    /// <summary>
    /// Worked on by Jonathan, Richard, Emmanuel
    /// Provides an EventArgs value for when a bind is invoked, optionally preventing the action from being invoked.
    /// </summary>
    public class BindPressEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates whether this event has been handled, preventing the mapped action from being invoked.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets the action that should be invoked for this press.
        /// </summary>
        public Action<BindState> Action { get; set; }

        /// <summary>
        /// Gets the InputManager that triggered this bind.
        /// </summary>
        public InputManager InputManager { get; set; }
    }

}
