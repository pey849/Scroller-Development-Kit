using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SDK_Application.Controls
{
    /// <summary>
    /// Worked on by Emmanuel, Peter, Richard, Mary, Jonathan
    /// A class to track data for Components.
    /// </summary>
    public class ComponentInfo
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public Type Type { get; set; }
        public List<ComponentPropertyInfo> Properties { get; set; }

        /// <summary>
        /// Gets the Name + Namespace.
        /// </summary>
        public string FullName { get { return string.Format("{0}.{1}", NameSpace, Name); } }

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
                return reg.Replace(Name, " ").Replace("Component", "").Trim();
            }
        }

        public ComponentInfo()
        {
            Properties = new List<ComponentPropertyInfo>();
        }

        public override string ToString()
        {
            return string.Format("{0} (Properties: {1})", Name, Properties.Count);
        }
    }
}
