using ScrollerEngine.Components;
using ScrollerEngine.Components.Graphics;
using SDK_Application.Communication;
using SDK_Application.Controls;
using SDK_Application.Error_Handling;
using SDK_Application.Image_Processing;
using SDK_Application.Input;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SDK_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected App App { get { return ((App)App.Current); } }
        public event EventHandler ClickToClose;
        public ObservableCollection<ComponentInfo> ComponentCollection { get { return App.ComponentCollection; } }

        int imgIndex = 0; // Initialize the stuff to be used in these events 
        int duration = 125;
        int minSpeed = 5;
        EntityCollection EC = new EntityCollection();
        public string xml_filename; // Entity editor
        bool _CurrentPlaying = false;
        bool _IsPlaying = false;
        System.ComponentModel.BackgroundWorker _BackgroundWorker;

        /// <summary>
        /// Stuff to be initialized (Think of this as the constructor function)
        /// </summary>
        public MainWindow()
        {
            //this.Icon = new BitmapImage(new Uri("Images/Playback/1384576670_61482.ico", UriKind.Relative));
            InitializeComponent();
            _BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            _BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(_BackgroundWorker_DoWork);
            _BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(_BackgroundWorker_RunWorkerCompleted);
        }

        void _BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _IsPlaying = false;
        }

        void _BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (_CurrentPlaying)
            {
                this.Dispatcher.BeginInvoke((Action)delegate()
                {
                    imgIndex = SpritePreviewer.PlayNextFrame(imgIndex, imageYIndexTextBox, imageXIndexTextBox, frameCount,
                        SpritesPerRowTextBox, ImageFileNameTextBox, SpritesPerColumnTextBox, Test_Img);
                });
                System.Threading.Thread.Sleep(duration);
            }
        }

        /// <summary>
        /// When browsing for an xml file with a SpriteComponent, attempt to open the image associated with file.
        /// </summary>
        private void OpenPngOnBrowse()
        {
            SpriteComponent SC = EC.getEntity(xml_filename).GetComponent<SpriteComponent>();
            if (SC == null)
                return;

            string filename = SC.TextureName;
            if (string.IsNullOrEmpty(filename))
                return;

            bool rightExtension = ImageHandling.ImgIsValid(filename);
            if (rightExtension)
            {
                spriteimg.Source = SpritePreviewer.ReturnBitmapFile(filename);
                spriteimg.ToolTip = "Height: " + (int)spriteimg.Source.Height + " Width: " + (int)spriteimg.Source.Width;
            }
        }

        /// <summary>
        /// When browsing for an image button is executed, this events appens
        /// </summary>
        private void OpenPng(object sender, RoutedEventArgs e)
        {
            FileManagement.open_File(ImageFileNameTextBox, "png", "C:\\Users\\Public\\Pictures");
            if (string.IsNullOrEmpty(ImageFileNameTextBox.Text))
                return;
            bool rightExtension = ImageHandling.ImgIsValid(ImageFileNameTextBox.Text);
            if (rightExtension)
            {
                spriteimg.Source = SpritePreviewer.ReturnBitmapFile(ImageFileNameTextBox.Text);
                spriteimg.ToolTip = "Height: " + (int)spriteimg.Source.Height + " Width: " + (int)spriteimg.Source.Width;
                char s = '\\';
                string[] test = ImageFileNameTextBox.Text.Split(s);
                if (test[test.Length-1].Equals("platformer_sprites_pixelized_mirrored.png"))
                {
                    SpritesPerRowTextBox.Text = "8";
                    SpritesPerColumnTextBox.Text = "18";
                    imageXIndexTextBox.Text = "0";
                    imageYIndexTextBox.Text = "4";
                    frameCount.Text = "8";
                    txtAnimationDuration.Text = "0.05";
                }
            }
            else
            {
                ImageFileNameTextBox.Text = "";
            }
        }

        /// <summary>
        /// Resets everything back to original state when event is exected 
        /// </summary>
        private void New_Click(object sender, RoutedEventArgs e)
        {
            PlayerEntityBrowseFileTextBox.Text = "";
            SizeBx.Text = "";
            var spritePreviewer = SpriteTab;
            TCcomp.Items.Clear();
            TCcomp.Items.Add(spritePreviewer);
            TCcomp.SelectedIndex = 0;
        }

        /// <summary>
        /// Event that browses for an xml file, then creates the neccesary tabs
        /// </summary>
        private void browse_xml(object sender, RoutedEventArgs e)
        {
            if (!EC.insert(ref xml_filename))
                return;
            EC.getPropertyValue(xml_filename);
            var spritePrevier = SpriteTab;
            TCcomp.Items.Clear();
            TCcomp.Items.Add(spritePrevier);
            _Animations.Clear();
            foreach (Component com in EC.getEntity(xml_filename).Components)
            {
                Grid customGrid = new GridContentReflection(com);
                customGrid.Style = Resources["GridStyle"] as Style;
                TCcomp.Items.Add(new TabItemVM(com.Name, customGrid));
            }
            PlayerEntityBrowseFileTextBox.Text = System.IO.Path.GetFileNameWithoutExtension(xml_filename);
            SizeBx.Text = FileManagement.size.ToString();
            OpenPngOnBrowse();
        }

        /// <summary>
        /// Loads Component on a dobule mouse click event
        /// </summary>
        private void MouseDbClick_LoadComponent(object sender, MouseButtonEventArgs e)
        {
            ComponentInfo selectedItems = (ComponentInfo)CompCollection.SelectedItem;
            string name = selectedItems.Name;

            Grid customGrid = new GridContentReflection(name);
            customGrid.Style = Resources["GridStyle"] as Style;

            var current = this.TCcomp.Items;
            bool doesNotHaveTab = true;
            foreach (TabItem item in TCcomp.Items)
            {
                if (item.Header.ToString().Equals(name))
                {
                    doesNotHaveTab = false;
                    break;
                }
            }

            if (doesNotHaveTab)
            {
                TCcomp.Items.Add(new TabItemVM(name, customGrid));
                TCcomp.SelectedIndex = TCcomp.Items.Count - 1;
            } 


        }
        /// <summary>
        /// When Play button is pressed, this event executes
        /// </summary>
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (InputHandling.isNumeric(imageXIndexTextBox, imageYIndexTextBox, SpritesPerColumnTextBox, SpritesPerRowTextBox))
            {
                if ((SpritesPerRowTextBox.Text != "") && (SpritesPerColumnTextBox.Text != "")
                    && (imageYIndexTextBox.Text != "") && (imageXIndexTextBox.Text != "")
                    && (frameCount.Text != "") && (ImageFileNameTextBox.Text != "") && _IsPlaying == false)
                {
                    _IsPlaying = true;
                    _CurrentPlaying = true;
                    _BackgroundWorker.RunWorkerAsync();

                    int frameSpeed = (int)(Convert.ToSingle(txtAnimationDuration.Text) * 1000);
                    if (frameSpeed < minSpeed)
                        txtAnimationDuration.Text = "0.05";

                    duration = string.IsNullOrEmpty(txtAnimationDuration.Text) ? 125 : Math.Max(frameSpeed, minSpeed);
                }
                else if (_IsPlaying && _CurrentPlaying)
                {
                    //nothing should happen because its playing so no errors
                }
                else
                {
                    MessageBoxes.Alert_PopUP("Missing field entries");
                }
            }
        }

        /// <summary>
        /// reset the image frame back to the beginning
        /// </summary>
        private void Stop_ButtonClick(object sender, RoutedEventArgs e)
        {
            //Set imgIndex to -1 so the next frame is 0 (the start)
            if (InputHandling.isNumeric(imageXIndexTextBox, imageYIndexTextBox, SpritesPerColumnTextBox, SpritesPerRowTextBox))
            {
                imgIndex = -1;
                imgIndex = SpritePreviewer.PlayNextFrame(imgIndex, imageYIndexTextBox, imageXIndexTextBox, frameCount,
                    SpritesPerRowTextBox, ImageFileNameTextBox, SpritesPerColumnTextBox, Test_Img);
                _CurrentPlaying = false;
            }
        }

        /// <summary>
        /// Pause at current imgIndex frame
        /// </summary>
        private void Pause_ButtonClick(object sender, RoutedEventArgs e)
        {
            //Go to previous frame and then iterate back to the frame you were at
            imgIndex -= 1;
            imgIndex = SpritePreviewer.PlayNextFrame(imgIndex, imageYIndexTextBox, imageXIndexTextBox, frameCount,
                SpritesPerRowTextBox, ImageFileNameTextBox, SpritesPerColumnTextBox, Test_Img);
            _CurrentPlaying = false;
        }
        /// <summary>
        /// Once Save button is pressed, XML file is saved to file
        /// </summary>
        private void SaveTOXMLFILE(object sender, RoutedEventArgs e)
        {
            FileManagement.Save_File_xml(EC.getEntity(xml_filename));
        }

        /// <summary>
        /// Add Animation for now
        /// </summary>
        private void AddAnimation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var imageFilename = ImageFileNameTextBox.Text;
                var name = (string)((ListBoxItem)cbAnimNames.SelectedItem).Content; // txtboxAnimationName.Text;
                var spr = Convert.ToInt32(SpritesPerRowTextBox.Text);
                var spc = Convert.ToInt32(SpritesPerColumnTextBox.Text);
                var imgIndexX = Convert.ToInt32(imageXIndexTextBox.Text);
                var imgIndexY = Convert.ToInt32(imageYIndexTextBox.Text);
                var frmCount = Convert.ToInt32(frameCount.Text);
                var dur = Convert.ToSingle(txtAnimationDuration.Text);

                Bitmap imgsrc = new Bitmap(ImageFileNameTextBox.Text);

                int width = (int)(imgsrc.Width / spr);
                int height = (int)(imgsrc.Height / spc);

                SpriteComponent SC = EC.getEntity(xml_filename).GetComponent<SpriteComponent>();

                if (SC != null)
                {
                    if (SC.faAnimations.ContainsKey(name))
                    {
                        SC.RemoveAnimationWithName(name);
                    }

                    SC.AddAnimation(name, imgIndexX * width, imgIndexY * height, width, height, dur, frmCount, (int)(imgsrc.Width));
                    HitboxAnalyzer.IntializeAutomaticHurtBoxes(imgsrc, SC.faAnimations[name]);

                    SC.TextureName = imageFilename;
                }

                System.Windows.MessageBox.Show("New animation added, be sure to click \"Save XML\" for changes to take effect.", "Success", MessageBoxButton.OK, MessageBoxImage.None);
            }
            catch 
            {
                System.Windows.MessageBox.Show("Did not add new animation. You may have left a textbox unfilled, or did not load an XML file.", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (ClickToClose != null)
            {
                ClickToClose(sender, e);
            }
        }


        private ObservableCollection<AnimationFrame> _Animations = new ObservableCollection<AnimationFrame>();
        public ObservableCollection<AnimationFrame> Animations
        {
            get { return _Animations; }
            set { _Animations = value; }
        }
    }
}

/// <summary>
/// Custom TabItem corresponding to a component
/// </summary>
public class TabItemVM : TabItem
{
    public TabItemVM(string header, Grid grid)
    {
        Header = header;
        Content = grid;
    }
}
