using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    private GameObject LvlName;
    private GameObject MoveCounter;
    private GameObject WLStatus;
    private GameObject Highlight;
	// Use this for initialization
	void Start () {
        LvlName = GameObject.Find("UI_LevelName");
        MoveCounter = GameObject.Find("UI_Moves");
        WLStatus = GameObject.Find("UI_WLStatus");
        Highlight = GameObject.Find("UI_Highlight");
        LvlName.GetComponent<Text>().text = "Level: " + PlayerPrefs.GetString("currlvl");
        SetWLStatus();
        //GetComponents<Text>(LvlName).guiText = PlayerPrefs.GetString("currlvl");

    }
    
    // Update is called once per frame
    void Update () {
	
	}

    public void SetWLStatus(bool won, int remaining)
    {
        if (won)
        {
            WLStatus.GetComponent<Text>().text = "Congratulations, you won!";
            WLStatus.GetComponent<Text>().color = new Color(0f, .7f, 0f);
        }
        if (won == false)
        {
            WLStatus.GetComponent<Text>().text = "You lost, " + remaining + " tiles remaining!";
            WLStatus.GetComponent<Text>().color = new Color(.7f, 0f, 0f);
        }
    }

    public void SetWLStatus()
    {
        WLStatus.GetComponent<Text>().color = Color.black;
        WLStatus.GetComponent<Text>().text = "Clear the board!\nRMB to switch tiles";

    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void UpdateMoves(int[] i)
    {

        MoveCounter.GetComponent<Text>().text = "Moves: \n"+
                                                "Black: " + i[0].ToString() + "\n"+
                                                "<color=yellow>Yellow: " + i[1].ToString() + "</color>\n"+
                                                "<color=white>White: " + i[2].ToString() + "</color>";
    }

    public void UpdateHighlight(ActiveTile.type selection)
    {
        //Vector3 pos = Highlight.GetComponent<RectTransform>().position;
        //pos.y -=  ((int)selection-(int)oldselection)*15;
        RectTransform parent = Highlight.transform.parent.gameObject.GetComponent<RectTransform>();
        Vector3[] corner = new Vector3[4];
        parent.GetWorldCorners(corner);
        //foreach (Vector3 i in corner)
        //    Debug.Log(i);
        Vector3 pos = corner[1] + new Vector3(5, -16 - (int)selection * 16, 0);


        //pos.y = -15 - (int)selection * 15;
        Highlight.GetComponent<RectTransform>().position = pos;
    }
}
