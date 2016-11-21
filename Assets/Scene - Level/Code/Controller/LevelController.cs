﻿using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class LevelController{

    public int Height;
    public int Width;
    //public int Tiles;
    public LevelData[] leveldata;
    public int[] moves;
//    public enum type { Black, Yellow, White };
    //public int[] Papapapa;

    public LevelController(string lvlid)
    {
        string lvlsxml;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            TextAsset myXmlAsset = Resources.Load<TextAsset>("Basic");
            Debug.Log(myXmlAsset);
            lvlsxml = myXmlAsset.text;

        }
        else
        {
            // Find Levels XML file in filesystem
            string filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Levels");
            filepath = System.IO.Path.Combine(filepath, "Basic.xml");
            lvlsxml = System.IO.File.ReadAllText(filepath);
        }
        // Setup XML Reader
        System.IO.TextReader treader = new System.IO.StringReader(lvlsxml);
        XmlTextReader xreader = new XmlTextReader(treader);
        // Read Level Data
        do
        {
            xreader.ReadToFollowing("Level");
        } while ((xreader.GetAttribute("lvlID") != lvlid) && (xreader.EOF == false));

        if (xreader.GetAttribute("lvlID") == lvlid)
        {
            Height = System.Int32.Parse(xreader.GetAttribute("sizey"));
            Width = System.Int32.Parse(xreader.GetAttribute("sizex"));
            XmlReader reader = xreader.ReadSubtree();
            while (reader.Read())
            {
                if (reader.Name == "Supply" && reader.IsStartElement())
                    GenerateSupplyData(reader.ReadSubtree());
                if (reader.Name == "Board" && reader.IsStartElement())
                    GenerateBoardData(reader.ReadSubtree());
            }
        }
        else
        {
            Height = 10;
            Width = 10;
            moves = new int[] { 99, 99 };
        }
    }

    private void GenerateSupplyData(XmlReader xreader)
    {
        Dictionary<ActiveTile.type, int> supply = new Dictionary<ActiveTile.type, int>();
    //Debug.Log("SupplyData");
    while (xreader.Read())
        {
        if(xreader.Name == "tiles")
            {
                if (xreader.GetAttribute("type") == "Black")
                    supply.Add(ActiveTile.type.Black, System.Int32.Parse(xreader.GetAttribute("amount")));

                else if(xreader.GetAttribute("type") == "Yellow")
                    supply.Add(ActiveTile.type.Yellow, System.Int32.Parse(xreader.GetAttribute("amount")));

                else if (xreader.GetAttribute("type") == "White")
                    supply.Add(ActiveTile.type.White, System.Int32.Parse(xreader.GetAttribute("amount")));
            }
            
        }
        moves = new int[3] { 0, 0, 0 };

        if (supply.ContainsKey(ActiveTile.type.Black))
            moves[0] = supply[ActiveTile.type.Black];
        if (supply.ContainsKey(ActiveTile.type.Yellow))
            moves[1] = supply[ActiveTile.type.Yellow];
        if (supply.ContainsKey(ActiveTile.type.White))
            moves[2] = supply[ActiveTile.type.White];
    }

    private void GenerateBoardData(XmlReader xreader)
    {
        List<LevelData> tmp = new List<LevelData>();
        while (xreader.Read())
        {
            if (xreader.Name == "tile")
            {
                tmp.Add(new LevelData(System.Int32.Parse(xreader.GetAttribute("x")), System.Int32.Parse(xreader.GetAttribute("y")), xreader.GetAttribute("type")));
            }

        }
        leveldata = tmp.ToArray();
    }


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

