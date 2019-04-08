namespace Editor.TileEngine
{
    public class TileMap
    {

        #region Field Region
        #endregion

        #region Property Region
        public string MapName { get; private set; }

        public TileSet TileSet { get; set; }

        public TileLayer GroundLayer { get; set; }

        public TileLayer EdgeLayer { get; set; }

        public TileLayer BuildingLayer { get; set; }

        public TileLayer DecorationLayer { get; set; }

        public int MapWidth { get; }

        public int MapHeight { get; }

        public int WidthInPixels
        {
            get { return MapWidth * 32; }
        }

        public int HeightInPixels
        {
            get { return MapHeight * 32; }
        }

        #endregion

        #region Constructor Region

        public TileMap()
        {
        }

        private TileMap(TileSet tileSet, string mapName)
        {
            TileSet = tileSet;
            MapName = mapName;
        }

        public TileMap(
            TileSet tileSet,
            TileLayer groundLayer,
            TileLayer edgeLayer,
            TileLayer buildingLayer,
            TileLayer decorationLayer,
            string mapName)
            : this(tileSet, mapName)
        {
            GroundLayer = groundLayer;
            EdgeLayer = edgeLayer;
            BuildingLayer = buildingLayer;
            DecorationLayer = decorationLayer;

            MapWidth = groundLayer.Width;
            MapHeight = groundLayer.Height;
        }

        #endregion

        #region Method Region

        public void SetGroundTile(int x, int y, int index)
        {
            GroundLayer.SetTile(x, y, index);
        }

        public int GetGroundTile(int x, int y)
        {
            return GroundLayer.GetTile(x, y);
        }

        public void SetEdgeTile(int x, int y, int index)
        {
            EdgeLayer.SetTile(x, y, index);
        }

        public int GetEdgeTile(int x, int y)
        {
            return EdgeLayer.GetTile(x, y);
        }

        public void SetBuildingTile(int x, int y, int index)
        {
            BuildingLayer.SetTile(x, y, index);
        }

        public int GetBuildingTile(int x, int y)
        {
            return BuildingLayer.GetTile(x, y);
        }

        public void SetDecorationTile(int x, int y, int index)
        {
            DecorationLayer.SetTile(x, y, index);
        }

        public int GetDecorationTile(int x, int y)
        {
            return DecorationLayer.GetTile(x, y);
        }

        public void FillEdges()
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    EdgeLayer.SetTile(x, y, -1);
                }
            }
        }

        public void FillBuilding()
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    BuildingLayer.SetTile(x, y, -1);
                }
            }
        }

        public void FillDecoration()
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    DecorationLayer.SetTile(x, y, -1);
                }
            }
        }
        #endregion
    }
}