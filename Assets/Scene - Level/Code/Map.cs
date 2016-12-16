using UnityEngine;
using System.Collections;

public class Map
{
    private TileGenerator map;
    private LevelController lvl;
    public Vector3 origin;
    public ActiveTile.type selectedtile;                                        // Current selection from supply
    public int[] moves;
    public UIController UI;

   public  Map(LevelController lvl, Vector3 origin)
    {
        UI = GameObject.Find("UI").GetComponent<UIController>();
        this.map = new TileGenerator(lvl.Width, lvl.Height);                        // Generate map with Weight/Height given by the Levelcontroller
        this.lvl = lvl;
        this.origin = origin;
    }

    public void GenerateMap()
    {
        // Generate Background Tiles
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                Tile tile = map.GetTileAt(x, y);
                tile.Sprite = (GameObject)GameObject.Instantiate(WorldController.Instance.BGTile, new Vector3(x, y, 0f) + origin, Quaternion.identity);
            }
        }
        // Fill in ActiveTile from LevelData
        GenerateLevel();
        // Set Cameraposition to center
        Camera.main.transform.position = new Vector3(lvl.Height / 2 + origin.x , lvl.Width / 2 + origin.y , Camera.main.transform.position.z);

    }

    public void GenerateLevel()
    {
        
        
        //if (lvl == null)
        //    Debug.LogError("Level controller missing!");
        //Debug.Log("Created TileGenerator");
        // Draw the playing field

        // Generate Background Tiles

        // Fill in ActiveTile from leveldata
        if (lvl.leveldata != null)
            foreach (var tile in lvl.leveldata)
            {
                //               Debug.Log("Levelcontroller creates Child at (" + tile.x + "," + tile.y + ")");
                switch (tile.type)
                {
                    case ActiveTile.type.Black:
                        GetTileAt(tile.x, tile.y).CreateBlackChild();
                        break;
                    case ActiveTile.type.White:
                        GetTileAt(tile.x, tile.y).CreateWhiteChild();
                        break;
                    case ActiveTile.type.Yellow:
                        GetTileAt(tile.x, tile.y).CreateYellowChild();
                        break;
                    case ActiveTile.type.Blue:
                        GetTileAt(tile.x, tile.y).CreateBlueChild();
                        break;
                }

            }

        // Setup supply
        moves = new int[4];
        for (int i = 0; i < lvl.moves.Length; i++)
        {
            moves[i] = lvl.moves[i];
        }

        // Update UI
        // TODO: Make UpdateMoves Available (???)
        UI.UpdateMoves(moves);

        if (moves[(int)selectedtile] == 0)
            SwitchSelectedTile();
    }
    public BackgroundTile GetTileAt(int x, int y)
    {
        if (0 <= x && x < map.Width && 0 <= y && y < map.Height)
        {
            return map.GetTileAt(x, y);
        }
        else return null;
    }


    public void SwitchSelectedTile()
    {
        int i = 0;
        while (++i <= moves.Length)
        {
            if (moves[(((int)selectedtile + i) % moves.Length)] != 0)
            {
                selectedtile = (ActiveTile.type)(((int)selectedtile + i) % moves.Length);
                UI.UpdateHighlight(selectedtile);
                return;
            }
        }
    }

}