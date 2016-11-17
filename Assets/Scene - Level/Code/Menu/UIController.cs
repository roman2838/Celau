using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    private GameObject LvlName;
    private GameObject MoveCounter;
    private GameObject WLStatus;
	// Use this for initialization
	void Start () {
        LvlName = GameObject.Find("UI_LevelName");
        MoveCounter = GameObject.Find("UI_Moves");
        WLStatus = GameObject.Find("UI_WLStatus");
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
            WLStatus.GetComponent<Text>().text = "You lost, " + remaining + " tiles remaining!\nPress RMB to restart!";
            WLStatus.GetComponent<Text>().color = new Color(.7f, 0f, 0f);
        }
    }

    public void SetWLStatus()
    {
        WLStatus.GetComponent<Text>().color = Color.black;
        WLStatus.GetComponent<Text>().text = "Clear the board!";

    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void UpdateMoves(int i)
    {

        MoveCounter.GetComponent<Text>().text = "Moves: " + i.ToString();
    }
}
