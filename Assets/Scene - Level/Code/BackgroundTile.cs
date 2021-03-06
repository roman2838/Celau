﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundTile : Tile {
    protected Dictionary<string, BackgroundTile> neighbors =
        new Dictionary<string, BackgroundTile>();

    bool hasChild = false;
    ActiveTile Child;

    public BackgroundTile()
    {

    }

    public BackgroundTile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public BackgroundTile(int x, int y, GameObject Sprite)
    {
        X = x;
        Y = y;
        sprite = Sprite;
        
    }

    public void AddNeighbor(string label, BackgroundTile tile)
    {
        neighbors.Add(label,tile);
    }

    public BackgroundTile GetNeighbor(string label)
    {
        try
        {
            return neighbors[label];
        }
        catch(KeyNotFoundException)
        {
            return null;
        }
    }
    public void CreateChild()
    {
//        Debug.Log("Try to create Child at (" + X + "," + Y + ")");
        if (Child != null)
        {
            return;
        }
        else
        {
//            Debug.Log("Created child at (" + X + "," + Y + ")");
            Child = new ActiveTile(this, X, Y);
            WorldController.Instance.RegisterTile(Child);
        }
    }
    public void DestroyChild()
    {
        if (Child == null)
        {
  //          Debug.Log("Destroy not existing child, this should never happen");
        }
        else
        {
//            Debug.Log("Destroy Child at (" + X + "," + Y + ")");
            WorldController.Instance.RemoveTile(Child);
            Child = null;
        }
    }
    public ActiveTile GetChild()
    {
        return Child;
    }
    
}
