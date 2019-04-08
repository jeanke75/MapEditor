using Editor.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Editor.MainWindow;

namespace Editor.Controls
{
    /// <summary>
    /// Interaction logic for MapDesigner.xaml
    /// </summary>
    public partial class MapDesigner : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        MainWindow main;
        bool isGridVisible;
        public bool IsGridVisible
        {
            get { return isGridVisible; }
            set
            {
                isGridVisible = value;
                NotifyPropertyChanged();
            }
        }

        public MapDesigner(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            this.DataContext = this;
        }

        public void CreateDesignArea()
        {
            if (main.currentMap == null) throw new Exception("No map data available!");

            grdTiles.RowDefinitions.Clear();
            grdTiles.ColumnDefinitions.Clear();
            for (int y = 0; y < main.currentMap.MapHeight; y++)
            {
                grdTiles.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(tileheight) });
            }

            for (int y = 0; y < main.currentMap.MapWidth; y++)
            {
                grdTiles.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(tilewidth) });
            }
        }

        public void RenderMap()
        {
            for (int y = 0; y < main.currentMap.MapHeight; y++)
            {
                for (int x = 0; x < main.currentMap.MapWidth; x++)
                {
                    DrawTile(x, y, Layer.Background);
                    DrawTile(x, y, Layer.Edge);
                    DrawTile(x, y, Layer.Building);
                    DrawTile(x, y, Layer.Decoration);
                }
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

        public void Draw(int x, int y)
        {
            Texture t = main.ctrlTextureSelector.GetSelectedTexture();
            if (t != null)
            {
                switch (main.ctrlLayerSelector.GetSelectedLayer())
                {
                    case Layer.Background:
                        if (main.currentMap.GetGroundTile(x, y) == t.ID) return;
                        main.currentMap.SetGroundTile(x, y, t.ID);
                        break;
                    case Layer.Edge:
                        if (main.currentMap.GetEdgeTile(x, y) == t.ID) return;
                        main.currentMap.SetEdgeTile(x, y, t.ID);
                        break;
                    case Layer.Building:
                        if (main.currentMap.GetBuildingTile(x, y) == t.ID) return;
                        main.currentMap.SetBuildingTile(x, y, t.ID);
                        break;
                    case Layer.Decoration:
                        if (main.currentMap.GetDecorationTile(x, y) == t.ID) return;
                        main.currentMap.SetDecorationTile(x, y, t.ID);
                        break;
                    default:
                        throw new NotImplementedException("This layer can't be handled in Draw()");
                }

                DrawTile(x, y, main.ctrlLayerSelector.GetSelectedLayer());
            }
        }

        private void DrawTile(int x, int y, Layer layer)
        {
            Image img = grdTiles.Children.OfType<Image>().Where(o => Grid.GetColumn(o) == x && Grid.GetRow(o) == y && (Layer)o.Tag == layer).FirstOrDefault();
            if (img == null)
            {
                img = new Image
                {
                    Tag = layer
                };
                grdTiles.Children.Add(img);
                Grid.SetZIndex(img, (int)layer);
                Grid.SetRow(img, y);
                Grid.SetColumn(img, x);
            }

            int tile = -1;
            switch (layer)
            {
                case Layer.Background:
                    tile = main.currentMap.GetGroundTile(x, y);
                    break;
                case Layer.Edge:
                    tile = main.currentMap.GetEdgeTile(x, y);
                    break;
                case Layer.Building:
                    tile = main.currentMap.GetBuildingTile(x, y);
                    break;
                case Layer.Decoration:
                    tile = main.currentMap.GetDecorationTile(x, y);
                    break;
            }

            if (tile >= 0)
            {
                System.Drawing.Bitmap b = main.currentMap.TileSet.Texture.Clone(main.currentMap.TileSet.SourceRectangles[tile], main.currentMap.TileSet.Texture.PixelFormat);
                img.Source = Extensions.ImageSourceForBitmap(b);
                UpdateLayout();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}