using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ScrollerEngine.Scenes
{
    /// <summary>
    /// Worked on by Peter, Richard, Emmanuel
    /// Contains the properties for this level.
    /// </summary>
    public class LevelProperty
    {
        /// <summary>
        /// The name of this property.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The value of this property.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Creates a new MapProperty.
        /// </summary>
        public LevelProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    /// A collection for managing level properties.
    /// </summary>
    public class LevelPropertyCollection : KeyedCollection<string, LevelProperty>
    {
        protected override string GetKeyForItem(LevelProperty item)
        {
            return item.Name;
        }
    }
}
