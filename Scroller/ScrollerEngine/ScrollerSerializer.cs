using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using ScrollerEngine.Components;
using ScrollerEngine.Components.Graphics;

namespace ScrollerEngine
{
    /// <summary>
    /// Worked on by Peter, Richard, Jonathan
    /// Provides serialization and data (Entity) management.
    /// </summary>
    public static class ScrollerSerializer
    {
        private const string DATA_FOLDER = "Data/Entities/";

        private static XmlWriterSettings _WriterSettings;
        private static EntityBlueprintCollection _AllBlueprints;

        /// <summary>
        /// Serializes the given entity to the given file.
        /// The filename should be the relative file location. Example: "/Data/Entities/something.xml"
        /// Should only be used by the App thing.
        /// </summary>
        public static void Serialize(string fileName, Entity entity)
        {
            using (var writer = XmlWriter.Create(fileName, _WriterSettings))
                IntermediateSerializer.Serialize<Entity>(writer, entity, null);
        }

        /// <summary>
        /// Deserializes the given file and returns the entity.
        /// The filename should be the relative file location. Example: "/Data/Entities/something.xml"
        /// Should only be used by the App thing.
        /// </summary>
        public static Entity Deserialize(string fileName)
        {
            using (var reader = XmlReader.Create(fileName))
                return IntermediateSerializer.Deserialize<Entity>(reader, null);        
        }

        /// <summary>
        /// Creates an entity with the specified name. 
        /// Should only be used by the Game.
        /// </summary>
        public static Entity CreateEntity(string name)
        {
            try
            {
                var entity = Deserialize(DATA_FOLDER + name + ".xml");
                //Due to the fail that is the serializer, I will have to manually assign the parent entity here.
                entity.Components.Entity = entity;
                foreach (var component in entity.Components)
                    component.Parent = entity;
                return entity;
            }
            catch (Exception e1)
            {
                var message = string.Format("Entity: {0}\n\n{1}", DATA_FOLDER + name + ".xml", e1.Message);
                throw new Exception(message);
            }
            throw new InvalidOperationException("ScrollerSerializer.cs: This should never happen.");
        }

        /// <summary>
        /// Loads all the files located in the Data folder.
        /// This will incorparate changes made to the entities.
        /// Should only be used by the Game.
        /// </summary>
        public static void ReloadEntities()
        {
            _AllBlueprints = new EntityBlueprintCollection();
            //Reads all the files and loads them.
            foreach (var file in Directory.GetFiles(DATA_FOLDER, "*.xml"))
                _AllBlueprints.Add(file);
        }

        /// <summary>
        /// Initiation logic.
        /// </summary>
        static ScrollerSerializer()
        {
            _WriterSettings = new XmlWriterSettings();
            _WriterSettings.Indent = true;

            if (!Directory.Exists(DATA_FOLDER))
                Directory.CreateDirectory(DATA_FOLDER);
            //Temp REMOVE THIS IN FUTURE

            //CreateEntities();
        }

        /// <summary>
        ///// This is temp until we get the app working.
        /// </summary>
        private static void CreateEntities()
        {
            List<Entity> entitiesArray = new List<Entity>();
            //Create the entity.
            var playerEntity = new Entity();
            playerEntity.Name = "Player";               //In this context, this is the file name for the entity.
            playerEntity.Size = new Vector2(64f, 64f); //reminder: this is a sprite size, not the texture size
            playerEntity.Position = new Vector2(100f, 100f); //This will generally be set by the level. 

            //Add Components here
            var ps = new PhysicsComponent();
            ps.GravityCoefficient = 0.05f;
            ps.HorizontalDragCoefficient = 0.5f;
            ps.TerminalVelocity = 700f;
            playerEntity.Components.Add(ps);
            //Repeat process for every other component you want to add.

            var hc = new HealthComponent();
            hc.MaxHealth = 9000;
            playerEntity.Components.Add(hc);

            var cc = new ClassificationComponent();
            cc.Classification = EntityClassification.Player;
            playerEntity.Components.Add(cc);

            var mc = new MovementComponent();
            mc.MoveSpeed = 300f;
            mc.MoveAcceleration = 2000f;
            mc.JumpSpeed = 600f;
            playerEntity.Components.Add(mc);

            var sc = new SpriteComponent();
            sc.InitializeDefaultAnimations();
            sc.isMirrored = false;
            playerEntity.Components.Add(sc);

            var pcc = new PlayerControlComponent();
            playerEntity.Components.Add(pcc);
                
            //Then add it to this array.
            entitiesArray.Add(playerEntity);

            //Finished. Repeat for all other entities.

            //A TestEnemy.
            var testEnemy1 = new Entity();
            testEnemy1.Name = "TestEnemy";
            testEnemy1.Size = new Vector2(32,32);
                var pc1 = new PhysicsComponent();
                pc1.GravityCoefficient = 0.005f;
                pc1.HorizontalDragCoefficient = 0.5f;
                pc1.TerminalVelocity = 700f;
                testEnemy1.Components.Add(pc1);
                var sc1 = new SpriteComponent();
                sc1.TextureName = "Sprites/pizza";
                testEnemy1.Components.Add(sc1);
                var cc1 = new ClassificationComponent();
                cc1.Classification = EntityClassification.Enemy;
                testEnemy1.Components.Add(cc1);
                var dc1 = new DisposeComponent();
                dc1.Classification = EntityClassification.Player;
                dc1.DisposeOnCollision = true;
                testEnemy1.Components.Add(dc1);
            entitiesArray.Add(testEnemy1);
            //end TestEnemy

            //Serializes the entity.
            //Basically, this is to check that the components can be serialized.
            foreach (var e in entitiesArray)
                Serialize(DATA_FOLDER + e.Name + ".xml", e);

            //This is to confirm that their was nothing wrong during the serialization.
            //Usually, there can be issues when deserializing Components.
            //If it complains about not being able to deserialize, then it is most likely something with a component.
            //  If it's something about a circular reference, then add this above the property causing it:[ContentSerializer(SharedResource=true)] 
            //      Note: Use the above with caution. Actually, probably better to not do the above! NO, IT IS BETTER TO NOT DO THE ABOVE!
            //  If it's a property you dont want, add this: [ContentSerializerIgnore]
            //  NOTE: Also need to include: using Microsoft.Xna.Framework.Content
            foreach (var blueprint in Directory.GetFiles(DATA_FOLDER, "*.xml"))
            {
                var entity = Deserialize(blueprint);
            }
        }
        
        private class EntityBlueprintCollection : System.Collections.ObjectModel.KeyedCollection<string, string>
        {
            protected override string GetKeyForItem(string item)
            {
                string name = item.Split('/').Last().Split('.').First();
                return name;
            }
        }

    }
}
