using System.Drawing;

namespace Editor.TileEngine
{
    public class TileSet
    {
        #region Fields
        public int TilesWide = 8;
        public int TilesHigh = 8;
        public int TileWidth = 32;
        public int TileHeight = 32;
        #endregion

        #region Property Region
        public Bitmap Texture { get; set; }

        public string TextureName { get; set; }

        public Rectangle[] SourceRectangles { get; }
        #endregion

        #region Constructor Region

        public TileSet()
        {
            SourceRectangles = new Rectangle[TilesWide * TilesHigh];

            int tile = 0;


            for (int y = 0; y < TilesHigh; y++)
                for (int x = 0; x < TilesWide; x++)
                {
                    SourceRectangles[tile] = new Rectangle(
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

            SourceRectangles = new Rectangle[TilesWide * TilesHigh];

            int tile = 0;

            for (int y = 0; y < TilesHigh; y++)
                for (int x = 0; x < TilesWide; x++)
                {
                    SourceRectangles[tile] = new Rectangle(
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