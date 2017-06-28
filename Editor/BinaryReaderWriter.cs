using System.Drawing;
using System.IO;
using Editor.TileEngine;

namespace Editor
{
    public class BinaryReaderWriter
    {
        public static TileMap Read(string filename)
        {
            TileMap map = new TileMap();
            // 1.
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read))) // expects .bin file
            {
                int length = (int)reader.BaseStream.Length;

                if (length > 0)
                {
                    int h = 0;
                    int w = 0;
                    string tilesetName = reader.ReadString();
                    Bitmap src = new Bitmap(tilesetName + ".png");
                    TileSet set = new TileSet(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                    set.TextureName = tilesetName;
                    set.Texture = src;

                    TileLayer background = new TileLayer(w = reader.ReadInt32(), h = reader.ReadInt32());
                    TileLayer edge = new TileLayer(w, h);
                    TileLayer buildings = new TileLayer(w, h);
                    TileLayer decorations = new TileLayer(w, h);

                    map = new TileMap(set, background, edge, buildings, decorations, "test-map");
                    map.FillEdges();
                    map.FillBuilding();
                    map.FillDecoration();

                    for (int j = 0; j < h; j++)
                    {
                        for (int i = 0; i < w; i++)
                        {
                            map.SetGroundTile(i, j, reader.ReadInt32());
                            map.SetEdgeTile(i, j, reader.ReadInt32());
                            map.SetBuildingTile(i, j, reader.ReadInt32());
                            map.SetDecorationTile(i, j, reader.ReadInt32());
                        }
                    }
                }
            }
            return map;
        }

        public static void Write(string filename, TileMap map)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                // write tileset
                writer.Write(map.TileSet.TextureName);
                // write parameters
                writer.Write(map.TileSet.TilesWide); // tiles wide
                writer.Write(map.TileSet.TilesHigh); // tiles high
                writer.Write(map.TileSet.TileWidth); // tile width
                writer.Write(map.TileSet.TileHeight); // tile height
                writer.Write(map.MapWidth); // map width
                writer.Write(map.MapHeight); // map height
                // write tile info
                for (int y = 0; y < map.MapHeight; y++)
                {
                    for (int x = 0; x < map.MapWidth; x++)
                    {
                        writer.Write(map.GetGroundTile(x, y));
                        writer.Write(map.GetEdgeTile(x, y));
                        writer.Write(map.GetBuildingTile(x, y));
                        writer.Write(map.GetDecorationTile(x, y));
                    }
                }
            }
        }
    }
}
