namespace Editor.TileEngine
{
    public class TileMap
    {
        #region Field Region

        string mapName;
        TileLayer groundLayer;
        TileLayer edgeLayer;
        TileLayer buildingLayer;
        TileLayer decorationLayer;
        //Dictionary<string, Point> characters;
        //CharacterManager characterManager;
        //PortalLayer portalLayer;

        int mapWidth;
        int mapHeight;

        TileSet tileSet;

        #endregion

        #region Property Region
        public string MapName
        {
            get { return mapName; }
            private set { mapName = value; }
        }

        public TileSet TileSet
        {
            get { return tileSet; }
            set { tileSet = value; }
        }

        public TileLayer GroundLayer
        {
            get { return groundLayer; }
            set { groundLayer = value; }
        }

        public TileLayer EdgeLayer
        {
            get { return edgeLayer; }
            set { edgeLayer = value; }
        }

        public TileLayer BuildingLayer
        {
            get { return buildingLayer; }
            set { buildingLayer = value; }
        }

        /*public PortalLayer PortalLayer
        {
            get { return portalLayer; }
            private set { portalLayer = value; }
        }*/

        /*public Dictionary<string, Point> Characters
        {
            get { return characters; }
            private set { characters = value; }
        }*/

        public int MapWidth
        {
            get { return mapWidth; }
        }

        public int MapHeight
        {
            get { return mapHeight; }
        }

        public int WidthInPixels
        {
            get { return mapWidth * 32; }
        }

        public int HeightInPixels
        {
            get { return mapHeight * 32; }
        }

        #endregion

        #region Constructor Region

        public TileMap()
        {
        }

        private TileMap(TileSet tileSet, string mapName/*, PortalLayer portals = null*/)
        {
            //this.characters = new Dictionary<string, Point>();
            this.tileSet = tileSet;
            this.mapName = mapName;
            //characterManager = CharacterManager.Instance;

            //portalLayer = portals != null ? portals : new PortalLayer();
        }

        public TileMap(
            TileSet tileSet,
            TileLayer groundLayer,
            TileLayer edgeLayer,
            TileLayer buildingLayer,
            TileLayer decorationLayer,
            string mapName/*,
            PortalLayer portalLayer = null*/)
            : this(tileSet, mapName/*, portalLayer*/)
        {
            this.groundLayer = groundLayer;
            this.edgeLayer = edgeLayer;
            this.buildingLayer = buildingLayer;
            this.decorationLayer = decorationLayer;

            mapWidth = groundLayer.Width;
            mapHeight = groundLayer.Height;
        }

        #endregion

        #region Method Region

        public void SetGroundTile(int x, int y, int index)
        {
            groundLayer.SetTile(x, y, index);
        }

        public int GetGroundTile(int x, int y)
        {
            return groundLayer.GetTile(x, y);
        }

        public void SetEdgeTile(int x, int y, int index)
        {
            edgeLayer.SetTile(x, y, index);
        }

        public int GetEdgeTile(int x, int y)
        {
            return edgeLayer.GetTile(x, y);
        }

        public void SetBuildingTile(int x, int y, int index)
        {
            buildingLayer.SetTile(x, y, index);
        }

        public int GetBuildingTile(int x, int y)
        {
            return buildingLayer.GetTile(x, y);
        }

        public void SetDecorationTile(int x, int y, int index)
        {
            decorationLayer.SetTile(x, y, index);
        }

        public int GetDecorationTile(int x, int y)
        {
            return decorationLayer.GetTile(x, y);
        }

        public void FillEdges()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    edgeLayer.SetTile(x, y, -1);
                }
            }
        }

        public void FillBuilding()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    buildingLayer.SetTile(x, y, -1);
                }
            }
        }

        public void FillDecoration()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    decorationLayer.SetTile(x, y, -1);
                }
            }
        }
        #endregion
    }
}
