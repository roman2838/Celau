using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class LevelController{

    public int Height;
    public int Width;
    //public int Tiles;
    public LevelData[] leveldata;
    public int[] moves;
    public enum type { Black, Yellow };
    //public int[] Papapapa;

    public LevelController(string lvlid)
    {
        // Find Levels XML file in filesystem
        string filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Levels");
        filepath = System.IO.Path.Combine(filepath, "Basic.xml");
        string lvlsxml = System.IO.File.ReadAllText(filepath);
        // Setup XML Reader
        System.IO.TextReader treader = new System.IO.StringReader(lvlsxml);
        XmlTextReader xreader = new XmlTextReader(treader);

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
        Dictionary<type, int> supply = new Dictionary<type, int>();
    //Debug.Log("SupplyData");
    while (xreader.Read())
        {
        if(xreader.Name == "tiles")
            {
                if (xreader.GetAttribute("type") == "Black")
                {
                    supply.Add(type.Black, System.Int32.Parse(xreader.GetAttribute("amount")));

                }
                else if(xreader.GetAttribute("type") == "Yellow")
                    supply.Add(type.Yellow, System.Int32.Parse(xreader.GetAttribute("amount")));
            }
            
        }
        moves = new int[2];
        if (supply.ContainsKey(type.Black))
            moves[0] = supply[type.Black];
        if (supply.ContainsKey(type.Yellow))
            moves[1] = supply[type.Yellow];
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

