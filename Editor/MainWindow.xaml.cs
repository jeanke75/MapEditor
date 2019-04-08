using Editor.Controls;
using Editor.TileEngine;
using Editor.Windows;
using Microsoft.Win32;
using System.Drawing;
using System.Windows;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int tilewidth = 32;
        public static int tileheight = 32;

        public TileMap currentMap;
        MapDesigner mapDesigner;

        public enum Layer {
            Background,
            Edge,
            Building,
            Decoration
        }

        public MainWindow()
        {
            InitializeComponent();

            var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            Title = "Map Editor - v" + version.Major + "." + version.Minor + "." + version.Build;

            mapDesigner = new MapDesigner(this)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            svMap.Content = mapDesigner;
            svMap.CanContentScroll = true;

            mniSave.IsEnabled = false;
        }

        #region menu
        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            NewMap nm = new NewMap
            {
                Owner = this
            };
            if (nm.ShowDialog() != true) return;

            Bitmap srcTileset = new Bitmap(nm.TilesetFilePath);

            TileSet set = new TileSet(srcTileset.Width / int.Parse(nm.TileWidth), srcTileset.Height / int.Parse(nm.TileHeight), int.Parse(nm.TileWidth), int.Parse(nm.TileHeight))
            {
                TextureName = nm.TilesetFileName,
                Texture = srcTileset
            };

            int w;
            int h;

            TileLayer background = new TileLayer(w = int.Parse(nm.MapWidth), h = int.Parse(nm.MapHeight));
            TileLayer edge = new TileLayer(w, h);
            TileLayer buildings = new TileLayer(w, h);
            TileLayer decorations = new TileLayer(w, h);

            currentMap = new TileMap(set, background, edge, buildings, decorations, "test-map");
            currentMap.FillEdges();
            currentMap.FillBuilding();
            currentMap.FillDecoration();

            mapDesigner.CreateDesignArea();
            mapDesigner.RenderMap();

            LoadTexturesFromTileset();

            mniSave.IsEnabled = true;
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            svMap.Visibility = Visibility.Hidden;
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Binary files (*.bin) | *.bin;"
            };
            if (dlg.ShowDialog() == true)
            {
                currentMap = BinaryReaderWriter.Read(dlg.FileName);

                mapDesigner.CreateDesignArea();
                mapDesigner.RenderMap();

                LoadTexturesFromTileset();

                svMap.Visibility = Visibility.Visible;
                mniSave.IsEnabled = true;
            }
            else
            {
                mniSave.IsEnabled = false;
            }
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            if (currentMap != null)
            {
                SaveFileDialog dlg = new SaveFileDialog
                {
                    Filter = "Binary files (*.bin) | *.bin;"
                };
                if (dlg.ShowDialog() == true)
                    BinaryReaderWriter.Write(dlg.FileName, currentMap);
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region design menu
        private void tbShowGrid_Click(object sender, RoutedEventArgs e)
        {
            mapDesigner.IsGridVisible = tbShowGrid.IsChecked == true;
        }
        #endregion

        private void LoadTexturesFromTileset()
        {
            if (currentMap == null && currentMap.TileSet == null)
            {
                ctrlTextureSelector.ClearTextures();
            }
            else
            {
                ctrlTextureSelector.SetTextures(currentMap.TileSet);
            }
        }
    }
}