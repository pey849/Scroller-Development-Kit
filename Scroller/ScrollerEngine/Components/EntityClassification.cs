﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Emmanuel, Peter, Richard
    /// Indicates the classification of an Entity.
    /// This can have multiple values, such as Enemy and Projectile, or Player and Trigger.
    /// </summary>
    [Flags]
    public enum EntityClassification
    {
        /// <summary>
        /// No classification is available for this Entity.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Indicates a human player.
        /// </summary>
        Player = 1,
        /// <summary>
        /// Indicates an enemy NPC.
        /// </summary>
        Enemy = 2,
        /// <summary>
        /// Indicates a powerup, usually associated with Player.
        /// </summary>
        Powerup = 4, 
        /// <summary>
        /// Indicates a trigger which can be associated with Player or Enemy.
        /// </summary>
        Trigger = 8,
        /// <summary>
        /// Indicates a bullet or projectile, usually associated with Player or Enemy.
        /// </summary>
        Projectile = 16,
        /// <summary>
        /// Indicates that this classification matches any Entity.
        /// </summary>
        Any = Player | Enemy | Powerup | Trigger | Projectile 
    }
}
