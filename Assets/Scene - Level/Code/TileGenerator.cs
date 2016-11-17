using UnityEngine;
using System.Collections;

public class TileGenerator{
    private int width = 10;
    private int height = 10;
    BackgroundTile[,] map;
    bool refresh = false;

    public int Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public TileGenerator(int w=10,int h=10)
    {
        //       Generate Tiles
        Width = w;
        Height = h;
        map = new BackgroundTile[Width, Height];
        Debug.Log("Created map with dimensions " + Width + "x" + Height);
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                //Debug.Log("Created BGTile at (" + x + "," + y);
                map[x, y] = new BackgroundTile(x,y);
    }
        }
        // Fill Neighbours Array
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x > 0)
                {
                    map[x, y].AddNeighbor("East", map[x - 1, y]);
                }
                if (x < Width-1)
                {
                    map[x, y].AddNeighbor("West", map[x + 1, y]);
                }
                if (y > 0)
                {
                    map[x, y].AddNeighbor("South", map[x, y - 1]);
                }
                if (y < Height-1)
                {
                    map[x, y].AddNeighbor("North", map[x, y + 1]);
                }
            }
        }
    }
    public BackgroundTile GetTileAt(int cx, int cy)
    {
        return map[cx, cy];
    }
}
