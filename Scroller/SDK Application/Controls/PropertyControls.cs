using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Reflection;
using ScrollerEngine.Components;
using System.Windows;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using System.Windows.Input;
using System.Drawing;
using SDK_Application.Communication;

namespace SDK_Application.Controls
{
    /// <summary>
    /// Worked on by Emmanuel, Peter, Richard, Mary, Jonathan
    /// Custom Control Class that creates a special control depending on the data type
    /// </summary>
    public class PropertyControls
    {
        ResourceDictionary res = (ResourceDictionary)Application.LoadComponent(new Uri("Themes/Styles.xaml", UriKind.Relative));
        public Label label;
        public Control control;
        public PropertyInfo prop;
        public Object propValue;
        public Component compo;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="p">The Propery info</param>
        /// <param name="pValue">The object</param>
        /// <param name="compo"> the component</param>
        public PropertyControls(PropertyInfo p, object pValue, Component compo)
        {
            this.compo = compo;
            prop = p;
            this.propValue = pValue;
            // Makes label
            label = new Label();
            label.Content = p.Name + ": " + pValue;

            // Make controls
            if (pValue is int)
            {
                control = new Slider();
                ((Slider)control).Value = (int)propValue;
                ((Slider)control).Maximum = (int)propValue + (int)propValue*.75f;
                ((Slider)control).Minimum = (int)propValue - (int)propValue *.75f;
                ((Slider)control).ValueChanged += new RoutedPropertyChangedEventHandler<double>(setPropValueToBeControl);
            }
            else if (pValue is bool)
            {
                control = new ComboBox();
                ((ComboBox)control).Items.Add(true);
                ((ComboBox)control).Items.Add(false);
                ((ComboBox)control).SelectedItem = (bool)propValue;
                ((ComboBox)control).SelectionChanged += new SelectionChangedEventHandler(setPropValueToBeControl);
                if (prop.GetSetMethod() == null)
                {
                    ((ComboBox)control).IsEnabled = false;
                    return;
                }
            }
            else if (pValue is string)
            {
                control = new TextBox();
                //((TextBox)control).Style = (Style)res["TBox"];
                
                ((TextBox)control).Text = propValue.ToString();
                ((TextBox)control).MaxLength = 50;
                ((TextBox)control).MouseDoubleClick += new MouseButtonEventHandler(setPropValueToBeControl);
                //((TextBox)control).MouseDoubleClick += new MouseButtonEventHandler(setPropValueToBeControl);

            }
            else if (pValue is float)
            {

                control = new Slider();
                ((Slider)control).Style = (Style)res["Slider"];
                ((Slider)control).Value = (float)propValue;
                ((Slider)control).Maximum = (float)propValue + (float)propValue *0.75f;
                ((Slider)control).Minimum = (float)propValue - (float)propValue *0.75f;
                ((Slider)control).ValueChanged += new RoutedPropertyChangedEventHandler<double>(setPropValueToBeControl);
            }
            else if (pValue is double)
            {
                control = new Slider();
                ((Slider)control).Style = (Style)res["Slider"];
                ((Slider)control).Value = (double)propValue;
                ((Slider)control).Maximum = (double)propValue + (double)propValue * 0.75f;
                ((Slider)control).Minimum = (double)propValue - (double)propValue * 0.75f;
                ((Slider)control).ValueChanged += new RoutedPropertyChangedEventHandler<double>(setPropValueToBeControl);

            }
            else if (pValue is Microsoft.Xna.Framework.Color)
            {
                control = new TextBox();
                //((TextBox)control).Style = (Style)res["TBox"];
                ((TextBox)control).Text = propValue.ToString();
                ((TextBox)control).MaxLength = 50;
                ((TextBox)control).LostFocus += new RoutedEventHandler(setPropValueToBeControl);

            }
            else if (pValue is Vector2)
            {
                control = new TextBox();
                //((TextBox)control).Style = (Style)res["TBox"];
                ((TextBox)control).Text = propValue.ToString();
                ((TextBox)control).MaxLength = 50;
                ((TextBox)control).TextChanged += new TextChangedEventHandler(setPropValueToBeControl);
            }
            else if (pValue is TimeSpan)
            {
                control = new Slider();
                ((Slider)control).Style = (Style)res["Slider"];
                ((Slider)control).Value = ((TimeSpan)propValue).Seconds;
                ((Slider)control).Maximum = ((TimeSpan)propValue).Seconds + ((TimeSpan)propValue).Seconds * 0.75f;
                ((Slider)control).Minimum = ((TimeSpan)propValue).Seconds - ((TimeSpan)propValue).Seconds * 0.75f;
                ((Slider)control).ValueChanged += new RoutedPropertyChangedEventHandler<double>(setPropValueToBeControl);

            }
            else
            {
                control = new TextBox();
                //((TextBox)control).Style = (Style)res["TBox"];
                ((TextBox)control).Text = propValue.ToString();
                ((TextBox)control).MaxLength = 50;
                ((TextBox)control).TextChanged += new TextChangedEventHandler(setPropValueToBeControl);
            }
            if (prop.GetSetMethod() == null)
            {
                control.IsEnabled = false;
                return;
            }
            control.Height = 22;
        }

        /// <summary>
        /// Universal EventHandler for all PropertyControls that sets <b>Component values</b> to be 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void setPropValueToBeControl(object sender, EventArgs e)
        {
            if (control.IsLoaded == true)
            {
                // Make controls
                if (propValue is int)
                {
                    // TODO methods with no setters are apperently being passed (eg. EnemyCount)
                    prop.SetValue(compo, (int)((Slider)control).Value, null);
                    propValue = (int)((Slider)control).Value;
                    label.Content = prop.Name + ": " + propValue;
                    //label.Content = ((Slider)control).Value;

                }
                else if (propValue is bool)
                {
                    prop.SetValue(compo, (bool)((ComboBox)control).SelectedItem, null);
                    propValue = (bool)((ComboBox)control).SelectedValue;
                    label.Content = prop.Name + ": " + propValue;

                }
                else if (propValue is string)
                {
                    string txt_test = FileManagement.open_File(".png");
                    ((TextBox)control).Text = txt_test;
                    prop.SetValue(compo, txt_test, null);
                    propValue = txt_test;
                    label.Content = prop.Name + ": " + propValue;
                }
                else if (propValue is float)
                {
                    prop.SetValue(compo, (float)((Slider)control).Value, null);
                    propValue = (float)((Slider)control).Value;
                    label.Content = prop.Name + ": " + propValue;
                }
                else if (propValue is double)
                {
                    prop.SetValue(compo, (double)((Slider)control).Value, null);
                    propValue = (double)((Slider)control).Value;
                    label.Content = prop.Name + ": " + propValue;
                }
                else if (propValue is Microsoft.Xna.Framework.Color)
                {
                    prop.SetValue(compo, Casting.FromTextToColor((TextBox)control), null);
                    propValue = Casting.FromTextToColor(((TextBox)control));
                    label.Content = prop.Name + ": " + propValue;

                    Microsoft.Xna.Framework.Color co = Casting.FromTextToColor(((TextBox)control));
                    label.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, co.R, co.G, co.B));
                }
                else if (propValue is Vector2)
                {
                    prop.SetValue(compo, Casting.FromTextToVector((TextBox)control), null);
                    propValue = Casting.FromTextToVector(((TextBox)control));
                    label.Content = prop.Name + ": " + propValue;
                }
                else if (propValue is TimeSpan)
                {
                    prop.SetValue(compo,  TimeSpan.FromSeconds(((Slider)control).Value), null);
                    propValue = TimeSpan.FromSeconds(((Slider)control).Value);
                    label.Content = prop.Name + ": " + propValue;
                }
                else
                {
                    prop.SetValue(compo, (Object)((TextBox)control).Text, null);
                    propValue = (string)((TextBox)control).Text;
                    label.Content = prop.Name + ": " + propValue;
                }
            }
        }


    }
}
