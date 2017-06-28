using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Editor.Controls;
using Editor.TileEngine;
using Microsoft.Win32;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int tilewidth = 32;
        public static int tileheight = 32;
        TileMap currentMap;
        enum Layer {
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

            mniSave.IsEnabled = false;
        }

        private void CreateMapArea()
        {
            grdTiles.RowDefinitions.Clear();
            grdTiles.ColumnDefinitions.Clear();
            for (int y = 0; y < currentMap.MapHeight; y++)
            {
                grdTiles.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(tileheight) });
            }

            for (int y = 0; y < currentMap.MapWidth; y++)
            {
                grdTiles.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(tilewidth) });
            }
        }

        private void DrawMap()
        {
            for (int y = 0; y < currentMap.MapHeight; y++)
            {
                for (int x = 0; x < currentMap.MapWidth; x++)
                {
                    DrawTile(x, y, Layer.Background);
                    DrawTile(x, y, Layer.Edge);
                    DrawTile(x, y, Layer.Building);
                    DrawTile(x, y, Layer.Decoration);
                }
            }
        }

        private void DrawTile(int x, int y, Layer layer)
        {
            System.Windows.Controls.Image img = grdTiles.Children.OfType<System.Windows.Controls.Image>().Where(o => Grid.GetColumn(o) == x && Grid.GetRow(o) == y && (Layer)o.Tag == layer).FirstOrDefault();
            if (img == null)
            {
                img = new System.Windows.Controls.Image();
                img.Tag = layer;
                grdTiles.Children.Add(img);
                Grid.SetZIndex(img, (int)layer);
                Grid.SetRow(img, y);
                Grid.SetColumn(img, x);
            }

            int tile = -1;
            switch (layer)
            {
                case Layer.Background:
                    tile = currentMap.GetGroundTile(x, y);
                    break;
                case Layer.Edge:
                    tile = currentMap.GetEdgeTile(x, y);
                    break;
                case Layer.Building:
                    tile = currentMap.GetBuildingTile(x, y);
                    break;
                case Layer.Decoration:
                    tile = currentMap.GetDecorationTile(x, y);
                    break;
            }

            if (tile >= 0)
            {
                Bitmap b = currentMap.TileSet.Texture.Clone(currentMap.TileSet.SourceRectangles[tile], currentMap.TileSet.Texture.PixelFormat);
                img.Source = Extensions.ImageSourceForBitmap(b);
                UpdateLayout();
            }
        }

        private void TileClicked(object sender, MouseButtonEventArgs e)
        {
            Texture t = wrpTextures.Children.OfType<Texture>().Where(o => o.IsSelected).FirstOrDefault();
            if (t == null) return;

            int x = Grid.GetColumn((UIElement)sender);
            int y = Grid.GetRow((UIElement)sender);
            if (rbBackground.IsChecked == true)
            {
                currentMap.SetGroundTile(x, y, t.ID);
                DrawTile(x, y, Layer.Background);
            }
            else if (rbEdge.IsChecked == true)
            {
                currentMap.SetEdgeTile(x, y, t.ID);
                DrawTile(x, y, Layer.Edge);
            }
            else if (rbBuilding.IsChecked == true)
            {
                currentMap.SetBuildingTile(x, y, t.ID);
                DrawTile(x, y, Layer.Building);
            }
            else if (rbDecoration.IsChecked == true)
            {
                currentMap.SetDecorationTile(x, y, t.ID);
                DrawTile(x, y, Layer.Decoration);
            }
        }

        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            NewMap nm =  new NewMap();
            nm.Owner = this;
            if (nm.ShowDialog() != true) return;

            Bitmap srcTileset = new Bitmap(nm.TilesetFilePath);

            TileSet set = new TileSet(srcTileset.Width / int.Parse(nm.TileWidth), srcTileset.Height / int.Parse(nm.TileHeight), int.Parse(nm.TileWidth), int.Parse(nm.TileHeight));
            set.TextureName = nm.TilesetFileName;
            set.Texture = srcTileset;

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

            CreateMapArea();
            DrawMap();
            LoadTexturesFromTileset();

            mniSave.IsEnabled = true;
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            svMap.Visibility = Visibility.Hidden;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Binary files (*.bin) | *.bin;";
            if (dlg.ShowDialog() == true)
            {
                currentMap = BinaryReaderWriter.Read(dlg.FileName);

                CreateMapArea();
                DrawMap();
                LoadTexturesFromTileset();

                svMap.Visibility = Visibility.Visible;
                mniSave.IsEnabled = true;
            }
            mniSave.IsEnabled = false;
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            if (currentMap != null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Binary files (*.bin) | *.bin;";
                if (dlg.ShowDialog() == true)
                    BinaryReaderWriter.Write(dlg.FileName, currentMap);
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void LoadTexturesFromTileset()
        {
            if (currentMap == null && currentMap.TileSet == null) return;

            Bitmap myBitmap = currentMap.TileSet.Texture;
            for (int i = 0; i < currentMap.TileSet.TilesHigh * currentMap.TileSet.TilesWide; i++)
            {
                int h = i % (myBitmap.Height / currentMap.TileSet.TileHeight);
                int w = i / (myBitmap.Width / currentMap.TileSet.TileWidth);
                Rectangle cloneRect = new Rectangle(h * currentMap.TileSet.TileWidth, w * currentMap.TileSet.TileHeight, currentMap.TileSet.TileWidth, currentMap.TileSet.TileHeight);
                Texture tile = new Texture((byte)i, myBitmap.Clone(cloneRect, myBitmap.PixelFormat));
                tile.WasClicked += Texture_WasClicked;
                wrpTextures.Children.Add(tile);
            }
        }

        private void Texture_WasClicked(object sender, EventArgs e)
        {
            foreach (UIElement c in wrpTextures.Children)
            {
                if (c is Texture)
                {
                    ((Texture)c).IsSelected = false;
                }
            }
        }

        private void grdTiles_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is Grid) return;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                UIElement element = (UIElement)e.Source;
                int x = Grid.GetColumn(element);
                int y = Grid.GetRow(element);

                Draw(x, y);
            }
        }

        private void grdTiles_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is Grid) return;

            UIElement element = (UIElement)e.Source;
            int x = Grid.GetColumn(element);
            int y = Grid.GetRow(element);

            Draw(x, y);
        }

        private void Draw(int x, int y)
        {
            Texture t = wrpTextures.Children.OfType<Texture>().Where(o => o.IsSelected).FirstOrDefault();
            if (t == null) return;

            if (rbBackground.IsChecked == true)
            {
                if (currentMap.GetGroundTile(x, y) == t.ID) return;
                currentMap.SetGroundTile(x, y, t.ID);
                DrawTile(x, y, Layer.Background);
            }
            else if (rbEdge.IsChecked == true)
            {
                if (currentMap.GetEdgeTile(x, y) == t.ID) return;
                currentMap.SetEdgeTile(x, y, t.ID);
                DrawTile(x, y, Layer.Edge);
            }
            else if (rbBuilding.IsChecked == true)
            {
                if (currentMap.GetBuildingTile(x, y) == t.ID) return;
                currentMap.SetBuildingTile(x, y, t.ID);
                DrawTile(x, y, Layer.Building);
            }
            else if (rbDecoration.IsChecked == true)
            {
                if (currentMap.GetDecorationTile(x, y) == t.ID) return;
                currentMap.SetDecorationTile(x, y, t.ID);
                DrawTile(x, y, Layer.Decoration);
            }
        }
    }
}