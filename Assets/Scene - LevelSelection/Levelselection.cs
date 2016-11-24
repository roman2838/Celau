using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Levelselection : MonoBehaviour {

    List<string> lvlnames;
    List<GameObject> buttons;
    public GameObject buttonprefab;
    public GameObject canvas;
    
	// Use this for initialization
	void Start ()
    {
        buttons = new List<GameObject>();
        string lvlsxml;
        string[] lvls;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
        TextAsset myXmlAsset = Resources.Load<TextAsset>("Basic");
        Debug.Log(myXmlAsset);
            lvlsxml = myXmlAsset.text;

        }
        else
        {
            string filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Levels");
            lvls = System.IO.Directory.GetFiles(filepath, "*.xml");
            SelectLevelFile(lvls);
            foreach(string s in lvls)
            {
                Debug.Log(s);
            }
            filepath = System.IO.Path.Combine(filepath, "Basic.xml");
            lvlsxml = System.IO.File.ReadAllText(filepath);
        }
        // Setup XML Reader
        System.IO.TextReader treader = new System.IO.StringReader(lvlsxml);
        XmlTextReader xreader = new XmlTextReader(treader);

//        PopulateMenues(xreader);
    }

    private void SelectLevelFile(string[] lvls)
    {
        int i = 0;
        foreach (string s in lvls)
        {
            if (s != null)
            {
                GameObject go = (GameObject)Instantiate(buttonprefab);
                go.transform.SetParent(canvas.transform);
                string filename = System.IO.Path.GetFileName(s);
                go.name = "Select " + filename;
                go.transform.GetComponentInChildren<Text>().text = filename;
                Button b = go.GetComponent<Button>();
                RectTransform parent = go.transform.parent.gameObject.GetComponent<RectTransform>();
                //Debug.Log(parent == Highlight.GetComponent<RectTransform>());
                Vector3[] corner = new Vector3[4];
                parent.GetWorldCorners(corner);

                b.transform.position = corner[1] + new Vector3(+80, -(15 + i), 0);
                i += 30;
                //Debug.Log(Screen.height);

                b.onClick.AddListener(delegate { SelectLevelPack(go.transform.GetComponentInChildren<Text>().text); });
                b.interactable = true;
                buttons.Add(go);
            }
        }
    }

    private void SelectLevelPack(string s)
    {
        string filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Levels");
        filepath = System.IO.Path.Combine(filepath, "Basic.xml");
        string lvlsxml = System.IO.File.ReadAllText(filepath);
    
        // Setup XML Reader
        System.IO.TextReader treader = new System.IO.StringReader(lvlsxml);
        XmlTextReader xreader = new XmlTextReader(treader);
        foreach(GameObject b in buttons)
        {
            Debug.Log(b);
            Destroy(b);
        }

        PopulateMenues(xreader);
}

public void StartLevel(string s)
    {
        PlayerPrefs.SetString("currlvl", s);
        Debug.Log("Opening Level:" + PlayerPrefs.GetString("currlvl"));
        SceneManager.LoadScene("Level");
    }

    public void PopulateMenues(XmlTextReader xreader)
    {

    lvlnames = new List<string>();

    do
    {
        xreader.ReadToFollowing("Level");
        lvlnames.Add(xreader.GetAttribute("lvlID"));
    } while (xreader.EOF == false);
    int i = 0;
    int j = 4;

    foreach (string s in lvlnames)
    {
        if (s != null)
        {
            LevelController lvl = new LevelController(s);
            GameObject go = (GameObject)Instantiate(buttonprefab);
            go.transform.SetParent(canvas.transform);
            go.name = "Select " + s;
            go.transform.GetComponentInChildren<Text>().text = s;
            Button b = go.GetComponent<Button>();
            // Choose a color according to lvldata
            if (s.Contains("(T)"))
            {
                b.image.color = Color.green;
                i = 0;
                j -= 1;
            }
            else if (lvl.moves[(int)ActiveTile.type.Black] > 0)
            {
                b.image.color = Color.black;
                if (lvl.moves[(int)ActiveTile.type.White] > 0)
                    b.image.color = Color.grey;
            }
            else
                b.image.color = Color.white;
            RectTransform parent = go.transform.parent.gameObject.GetComponent<RectTransform>();
            //Debug.Log(parent == Highlight.GetComponent<RectTransform>());
            Vector3[] corner = new Vector3[4];
            parent.GetWorldCorners(corner);

            b.transform.position = corner[2] + new Vector3(-j * 180, -(15 + i), 0);
            i += 30;
            //Debug.Log(Screen.height);
            if (i > Screen.height)
            {
                //    Debug.Log(i * 15 + ":" +Screen.height);
                ++j;
                i = 0;
            }

            b.onClick.AddListener(delegate { StartLevel(go.transform.GetComponentInChildren<Text>().text); });
            b.interactable = true;
        }
    }
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void GoToMenu()
    {

    }
}

