using UnityEngine;
using System.Collections;

public class Map
{
    private TileGenerator map;
    private LevelController lvl;
    private Vector3 origin;
    public ActiveTile.type selectedtile;                                        // Current selection from supply
    private int[] moves;
    UIController UI;

    Map(LevelController lvl, Vector3 origin)
    {
        UI = GameObject.Find("UI").GetComponent<UIController>();
        this.map = new TileGenerator(lvl.Width, lvl.Height);                        // Generate map with Weight/Height given by the Levelcontroller
        this.lvl = lvl;
        this.origin = origin;
        //if (lvl == null)
        //    Debug.LogError("Level controller missing!");
        //Debug.Log("Created TileGenerator");
        // Set Cameraposition to center
        //Camera.main.transform.position = new Vector3(lvl.Height / 2, lvl.Width / 2, Camera.main.transform.position.z);
        // Draw the playing field
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                Tile tile = map.GetTileAt(x, y);
                tile.Sprite = (GameObject)GameObject.Instantiate(WorldController.Instance.BGTile, new Vector3(x, y, 0f) + origin, Quaternion.identity);
            }
        }
        // Place initial blocks
        GenerateLevel();
        if (moves[(int)selectedtile] == 0)
            SwitchSelectedTile();
    }

    private void GenerateLevel()
    {
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
        moves = new int[4];
        for (int i = 0; i < lvl.moves.Length; i++)
        {
            moves[i] = lvl.moves[i];
        }
        // TODO: Make UpdateMoves Available
        UI.UpdateMoves(moves);
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