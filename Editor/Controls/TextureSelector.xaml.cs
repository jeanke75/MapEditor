using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Editor.Models;
using Editor.TileEngine;

namespace Editor.Controls
{
    /// <summary>
    /// Interaction logic for TextureSelector.xaml
    /// </summary>
    public partial class TextureSelector : UserControl
    {
        public TextureSelector()
        {
            InitializeComponent();
        }

        public void SetTextures(TileSet set)
        {
            ClearTextures();
            
            Bitmap myBitmap = set.Texture;
            for (int i = 0; i < set.TilesHigh * set.TilesWide; i++)
            {
                int h = i % (myBitmap.Height / set.TileHeight);
                int w = i / (myBitmap.Width / set.TileWidth);
                Rectangle cloneRect = new Rectangle(h * set.TileWidth, w * set.TileHeight, set.TileWidth, set.TileHeight);
                Texture tile = new Texture((byte)i, myBitmap.Clone(cloneRect, myBitmap.PixelFormat));
                tile.WasClicked += Texture_WasClicked;
                wrpTextures.Children.Add(tile);
            }
        }

        public void ClearTextures()
        {
            wrpTextures.Children.Clear();
        }

        public Texture GetSelectedTexture()
        {
            return wrpTextures.Children.OfType<Texture>().Where(o => o.IsSelected).FirstOrDefault();
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
    }
}
