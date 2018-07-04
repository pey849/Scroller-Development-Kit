using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ScrollerEngine.Engine
{

    /// <summary>
    /// Worked on by Richard
    /// A Keyed collection for managing game states.
    /// </summary>
    public class GameStateCollection : KeyedCollection<Type, GameState>
    {
        protected override Type GetKeyForItem(GameState item)
        {
            return item.GetType();
        }
    }
}
