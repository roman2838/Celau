using UnityEngine;
using System.Collections;

public class TilePrototype{
    public int x;
    public int y;
//    public enum type { Black, Yellow, White };
//    public type tiletype;
    public ActiveTile.type type;

    public TilePrototype(int x, int y, string type)
    {
        this.x = x;
        this.y = y;
        switch (type)
        {
            case "Black":
                this.type = ActiveTile.type.Black;
                break;
            case "Yellow":
                this.type = ActiveTile.type.Yellow;
                break;
            case "White":
                this.type = ActiveTile.type.White;
                break;
        }
    }
}

//public class LevelData : MonoBehaviour
//{
//    public int x;
//    public int y;
//    public enum type { Black };
//}