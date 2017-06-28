using System.Drawing;

namespace Editor.TileEngine
{
    public class TileLayer
    {
        #region Field Region

        int[] tiles;

        int width;
        int height;

        System.Drawing.Point cameraPoint;
        System.Drawing.Point viewPoint;
        System.Drawing.Point min;
        System.Drawing.Point max;
        Rectangle destination;

        #endregion

        #region Property Region

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        public int Width
        {
            get { return width; }
            private set { width = value; }
        }

        public int Height
        {
            get { return height; }
            private set { height = value; }
        }

        #endregion

        #region Constructor Region

        private TileLayer()
        {
            Enabled = true;
            Visible = true;
        }

        public TileLayer(int[] tiles, int width, int height)
            : this()
        {
            this.tiles = (int[])tiles.Clone();
            this.width = width;
            this.height = height;
        }

        public TileLayer(int width, int height)
            : this()
        {
            tiles = new int[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y * width + x] = 0;
                }
            }
        }

        public TileLayer(int width, int height, int fill)
            : this()
        {
            tiles = new int[height * width];
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y * width + x] = fill;
                }
            }
        }

        #endregion

        #region Method Region

        public int GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                return -1;

            if (x >= width || y >= height)
                return -1;

            return tiles[y * width + x];
        }

        public void SetTile(int x, int y, int tileIndex)
        {
            if (x < 0 || y < 0)
                return;

            if (x >= width || y >= height)
                return;

            tiles[y * width + x] = tileIndex;
        }
        #endregion
    }
}
