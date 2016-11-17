using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {

    private int x;
    private int y;

    protected GameObject sprite;

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }

    public GameObject Sprite
    {
        get
        {
            return sprite;
        }

        set
        {
            sprite = value;
        }
    }

    public Tile()
    {

    }

    public Tile(int cx, int cy)
    {
        x = cx;
        y = cy;
    }

    public Tile(int cx, int cy, GameObject Sprite)
    {
        x = cx;
        y = cy;
        sprite = Sprite;
    }
    public void SayHello()
    {
        Debug.Log("Hello, I'm at (" + X + "," + Y+")");
    }

}
