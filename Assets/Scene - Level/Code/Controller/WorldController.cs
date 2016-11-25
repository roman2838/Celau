using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldController : MonoBehaviour {
    public bool updateneeded = false;
    public GameObject BGTile;                                                   //Backgroundtile Prefab
    public GameObject[] ATiles;                                                 //ActiveTile Prefabs
    public bool locked = false;                                                 // Accepts Input? Yes or no!
    public static WorldController Instance { get; protected set; }              //Make this WorldController instance publicly available

    private TileGenerator map;                                                  //Create the map
    private List<ActiveTile> activetiles = new List<ActiveTile>();              //List of active Tiles
    private List<GameObject> sprites = new List<GameObject>();                  // Manage Sprites
    public delegate void Del();
    public int[] moves;
    public Del cdqueue;                                                         //Creation/Destruction queue
    public LevelController lvl;
    public UIController UI;
    public ActiveTile.type selectedtile;                                               // Current selection from supply


    void Start() {
        UI = GameObject.Find("UI").GetComponent<UIController>();
        
        Physics.gravity = new Vector3(0f, 0f, -9.81f);                          // Set Gravity in z-direction
        // Make this accessible via WorldController.Instance
        if (Instance != null)
        {
            Debug.LogError("There should never be two world controllers.");
        }
        Instance = this;                                                        // instantiate Worldcontroller
        Debug.Log(PlayerPrefs.GetString("currlvl"));
        lvl = new LevelController(PlayerPrefs.GetString("currlvl"));
        map = new TileGenerator(lvl.Width, lvl.Height);                        // Generate map with Weight/Height given by the Levelcontroller
        //if (lvl == null)
        //    Debug.LogError("Level controller missing!");
        //Debug.Log("Created TileGenerator");
        // Set Cameraposition to center
        Camera.main.transform.position = new Vector3(lvl.Height / 2, lvl.Width / 2, Camera.main.transform.position.z);
        // Draw the playing field
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                Tile tile = map.GetTileAt(x, y);
                tile.Sprite = (GameObject)Instantiate(BGTile, new Vector3(x, y, 0f), Quaternion.identity);
            }
        }
        // Place initial blocks
        GenerateLevel();
        if (moves[(int)selectedtile] == 0)
            SwitchSelectedTile();
    }

    // Called on destraction of the Object
    void OnDestroy()
    {
        Destroy(Instance);                                                      // Destroy the instance of the world controller.
    }

    private IEnumerator RestartRoutine()
    {
        List<ActiveTile> tmp = new List<ActiveTile>(activetiles);                       // Generate temporary list for foreach, since CheckNeighbors modifies activetiles
        updateneeded = false;
        foreach (var tile in tmp)
        {
            cdqueue += GetTileAt(tile.X, tile.Y).DestroyChild;
        }
        cdqueue();
        yield return new WaitForSeconds(.15f);
        cdqueue = () => { };
        updateneeded = false;
        locked = false;
        UI.SetWLStatus();
        GenerateLevel();
        if (moves[(int)selectedtile] == 0)
            SwitchSelectedTile();

    }

    public void Restart()
    {
        StartCoroutine(Instance.RestartRoutine());
    }

    private void GenerateLevel()
    {
        if(lvl.leveldata != null)
            foreach (var tile in lvl.leveldata)
            {
 //               Debug.Log("Levelcontroller creates Child at (" + tile.x + "," + tile.y + ")");
                switch (tile.type)
                {
                    case ActiveTile.type.Black:
                        GetTileAt(tile.x, tile.y).CreateBlackChild();
                        break;
                    case ActiveTile.type.White:
                        GetTileAt(tile.x, tile.y).CreateWhiteChild();
                        break;
                    case ActiveTile.type.Yellow:
                        GetTileAt(tile.x, tile.y).CreateYellowChild();
                        break;
                    case ActiveTile.type.Blue:
                        GetTileAt(tile.x, tile.y).CreateBlueChild();
                        break;
                }
                
            }
        moves = new int[4];
        for (int i = 0; i < lvl.moves.Length; i++)
        {
            moves[i] = lvl.moves[i];
        }
        UI.UpdateMoves(moves);
    }
	
	// Find the tile at position x,y
    public BackgroundTile GetTileAt(int x, int y)
    {
        if (0 <= x && x < map.Width && 0 <= y && y < map.Height)
        {
            return map.GetTileAt(x, y);
        }
        else return null;
    }

    // Register tile to the ActiveTiles list and create the sprite
    public void RegisterTile(ActiveTile atile)
    {
        atile.Sprite = (GameObject)Instantiate(ATiles[(int)atile.Type], new Vector3(atile.X, atile.Y, 3f), Quaternion.identity);
        atile.Sprite.GetComponent<Rigidbody>().AddForce(0, 0, -1000f);
        activetiles.Add(atile);    
    }
    // Remove an active tile from screen, internally and visually
    public void RemoveTile(ActiveTile atile)
    {
        activetiles.Remove(atile);
        Destroy(atile.Sprite.GetComponent<BoxCollider>());
        Rigidbody rb = atile.Sprite.GetComponent<Rigidbody>();
        Vector3 vel = new Vector3(-(5 - atile.X), -(5 - atile.Y), 20f);
        rb.velocity = new Vector3(-(5-atile.X)/(1+Mathf.Pow((5-atile.X),2))*15f, -(5-atile.Y)/(1+Mathf.Pow((5 - atile.Y),2))*15f, 20f);
        rb.rotation = Quaternion.Euler(atile.X*10, atile.Y*10, 0);
        sprites.Add(atile.Sprite);
    //    Destroy(atile.Sprite);
    //    GameObject corpse = (GameObject)Instantiate(ATiles[0], new Vector3(atile.X, atile.Y, .1f), Quaternion.identity);
    //    corpse.GetComponent<Rigidbody>().AddForce(new Vector3(10, 10, 10));
    }
    // Update is called once per frame
    void Update () {
        List<GameObject> tmp = new List<GameObject>(sprites);                       // Generate temporary list for foreach, since CheckNeighbors modifies activetiles
        foreach (var sprite in tmp) {
            if(Mathf.Abs(sprite.transform.position.z) > 5)
            {
                sprites.Remove(sprite);
                Destroy(sprite);
            }
        }
    }

    // Ask any ActiveTile if it satisfies condition for metamorphosis
    public IEnumerator UpdateTiles()
    {
        locked = true;
        updateneeded = true;
        cdqueue = () => { };
        while (updateneeded)
        {
            yield return new WaitForSeconds(.15f);
            List<ActiveTile> tmp = new List<ActiveTile>(activetiles);                       // Generate temporary list for foreach, since CheckNeighbors modifies activetiles
            updateneeded = false;
            foreach (var tile in tmp)
            {
                tile.CheckNeighbors();
            }
            cdqueue();
            yield return new WaitForSeconds(.15f);
            cdqueue = () => { };
        }
        //if(moves[0] != 0)
        //    locked = false;
        CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        int totalmoves = 0;
        foreach (int i in moves)
            totalmoves += i;
        UI.UpdateMoves(moves);
        if (activetiles.Count == 0)
        {
            UI.SetWLStatus(true, activetiles.Count);
        }
        else if (totalmoves == 0)
            UI.SetWLStatus(false, activetiles.Count);
        else locked = false;
    }
    

    public void OnAction(int x, int y)
    {
        //Debug.Log("Moves for " + selectedtile + ":" + (moves[(int)selectedtile]-1));
        BackgroundTile tile = GetTileAt(x,y);
        if ((tile != null) && (tile.GetChild() == null) && moves[(int)selectedtile] > 0)
        {
            switch (selectedtile)
            {
                case ActiveTile.type.Black:                 
                        tile.CreateBlackChild();
                        WorldController.Instance.moves[(int)ActiveTile.type.Black]--;
                        StartCoroutine(WorldController.Instance.UpdateTiles());
                    break;
                case ActiveTile.type.White:
                    tile.CreateWhiteChild();
                    WorldController.Instance.moves[(int)ActiveTile.type.White]--;
                    StartCoroutine(WorldController.Instance.UpdateTiles());
                    break;
                case ActiveTile.type.Yellow:
                    tile.CreateYellowChild();
                    WorldController.Instance.moves[(int)ActiveTile.type.Yellow]--;
                    StartCoroutine(WorldController.Instance.UpdateTiles());
                    break;
            }
        }
    }

    public void SwitchSelectedTile()
    {
        int i = 0;
        while (++i <= moves.Length)
        {
            if (moves[(((int)selectedtile + i) % moves.Length)] != 0)
            {
                selectedtile = (ActiveTile.type)(((int)selectedtile + i) % moves.Length);
                UI.UpdateHighlight(selectedtile);
                return;
            }
        }
    }
}
