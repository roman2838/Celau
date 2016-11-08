using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActiveTile : Tile
{

    private BackgroundTile parent;

    public ActiveTile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public ActiveTile(BackgroundTile Parent, int x, int y)
    {
        X = x;
        Y = y;
        parent = Parent;
    }

    public ActiveTile(BackgroundTile Parent, int x, int y, GameObject Sprite)
    {
        X = x;
        Y = y;
        parent = Parent;
        sprite = Sprite;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckNeighbors()
    {
        // Game Logic for black, if east, delete east and creat north.
        if (parent.GetNeighbor("East") != null)
            if (parent.GetNeighbor("East").GetChild() != null)
            {
//                Debug.Log("Found child east of (" + X + "," + Y + ")");
                WorldController.Instance.cdqueue += parent.GetNeighbor("East").DestroyChild;
                if (parent.GetNeighbor("North") != null)
                    WorldController.Instance.cdqueue +=  parent.GetNeighbor("North").CreateChild;
                WorldController.Instance.updateneeded = true;
            }

    }
}