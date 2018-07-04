using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using ScrollerEngine.Components;
using ScrollerEngine;

namespace SDK_Application.Communication
{
    /// <summary>
    /// Worked on by Richard, Mary, Emmanuel, Peter
    /// </summary>
    class FileManagement
    {
        static public int size = -1;

        /// <summary>
        /// Opens file
        /// </summary>
        /// <param name="extension_type"> Specifies the extension </param>
        /// <returns> the full path entered by user</returns>
        public static string open_File(String extension_type)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Data\\Entities\\";

            openFileDialog1.Filter = "Entity"+ " ("+extension_type+")|"+ "*"+ extension_type; // Filter files by extension
            Nullable<bool> result = openFileDialog1.ShowDialog(); // Show the dialog.
            String file2 = "";
            if (result == true) // Test result.
            {
                file2 = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file2);
                    byte[] arraysize = File.ReadAllBytes(file2);
                    size = arraysize.Length;
                }
                catch (IOException)
                {
                }
            }
            return Convert.ToString(file2);
        }

        /// <summary>
        /// Prompts user to open a file, and puts string path into txtbox
        /// </summary>
        /// <param name="txtbox">takes in the textbox to output string path</param>
        public static void open_File(System.Windows.Controls.TextBox txtbox)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Data\\Entities\\";
            Nullable<bool> result = openFileDialog1.ShowDialog(); // Show the dialog.
            String file2 = "";
            if (result == true) // Test result.
            {
                file2 = openFileDialog1.FileName;
                    try
                {
                    string text = File.ReadAllText(file2);
                    byte[] arraysize = File.ReadAllBytes(file2);
                    size = arraysize.Length;
                }
                catch (IOException)
                {
                }
            }
            txtbox.Text = Convert.ToString(file2);
        }
       
        /// <summary>
        /// Open file
        /// </summary>
        /// <param name="txtbox">sets the textbox to be filename</param>
        /// <param name="extension"> example: <example><code>"*.xml"</code></example></param>
        public static void open_File(System.Windows.Controls.TextBox txtbox, string extension, string initialDirectory = "Data\\Entities\\")
        {
            var dir = initialDirectory;
            if (!Path.IsPathRooted(initialDirectory))
                dir = System.IO.Directory.GetCurrentDirectory() + "\\" + initialDirectory;

            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.InitialDirectory = dir;
            openFileDialog1.Filter = extension + "|*." + extension; 
            Nullable<bool> result = openFileDialog1.ShowDialog(); // Show the dialog.
            String file2 = "";
            if (result == true) // Test result.
            {
                file2 = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file2);
                    byte[] arraysize = File.ReadAllBytes(file2);
                    size = arraysize.Length;
                }
                catch (IOException)
                {
                }

                txtbox.Text = Convert.ToString(file2);
            }
        }

        /// <summary>
        /// Serializes file with given name
        /// <param name="entity">Pass in Entity to Serialize</param>
        /// </summary>
        public static void Save_File_xml(Entity entity)
        {
           // Configure save file dialog box
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Entity.xml"; // Default file name
                dlg.DefaultExt = ".xml"; // Default file extension
                dlg.Filter = "Entity File |*.xml"; // Filter files by extension 

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results 
                if (result == true)
                {
                    // Save document 
                    // TODO Serialize here
                    ScrollerSerializer.Serialize(dlg.FileName, entity);

                }

        }

        /// <summary>
        /// Saves the file to path specified
        /// <param name="extension">default extension</param>
        /// <param name="name"> default name</param>
        /// <param name="path"> default path</param>
        /// </summary>
        public static void Save_File(String path, String name, String extension)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = name; // Default file name
            dlg.DefaultExt = extension; // Default file extension
            dlg.InitialDirectory = path;
            dlg.Filter = "All Files|*.*"; // Filter files by extension 

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                // Save document 
                string filename = dlg.FileName;
                using (StreamWriter sw = new StreamWriter(dlg.FileName))
                    sw.WriteLine("Hello World!");

            }

        }

        /// <summary>
        /// This function doesnt have to exist... lol XML
        /// The XML is the resume state o_O
        /// </summary>
        /// <param name="fullpath">the fullpath</param>
        public static void Resume_State(String fullpath)
        {
            // ??
           // Opening applications state
        }

        /// <summary>
        /// This function doesnt have to exist... lol XML
        /// The XML is the save state o_O
        /// </summary>
        /// <param name="fullpath"></param>
        public static void Save_State(String fullpath)
        {
            // ??
            // Saving application's state
        }
    }

}
