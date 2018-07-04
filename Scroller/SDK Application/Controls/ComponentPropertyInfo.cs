using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SDK_Application.Controls
{
    /// <summary>
    /// Worked on by Emmanuel, Peter, Richard, Mary, Jonathan
    /// A class to track a Property within a Component.
    /// </summary>
    public class ComponentPropertyInfo
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets the name without the 'Component' part at the end.
        /// </summary>
        public string ReadableName
        {
            get
            {
                var reg = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
                return reg.Replace(Name, " ");
            }
        }

        public override string ToString()
        {
            return string.Format("{0} (Type: {1})", Name, Type.Name);
        }

    }
}
