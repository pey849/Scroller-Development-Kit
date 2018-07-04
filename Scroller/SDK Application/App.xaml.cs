using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Reflection;
using SDK_Application.Controls;
using ScrollerEngine.Components;
using Microsoft.Xna.Framework.Content;

namespace SDK_Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ObservableCollection<ComponentInfo> _ComponentCollection = new ObservableCollection<ComponentInfo>();

        public ObservableCollection<ComponentInfo> ComponentCollection { get { return _ComponentCollection; } }
        
        private IEnumerable<Assembly> GetAssemblies()
        {
            yield return Assembly.GetAssembly(typeof(ScrollerEngine.ScrollerBase));
            yield return Assembly.GetAssembly(typeof(Scroller.ScrollerGame));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var components = new List<ComponentInfo>();
            foreach (var assembly in GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(Component)) && !type.IsAbstract)
                    {
                        ComponentInfo ci = new ComponentInfo();
                        ci.Name = type.Name;
                        ci.NameSpace = type.Namespace;
                        ci.Type = type;
                        //Get all properties
                        var comp = Activator.CreateInstance(type);
                        foreach (var property in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
                        {
                            //If it contains this attributes, ignore it.
                            if (!property.CanWrite || Attribute.IsDefined(property, typeof(ContentSerializerIgnoreAttribute)))
                                continue;
                            var cpi = new ComponentPropertyInfo();
                            cpi.Name = property.Name;
                            cpi.Type = property.PropertyType;
                            cpi.DefaultValue = property.GetValue(comp, null);
                            ci.Properties.Add(cpi);
                        }
                        components.Add(ci);
                    }
                }
            }

            foreach (var c in components.OrderBy(c => c.Name))
                _ComponentCollection.Add(c);

        }

    }
}
