using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Scroller;
using ScrollerEngine;
using System.Reflection;
using System.Windows;
using ScrollerEngine.Components;
using Microsoft.Xna.Framework.Content;
using SDK_Application.Controls;

namespace SDK_Application.Input
{
    /// <summary>
    /// Worked on by Emmanuel, Peter, Richard
    /// Custom Grid Maker (Reflection Style) for new Tabs. (This should be used everytime you make a tabItem)
    /// Or something similar.
    /// </summary>
    public class GridContentReflection : Grid
    {
        public List<PropertyControls> ControlList = new List<PropertyControls>(1); // T
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
        /// Old Version. Do not use.
        /// </summary>
        /// <param name="componentName">the component name</param>
        public GridContentReflection(string componentName)
        {
            //ShowGridLines = true;
            ColumnDefinition cdlabel = new ColumnDefinition();
            ColumnDefinition cdtext = new ColumnDefinition();

            cdlabel.Width = new GridLength(1, GridUnitType.Star);
            cdtext.Width = new GridLength(1, GridUnitType.Star);

            this.ColumnDefinitions.Add(cdlabel);
            this.ColumnDefinitions.Add(cdtext);

            getProperties(componentName);
        }

        /// <summary>
        /// New Version - using componets that is used in the custom controls
        /// </summary>
        /// <param name="compo">the entity components</param>
        public GridContentReflection(Component compo)
        {
            //ShowGridLines = true;
            ColumnDefinition cdlabel = new ColumnDefinition();
            ColumnDefinition cdtext = new ColumnDefinition();

            cdlabel.Width = new GridLength(1, GridUnitType.Star);
            cdtext.Width = new GridLength(1, GridUnitType.Star);

            this.ColumnDefinitions.Add(cdlabel);
            this.ColumnDefinitions.Add(cdtext);

            GetEntityComponent(compo);
        }

        /// <summary>
        /// Old version - do not use
        /// </summary>
        /// <param name="name"></param>
        /// <param name="i"></param>
        public void makeControls(string name, int i)
        {
            RowDefinition rd = new RowDefinition();
            this.RowDefinitions.Add(rd);

            Label label = new Label();
            label.Foreground = System.Windows.Media.Brushes.White;
            label.Content = name;
            SetRow(label, i);
            SetColumn(label, 0);

            TextBox value = new TextBox();
            value.Text = "Enter Shit Here";
            SetRow(value, i);
            SetColumn(value, 1);
            value.Height = 22;
            // creating the value name
            value.Name = name;
            rd.Height = new GridLength(label.FontSize + 15, GridUnitType.Pixel);

            Children.Add(label);
            Children.Add(value);
        }

        /// <summary>
        /// New version - make controls dynamically (reflection)
        /// </summary>
        /// <param name="p">Property info aka communication</param>
        /// <param name="O">Object - you can be anything you like!</param>
        /// <param name="i">iterator (to set the row index)</param>
        /// <param name="c">Component c</param>
        public void makeControls(PropertyInfo p, Object O, int i, Component c)
        {
            RowDefinition rd = new RowDefinition();
            this.RowDefinitions.Add(rd);

            PropertyControls prop = new PropertyControls(p, O, c);
            //prop.compo = c;

            SetRow(prop.label, i);
            SetColumn(prop.label, 0);

            SetRow(prop.control, i);
            SetColumn(prop.control, 1);

            rd.Height = new GridLength(prop.label.FontSize + 15, GridUnitType.Pixel);

            Children.Add(prop.label);
            Children.Add(prop.control);

            ControlList.Add(prop);

            //Label label = new Label();
            //label.Foreground = System.Windows.Media.Brushes.White;
            //label.Content = p.Name;
            //SetRow(label, i);
            //SetColumn(label, 0);

            //TextBox value = new TextBox();
            //value.Text = "Enter Shit Here";
            //SetRow(value, i);
            //SetColumn(value, 1);
            //value.Height = 22;
            //// creating the value name
            //value.Name = name;
            //rd.Height = new GridLength(label.FontSize + 15, GridUnitType.Pixel);

            //Children.Add(label);
            //Children.Add(value);
        }

        /// <summary>
        /// old version
        /// </summary>
        /// <param name="name"></param>
        public void getProperties(string name)
        {
            int i = 0;
            //Get Assemblies
            foreach (Assembly asm in GetAssemblies())
            {
                //Look in all types defined in asm 
                foreach (Type type in asm.GetTypes())
                {
                    //get Types that extend from Component and aren't abstract
                    if (type.IsSubclassOf(typeof(Component)) && !type.IsAbstract && type.Name.Equals(name))
                    {
                        //Look at all properties defined by the Component
                        foreach (PropertyInfo property in type.GetProperties())
                        {
                            //Gets the properties that don't have the Attribute: ContentSerializerIgnoreAttribute
                            if (!Attribute.IsDefined(property, typeof(ContentSerializerIgnoreAttribute)))
                            {
                                makeControls(property.Name, i);
                                i++;
                                //ControlList.Add(new PropertyControls());
                                //var Field_Type = property.PropertyType;
                                // Contain [ LABEL | TEXTBOX | PropertyInfo]
                                // This way, when the components is being assigned its know what the hell its trying to do
                            }
                        }

                    }
                }

            }
        }

        /// <summary>
        /// New version! gets the entity
        /// </summary>
        /// <param name="compo">compo</param>
        public void GetEntityComponent(Component compo)
        {
            Type myType = compo.GetType();
            //Console.Write("\tComponent: " + compo.Name + "\n");
            int i = 0;

            foreach (PropertyInfo prop in myType.GetProperties())
            {
                if (!Attribute.IsDefined(prop, typeof(ContentSerializerIgnoreAttribute)))
                {
                    object propValue = prop.GetValue(compo, null);
                    makeControls(prop, propValue, i, compo);
                    i++;
                }
            }
        }

    }
}
