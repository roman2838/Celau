using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class LevelMap {

    public Dictionary<string, Map> Maps;
    private List<string> lvlnames;

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
        Maps = new Dictionary<string, Map>();
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
                Maps.Add(s, map);
                Maps[s].GenerateMap();
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
        WorldController.Instance.Maps = Maps;
    }
}