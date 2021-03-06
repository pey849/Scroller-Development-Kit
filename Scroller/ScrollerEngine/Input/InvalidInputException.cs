﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollerEngine.Input
{
    /// <summary>
    /// Worked on by Richard
    /// Provides an exception thrown when an invalid input is used.
    /// </summary>
    public class InvalidInputException : Exception
    {
        /// <summary>
        /// Creates a new InvalidInputException with the given message.
        /// </summary>
        public InvalidInputException(string message)
            : base(message)
        {

        }
    }
}
