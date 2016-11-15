using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Levelselection : MonoBehaviour {
    List<string> lvlnames;
    public GameObject buttonprefab;
    public GameObject canvas;
	// Use this for initialization
	void Start () {
        string filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Levels");
        filepath = System.IO.Path.Combine(filepath, "Basic.xml");
        string lvlsxml = System.IO.File.ReadAllText(filepath);
        // Setup XML Reader
        System.IO.TextReader treader = new System.IO.StringReader(lvlsxml);
        XmlTextReader xreader = new XmlTextReader(treader);

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
            Debug.Log(i);
            GameObject go = (GameObject)Instantiate(buttonprefab);
            go.transform.SetParent(canvas.transform);
            go.name = "Select " + s;
            go.transform.GetComponentInChildren<Text>().text = s;
            Button b = go.GetComponent<Button>();
            go.transform.position = new Vector3(80, 15+i, 0);
            i += 30;
                //        Button b = GetComponent<Button>(go);
            b.onClick.AddListener(delegate { StartLevel(go.transform.GetComponentInChildren<Text>().text); });

                
            }
        }
    }
    public void StartLevel(string s)
    {
        Debug.Log(s);
        PlayerPrefs.SetString("currlvl", s);
        Debug.Log("Opening Level:" + PlayerPrefs.GetString("currlvl"));
        SceneManager.LoadScene("Level");
    }


    // Update is called once per frame
    void Update () {
	
	}

    public void GoToMenu()
    {

    }
}

