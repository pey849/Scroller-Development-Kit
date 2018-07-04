using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollerEngine.Components
{
    /// <summary>
    /// Worked on by Richard, Jonathan
    /// Provides a component that classifies an Entity
    /// </summary>
    public class ClassificationComponent : Component
    {
        private EntityClassification _Classification = EntityClassification.Unknown;

        /// <summary>
        /// Gets or sets the classification of this Entity.
        /// </summary>
        public EntityClassification Classification
        {
            get { return _Classification; }
            set { _Classification = value; }
        }
    }
}
