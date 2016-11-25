using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActiveTile : Tile
{
    public enum type { Black, White, Yellow, Blue};
    private BackgroundTile parent;
    public type Type;

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

    public ActiveTile(BackgroundTile Parent, int x, int y, type Type)
    {
        X = x;
        Y = y;
        parent = Parent;
        sprite = Sprite;
        this.Type = Type;
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
        switch (Type)
        {
            // Game Logic for black
            case ActiveTile.type.Black:
                if (parent.GetNeighbor("East") != null && parent.GetNeighbor("East").GetChild() != null)
                {
                    switch (parent.GetNeighbor("East").GetChild().Type)
                    {
                        // If eastern neighbor is black
                        case type.Black:

                            WorldController.Instance.cdqueue += parent.GetNeighbor("East").DestroyChild;
                            WorldController.Instance.cdqueue += parent.DestroyChild;
                            if (parent.GetNeighbor("North") != null)
                            {
                                if(parent.GetNeighbor("North").GetChild() == null)
                                    WorldController.Instance.cdqueue += parent.GetNeighbor("North").CreateBlackChild;
                                if (parent.GetNeighbor("North").GetNeighbor("North") != null && parent.GetNeighbor("North").GetNeighbor("North").GetChild() == null)
                                    WorldController.Instance.cdqueue += parent.GetNeighbor("North").GetNeighbor("North").CreateBlackChild;
                            }
                            WorldController.Instance.updateneeded = true;
                            break;
                        // If eastern neighbor is white
                        case type.White:
                            WorldController.Instance.cdqueue += parent.DestroyChild;
                            if(parent.GetNeighbor("West") != null && parent.GetNeighbor("West").GetChild() == null)
                                WorldController.Instance.cdqueue += parent.GetNeighbor("West").CreateBlackChild;
                            WorldController.Instance.updateneeded = true;
                            break;
                    }
                }
                // If western neighbor is white
                if(parent.GetNeighbor("West")!= null && parent.GetNeighbor("West").GetChild() != null)
                    if(parent.GetNeighbor("West").GetChild().Type == type.White)
                    {
                        WorldController.Instance.cdqueue += parent.DestroyChild;
                        if (parent.GetNeighbor("East") != null && parent.GetNeighbor("East").GetChild() == null)
                            WorldController.Instance.cdqueue += parent.GetNeighbor("East").CreateBlackChild;
                        WorldController.Instance.updateneeded = true;
                    }
                break;
            // Gamelogic for white tiles
            case type.White:
                // Has western neighbor?
                if (parent.GetNeighbor("West") != null && parent.GetNeighbor("West").GetChild() != null)
                {
                    switch (parent.GetNeighbor("West").GetChild().Type)
                    {
                        // If western neighbor is white
                        case type.White:
                            WorldController.Instance.cdqueue += parent.GetNeighbor("West").DestroyChild;
                            WorldController.Instance.cdqueue += parent.DestroyChild;
                            if (parent.GetNeighbor("North") != null)
                            {
                                if(parent.GetNeighbor("North").GetChild() == null)
                                    WorldController.Instance.cdqueue += parent.GetNeighbor("North").CreateWhiteChild;
                                if (parent.GetNeighbor("North").GetNeighbor("North") != null && parent.GetNeighbor("North").GetNeighbor("North").GetChild() == null)
                                    WorldController.Instance.cdqueue += parent.GetNeighbor("North").GetNeighbor("North").CreateWhiteChild;
                            }
                            WorldController.Instance.updateneeded = true;
                            break;
                       // If western neighbor is black
                       case type.Black:
                            WorldController.Instance.cdqueue += parent.DestroyChild;
                            if (parent.GetNeighbor("East") != null && parent.GetNeighbor("East").GetChild() == null)
                                WorldController.Instance.cdqueue += parent.GetNeighbor("East").CreateWhiteChild;
                            WorldController.Instance.updateneeded = true;
                            break;

                    }
                // Has eastern neighbor?
                }
                if (parent.GetNeighbor("East") != null && parent.GetNeighbor("East").GetChild() != null)
                    // If eastern neighbor is black
                    if (parent.GetNeighbor("East").GetChild().Type == type.Black)
                    {
                        WorldController.Instance.cdqueue += parent.DestroyChild;
                        if (parent.GetNeighbor("West") != null && parent.GetNeighbor("West").GetChild() == null)
                            WorldController.Instance.cdqueue += parent.GetNeighbor("West").CreateWhiteChild;
                        WorldController.Instance.updateneeded = true;
                    }
                break;
            case type.Yellow:
                // Has southern neighbor that is Yellow
                if(parent.GetNeighbor("South") != null)
                    if(parent.GetNeighbor("South").GetChild() != null && parent.GetNeighbor("South").GetChild().Type == type.Yellow)
                    {
                        if (parent.GetNeighbor("North") != null)
                        {
                            if (parent.GetNeighbor("North").GetChild() == null)
                                WorldController.Instance.cdqueue += parent.GetNeighbor("North").CreateYellowChild;
                            if (parent.GetNeighbor("North").GetNeighbor("West") != null)
                                if (parent.GetNeighbor("North").GetNeighbor("West").GetChild() == null)
                                    WorldController.Instance.cdqueue += parent.GetNeighbor("North").GetNeighbor("West").CreateYellowChild;
                        }
                        WorldController.Instance.cdqueue += parent.GetNeighbor("South").DestroyChild;
                        WorldController.Instance.cdqueue += parent.DestroyChild;
                        WorldController.Instance.updateneeded = true;
                    }
                // Has southeastern Yellow neighbor?
                if(parent.GetNeighbor("South") != null && parent.GetNeighbor("South").GetNeighbor("East") != null)
                    if(parent.GetNeighbor("South").GetNeighbor("East").GetChild() != null && parent.GetNeighbor("South").GetNeighbor("East").GetChild().Type == type.Black)
                    {
                        if (parent.GetNeighbor("South").GetNeighbor("West") != null)
                            if(parent.GetNeighbor("South").GetNeighbor("West").GetChild() == null)
                                WorldController.Instance.cdqueue += parent.GetNeighbor("South").GetNeighbor("West").CreateYellowChild;
                        WorldController.Instance.cdqueue += parent.GetNeighbor("South").GetNeighbor("East").DestroyChild;
                        WorldController.Instance.cdqueue += parent.DestroyChild;
                        WorldController.Instance.cdqueue += parent.CreateBlackChild;
                        WorldController.Instance.updateneeded = true;

                    }

                //Has Southwestern white neighbor
                if (parent.GetNeighbor("South") != null && parent.GetNeighbor("South").GetNeighbor("West") != null)
                    if (parent.GetNeighbor("South").GetNeighbor("West").GetChild() != null && parent.GetNeighbor("South").GetNeighbor("West").GetChild().Type == type.White)
                    {
                        if (parent.GetNeighbor("South").GetNeighbor("East") != null)
                            if (parent.GetNeighbor("South").GetNeighbor("East").GetChild() == null)
                                WorldController.Instance.cdqueue += parent.GetNeighbor("South").GetNeighbor("East").CreateYellowChild;
                        WorldController.Instance.cdqueue += parent.GetNeighbor("South").GetNeighbor("West").DestroyChild;
                        WorldController.Instance.cdqueue += parent.DestroyChild;
                        WorldController.Instance.cdqueue += parent.CreateWhiteChild;
                        WorldController.Instance.updateneeded = true;
                    }

                // OLD YELLOW LOGIC
                //if(parent.GetNeighbor("East") != null && parent.GetNeighbor("East").GetNeighbor("North") != null)
                //    if(parent.GetNeighbor("East").GetNeighbor("North").GetChild() != null && parent.GetNeighbor("East").GetNeighbor("North").GetChild().Type == type.Yellow)
                //    {
                //        WorldController.Instance.cdqueue += parent.GetNeighbor("East").GetNeighbor("North").DestroyChild;
                //        if (parent.GetNeighbor("West") != null && parent.GetNeighbor("West").GetNeighbor("North") != null)
                //            if(parent.GetNeighbor("West").GetNeighbor("North").GetChild() == null)
                //                WorldController.Instance.cdqueue += parent.GetNeighbor("West").GetNeighbor("North").CreateYellowChild;
                //        WorldController.Instance.updateneeded = true;
                //    }
                //if(parent.GetNeighbor("North") != null && parent.GetNeighbor("North").GetChild() != null)
                //    if(parent.GetNeighbor("North").GetChild().Type == type.Yellow)
                //    {
                //        WorldController.Instance.cdqueue += parent.GetNeighbor("North").DestroyChild;
                //        WorldController.Instance.cdqueue += parent.DestroyChild;
                //        WorldController.Instance.updateneeded = true;
                //        if (parent.GetNeighbor("South") != null)
                //        {
                //            if (parent.GetNeighbor("South").GetChild() == null)
                //                WorldController.Instance.cdqueue += parent.GetNeighbor("South").CreateYellowChild;
                //            if (parent.GetNeighbor("South").GetNeighbor("West") != null)
                //                if (parent.GetNeighbor("South").GetNeighbor("West").GetChild() == null)
                //                    WorldController.Instance.cdqueue += parent.GetNeighbor("South").GetNeighbor("West").CreateYellowChild;
                //        }

                //    }
                break;
        }
    }
}