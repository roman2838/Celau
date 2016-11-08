using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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


    void Start() {
        Physics.gravity = new Vector3(0f, 0f, -9.81f);                          // Set Gravity in z-direction
        // Make this accessible via WorldController.Instance
        if (Instance != null)
        {
            Debug.LogError("There should never be two world controllers.");
        }
        Instance = this;                                                        // instantiate Worldcontroller
        map = new TileGenerator(lvl.Width, lvl.Height);                        // Generate map with Weight/Height given by the Levelcontroller
        if (lvl == null)
            Debug.LogError("Level controller missing!");
        Debug.Log("Created TileGenerator");
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
    }

    public IEnumerator Restart()
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
        GenerateLevel();
    }


    private void GenerateLevel()
    {

        foreach (var block in lvl.leveldata)
        {
            Debug.Log("Levelcontroller creates Child at (" + block.x + "," + block.y + ")");
            GetTileAt(block.x - 1, block.y - 1).CreateChild();                  // ATTENTION! COORDINATES CONVERTED -1!
        }
        moves = new int[lvl.moves.Length];
        for (int i = 0; i < lvl.moves.Length; i++)
        {
            moves[i] = lvl.moves[i];
        }
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
        atile.Sprite = (GameObject)Instantiate(ATiles[0], new Vector3(atile.X, atile.Y, 3f), Quaternion.identity);
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
        if(moves[0] != 0)
            locked = false;
        if (CheckWinCondition())
            Debug.Log("You won!");

    }

    public bool CheckWinCondition()
    {
        if(activetiles.Count == 0)
        {
            return true;
        }
        return false;
    }
}
