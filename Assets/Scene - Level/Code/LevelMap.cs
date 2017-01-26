using UnityEngine;
using System.Collections.Generic;
using System.Xml;


/// <summary>
/// Generates all maps from XML and gives access to liste of levels via .Maps attribute.
/// </summary>
public class LevelMap {

    public List<Map> Maps;
    private List<string> lvlnames;
    private Map current;

    public LevelMap(string lvlpack = "Basic.xml")
    {
        PlayerPrefs.SetString("LevelFile", lvlpack);
        string filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Levels");
        filepath = System.IO.Path.Combine(filepath, lvlpack);
        string lvlsxml = System.IO.File.ReadAllText(filepath);
        // Setup XML Reader
        System.IO.TextReader treader = new System.IO.StringReader(lvlsxml);
        XmlTextReader xreader = new XmlTextReader(treader);

        //      DestroyAllButtons();
        PopulateWorld(xreader);
    }

    public void PopulateWorld(XmlTextReader xreader)
    {
        Maps = new List<Map>();
        lvlnames = new List<string>();
        do
        {
            xreader.ReadToFollowing("Level");
            lvlnames.Add(xreader.GetAttribute("lvlID"));
        } while (xreader.EOF == false);
        int i = 0;

        foreach (string s in lvlnames)
        {
            if (s != null)
            {
                LevelController lvl = new LevelController(s);
                Map map = new Map(lvl, new Vector3(0, i * 20, 0));
                map.name = s;
                Maps.Add(map);
                Maps[i].GenerateMap();
                i += 1;
            }
                //GameObject go = (GameObject)Instantiate(buttonprefab);
                //go.transform.SetParent(canvas.transform);
                //go.name = "Select " + s;
                //go.transform.GetComponentInChildren<Text>().text = s;
                //Button b = go.GetComponent<Button>();
                //// Choose a color according to lvldata
                //if (s.Contains("(T)"))
                //{
                //    b.image.color = Color.green;// overrideSprite = textures[5];
                //    i = 0;
                //    j -= 1;
                //}
                //else if (lvl.moves[(int)ActiveTile.type.Black] > 0)
                //{
                //    b.image.color = Color.black;//overrideSprite = textures[0];
                //    if (lvl.moves[(int)ActiveTile.type.White] > 0)
                //        b.image.color = Color.grey;
                //}
                //else
                //    b.image.color = Color.white;
                //RectTransform parent = go.transform.parent.gameObject.GetComponent<RectTransform>();
                ////Debug.Log(parent == Highlight.GetComponent<RectTransform>());
                //Vector3[] corner = new Vector3[4];
                //parent.GetWorldCorners(corner);

                //b.transform.position = corner[2] + new Vector3(-j * 180, -(15 + i), 0);
                //i += 30;
                ////Debug.Log(Screen.height);
                //if (i > Screen.height)
                //{
                //    //    Debug.Log(i * 15 + ":" +Screen.height);
                //    ++j;
                //    i = 0;
                //}

                //b.onClick.AddListener(delegate { StartLevel(go.transform.GetComponentInChildren<Text>().text); });
                //b.interactable = true;
                //buttons.Add(go);
        }
        Debug.Log(Maps[0].name);
    }
    public bool SetCurrentMap(string s)
    {
        int i = Maps.FindIndex(x => x.name == s);
        return SetCurrentMap(i);
    }

    public bool SetCurrentMap(int i)
    {
        if(i < Maps.Count)
        {
            current = Maps[i];
            Camera.main.transform.position = new Vector3(current.GetHeight() / 2 + current.origin.x, current.GetWidth() / 2 + current.origin.y, Camera.main.transform.position.z);
            return true;
        }
        else
        {
            Debug.LogError("Index out of range\n Index: " + i + "\n Size of Maps: " + Maps.Count);
            return false;
        }
    }

    public ActiveTile.type GetSelected()
    {
        return current.selectedtile;
    }

    public BackgroundTile GetTileAt(int x, int y)
    {
        return current.GetTileAt(x, y);
    }
}