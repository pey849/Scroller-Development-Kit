using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ScrollerEngine.Components
{
    /* 
     * This class allows entities to spawn more entities. Primarily will be used for projectiles.
     * Once an entity has been made and put into a scene, it will run on its own .
     */
    /// <summary>
    /// Worked on by Peter, Richard 
    /// </summary>
    public class NewEntityComponent : Component
    {
        protected int _EntityCount = 0;
        protected List<Entity> _Entities = new List<Entity>();

        /// <summary>
        /// Number of entities made so far
        /// </summary>
        public int EntityCount
        {
            get { return _EntityCount; }
        }

        /// <summary>
        /// The list of entities to add to the scene. As an example for projectiles,
        /// one entity can be created to represent a bullet, 
        /// or many entities can be created at once to represent shooting multiple bullets at once.
        /// </summary>
        [ContentSerializerIgnore]
        public List<Entity> Entities
        {
            get { return _Entities; }
            set { _Entities = value; }
        }

        /// <summary>
        /// Gets or sets the classification that the Entity needs to have for the collision to occur.
        /// The default is to affect all entities.
        /// Essenitially identical to the classification method in CollisionEventComponent
        /// </summary>
        public virtual void AddEntityToScene()
        {
            foreach (var e in _Entities)
            {
                Parent.Scene.AddEntity(e);
            }

            // Don't want to have reference values popping up in all the wrong places,
            // so might as well just get rid of them 

            _Entities.Clear();
        }

        /* Entities and their components are initialized here, then added to the entities List */
        public virtual void CreateEntity()
        {
            Entity e = new Entity();
            _Entities.Add(e);
        }

        /* Create x number of entities, all with the same properties */
        public virtual void CreateMultipleEntities(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateEntity();
            }
        }
    }
}
