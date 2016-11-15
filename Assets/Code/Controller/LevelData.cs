using UnityEngine;
using System.Collections;

public class LevelData{
    public int x;
    public int y;
    public enum type { Black, Yellow };
    public type tiletype;

    public LevelData(int x, int y, string type)
    {
        this.x = x;
        this.y = y;
        if(type == "Black")
            this.tiletype = LevelData.type.Black;
    }
}

//public class LevelData : MonoBehaviour
//{
//    public int x;
//    public int y;
//    public enum type { Black };
//}