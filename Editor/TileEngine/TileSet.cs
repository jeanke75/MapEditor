using System.Drawing;

namespace Editor.TileEngine
{
    public class TileSet
    {
        public int TilesWide = 8;
        public int TilesHigh = 8;
        public int TileWidth = 32;
        public int TileHeight = 32;

        #region Fields and Properties

        Bitmap image;
        string imageName;
        Rectangle[] sourceRectangles;

        #endregion

        #region Property Region
        public Bitmap Texture
        {
            get { return image; }
            set { image = value; }
        }

        public string TextureName
        {
            get { return imageName; }
            set { imageName = value; }
        }
        
        public Rectangle[] SourceRectangles
        {
            get { return sourceRectangles; }
        }
        #endregion

        #region Constructor Region

        public TileSet()
        {
            sourceRectangles = new Rectangle[TilesWide * TilesHigh];

            int tile = 0;


            for (int y = 0; y < TilesHigh; y++)
                for (int x = 0; x < TilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * TileWidth,
                        y * TileHeight,
                        TileWidth,
                        TileHeight);
                    tile++;
                }
        }

        public TileSet(int tilesWide, int tilesHigh, int tileWidth, int tileHeight)
        {
            TilesWide = tilesWide;
            TilesHigh = tilesHigh;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            sourceRectangles = new Rectangle[TilesWide * TilesHigh];

            int tile = 0;

            for (int y = 0; y < TilesHigh; y++)
                for (int x = 0; x < TilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * TileWidth,
                        y * TileHeight,
                        TileWidth,
                        TileHeight);
                    tile++;
                }
        }

        #endregion

        #region Method Region
        #endregion
    }
}
