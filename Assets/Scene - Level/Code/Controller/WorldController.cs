using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// The class every other class should interact with. Should only pass commands to other classes.
/// </summary>
public class WorldController : MonoBehaviour {
    public bool updateneeded = false;
    public GameObject BGTile;                                                   //Backgroundtile Prefab
    public GameObject[] ATiles;                                                 //ActiveTile Prefabs
    public bool locked = false;                                                 // Accepts Input? Yes or no!
    public static WorldController Instance { get; protected set; }              //Make this WorldController instance publicly available

    private List<ActiveTile> activetiles = new List<ActiveTile>();              //List of active Tiles
    private List<GameObject> sprites = new List<GameObject>();                  // Manage Sprites
    public delegate void Del();
    public Del cdqueue;                                                         //Creation/Destruction queue
    public Vector3 Origin;
    public string CrrScene = "LevelSelection";
    private LevelMap Levelmap;

    // NEW CODE USING THE MAP CLASS
    //public static Map crrLevel;


    void Start() {
        //TODO: Remove Physics completely?
        Physics.gravity = new Vector3(0f, 0f, -9.81f);                          // Set Gravity in z-direction

        // Make this accessible via WorldController.Instance
        if (Instance != null)
        {
            Debug.LogError("There should never be two world controllers.");
        }
        Instance = this;                                                        // instantiate Worldcontroller


        /**********************************
        / Old Code using the map class    *        
        /**********************************/
        //lvl = new LevelController(PlayerPrefs.GetString("currlvl"));
        //map = new Map(lvl, Origin);
        //map.GenerateMap();
        //Debug.Log("We have a map now!");
        /************************************
        / New Code using the LevelMap class *
        /************************************/
        Levelmap = new LevelMap();
        Levelmap.SetCurrentMap(0);
    }

    // Called on destraction of the Object
    void OnDestroy()
    {
        Destroy(Instance);                                                      // Destroy the instance of the world controller.
    }


    // TODO: Move this into LevelMap?!
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
//        crrLevel.UI.SetWLStatus();
        //crrLevel.GenerateLevel();
//        if (crrLevel.moves[(int)crrLevel.selectedtile] == 0)
//            SwitchSelectedTile();

    }

    public void Restart()
    {
        StartCoroutine(Instance.RestartRoutine());
    }
    /*
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
	*/
	// Find the tile at position x,y
    public BackgroundTile GetTileAt(int x, int y)
    {
        return Levelmap.GetTileAt(x, y);
        
    }

    // Register tile to the ActiveTiles list and create the sprite
    public void RegisterTile(ActiveTile atile)
    {
        if (crrLevel == null)
        {
            Debug.Log("Map not found");
            if (crrLevel.origin == null)
                Debug.Log("No Origin");
        }
        
        atile.Sprite = (GameObject)Instantiate(ATiles[(int)atile.Type],crrLevel.origin +  new Vector3(atile.X, atile.Y, 3f), Quaternion.identity);
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
        foreach (int i in crrLevel.moves)
            totalmoves += i;
        crrLevel.UI.UpdateMoves(crrLevel.moves);
        if (activetiles.Count == 0)
        {
            crrLevel.UI.SetWLStatus(true, activetiles.Count);
        }
        else if (totalmoves == 0)
            crrLevel.UI.SetWLStatus(false, activetiles.Count);
        else locked = false;
    }
    

    public void OnAction(int x, int y)
    {
        int localX = x - (int)crrLevel.origin.x;
        int localY = y - (int)crrLevel.origin.y;
        //Debug.Log("Moves for " + selectedtile + ":" + (moves[(int)selectedtile]-1));
        BackgroundTile tile = GetTileAt(localX,localY);
        if ((tile != null) && (tile.GetChild() == null) && crrLevel.moves[(int)crrLevel.selectedtile] > 0)
        {
            switch (crrLevel.selectedtile)
            {
                case ActiveTile.type.Black:                 
                        tile.CreateBlackChild();
                    crrLevel.moves[(int)ActiveTile.type.Black]--;
                        StartCoroutine(WorldController.Instance.UpdateTiles());
                    break;
                case ActiveTile.type.White:
                    tile.CreateWhiteChild();
                    crrLevel.moves[(int)ActiveTile.type.White]--;
                    StartCoroutine(WorldController.Instance.UpdateTiles());
                    break;
                case ActiveTile.type.Yellow:
                    tile.CreateYellowChild();
                    crrLevel.moves[(int)ActiveTile.type.Yellow]--;
                    StartCoroutine(WorldController.Instance.UpdateTiles());
                    break;
            }
        }
    }

    public void SwitchSelectedTile()
    {
        int i = 0;
        while (++i <= crrLevel.moves.Length)
        {
            if (crrLevel.moves[(((int)crrLevel.selectedtile + i) % crrLevel.moves.Length)] != 0)
            {
                crrLevel.selectedtile = (ActiveTile.type)(((int)crrLevel.selectedtile + i) % crrLevel.moves.Length);
                crrLevel.UI.UpdateHighlight(crrLevel.selectedtile);
                return;
            }
        }
    }
    public static ActiveTile.type GetSelectedTile()
    {
        return Levelmap.GetSelected();
    }
}
