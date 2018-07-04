using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scroller;
using ScrollerEngine;
using ScrollerEngine.Components;
using SDK_Application.Image_Processing;
using SDK_Application.Communication;
using SDK_Application.Error_Handling;
using SDK_Application.Controls;
using SDK_Application.Input;
using System.Reflection;
using Microsoft.Xna.Framework.Content;

namespace SDK_Application.Controls
{
    /// <summary>
    /// Worked on by Emmanuel, Peter, Richard, Mary, Jonathan
    /// This class makes a collection of Entities that contains the entity and the filename associated
    /// This is the class you deserialize
    /// </summary>
    public class EntityCollection
    {
        // Entity editor
        private Dictionary<String, Entity> EntityCollect; // A list of Entites and associated filename;
        private string xml_filename;

        /// <summary>
        /// Creator Method
        /// </summary>
        public EntityCollection()
        {
            EntityCollect = new Dictionary<String, Entity>();
        }

        /// <summary>
        /// An iterator to get the assemblies
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Assembly> GetAssemblies()
        {
            yield return Assembly.GetAssembly(typeof(ScrollerBase));
            yield return Assembly.GetAssembly(typeof(ScrollerGame));
        }

        /// <summary>
        /// Insert an Entity
        /// </summary>
        /// <returns>true if succesfull, false otherwise</returns>
        public bool insert(ref string fileName)
        {
            xml_filename = FileManagement.open_File(".xml");
            if (string.IsNullOrEmpty(xml_filename))
                return false;
            else if (EntityCollect.ContainsKey(xml_filename))
            {
                delete(xml_filename);
            }
            fileName = xml_filename;
            EntityCollect.Add(xml_filename, ScrollerSerializer.Deserialize(xml_filename));
            return true;
        }

        /// <summary>
        /// Delete an Entity
        /// </summary>
        /// <param name="filename">Index </param>
        /// <returns>true if sucessful, false otherwise</returns>
        public bool delete(string filename)
        {
            EntityCollect.Remove(filename);
            return true;
        }

        /// <summary>
        /// Gets the currently loaded in entity
        /// </summary>
        /// <param name="fileName">Index</param>
        /// <returns>true if successful, false otherwise</returns>
        public Entity getEntity(string fileName)
        {
            return EntityCollect[fileName];
        }

        /// <summary>
        /// Usually a User will work with one Entity... therefore get first Entity's filename
        /// </summary>
        /// <returns>Filename</returns>
        public string getFirstFileName()
        {
            return EntityCollect.First().Key;
        }

        /// <summary>
        /// gets all values associated with ComponentCollection cc
        /// </summary>
        /// <param name="cc">A list of values in a component to iterate through</param>
        public void getComponentValues(ComponentCollection cc)
        {
            foreach (Component compo in cc)
            {
                Type myType = compo.GetType();

                foreach (PropertyInfo prop in myType.GetProperties())
                {
                    if (!Attribute.IsDefined(prop, typeof(ContentSerializerIgnoreAttribute)))
                    {
                        object propValue = prop.GetValue(compo, null);
                    }
                    
                }
            }
        }

        public bool isEmpty()
        {
            return EntityCollect.Count == 0;
        }

        /// <summary>
        /// I dont think this function is used. It was only used for testing
        /// </summary>
        /// <param name="filename"></param>
        public void getPropertyValue(string filename)
        {
            Type myType = EntityCollect[filename].GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                if (!Attribute.IsDefined(prop, typeof(ContentSerializerIgnoreAttribute)))
                {
                    object propValue = prop.GetValue(EntityCollect[filename], null);
                    Console.Write(prop.Name + ": ");
                    Console.Write(propValue + "\n");

                    if (prop.PropertyType.Name.Equals("ComponentCollection"))
                    {
                        getComponentValues((ComponentCollection)propValue);
                    }
                    else
                    {
                        Console.WriteLine("No collection found");
                    }
                }
            }
        }

    }
}