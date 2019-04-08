using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for NewMap.xaml
    /// </summary>
    public partial class NewMap : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string mapWidth;
        public string MapWidth
        {
            get { return mapWidth; }
            set
            {
                mapWidth = value;
                NotifyPropertyChanged();
            }
        }

        private string mapHeight;
        public string MapHeight
        {
            get { return mapHeight; }
            set
            {
                mapHeight = value;
                NotifyPropertyChanged();
            }
        }

        private string tilesetFilePath;
        public string TilesetFilePath
        {
            get { return tilesetFilePath; }
            set
            {
                tilesetFilePath = value;
                NotifyPropertyChanged();
            }
        }

        private string tilesetFileName;
        public string TilesetFileName
        {
            get { return tilesetFileName; }
            set
            {
                tilesetFileName = value;
                NotifyPropertyChanged();
            }
        }

        private string tileWidth;
        public string TileWidth
        {
            get { return tileWidth; }
            set
            {
                tileWidth = value;
                NotifyPropertyChanged();
            }
        }

        private string tileHeight;
        public string TileHeight
        {
            get { return tileHeight; }
            set
            {
                tileHeight = value;
                NotifyPropertyChanged();
            }
        }

        public NewMap()
        {
            InitializeComponent();
            DataContext = this;
            MapWidth = "10";
            MapHeight = "10";
            TileWidth = "32";
            TileHeight = "32";
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtMapWidth.Focus();
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PickTileset_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png) | *.png;";
            if (dlg.ShowDialog() == true)
            {
                TilesetFilePath = dlg.FileName;
                TilesetFileName = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName);
            }
        }
    }
}