using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour
{
    public GameObject hover;
    private LevelController lvl;
    public Material[] materials;
    public Renderer rend;
    
    //    public Object marker;

    // The world-position of the mouse last frame.
    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    // Use this for initialization

    void Start()
    {
        
        hover = (GameObject)Instantiate(hover);
        rend = hover.GetComponent<Renderer>();
        if (WorldController.Instance != null)
            if(WorldController.Instance.lvl != null)
                lvl = WorldController.Instance.lvl;
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (lvl == null)
            lvl = WorldController.Instance.lvl;
        //       currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        currFramePosition.z = .5f;
        // Do a raycast to find position on field
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        currFramePosition = ray.origin - (ray.origin.z / ray.direction.z) * ray.direction;
        currFramePosition.z = .5f;

        UpdateCursor();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = .5f;
        // Press left mouse button
        // TODO: Probably move this code to another controller to enable different interfaces
        if (Input.GetMouseButtonDown(0))
        {
            if (WorldController.Instance.locked == false)
            {
                Vector3 Absoluteposition = new Vector3(Mathf.Round(currFramePosition.x), Mathf.Round(currFramePosition.y), .5f);
                WorldController.Instance.OnAction((int)Absoluteposition.x, (int)Absoluteposition.y);
            }
        }
        if (Input.GetMouseButtonDown(1))
            WorldController.Instance.SwitchSelectedTile();
    }

    void UpdateCursor()
    {
         hover.transform.position = new Vector3(Mathf.Round(currFramePosition.x), Mathf.Round(currFramePosition.y), .5f);
        if (WorldController.Instance.locked)
        {
            //            hover.GetComponent<MeshRenderer>().material.color = Color.red;
            rend.sharedMaterial = materials[0];
        }
        else
        {
            if (WorldController.Instance.selectedtile == ActiveTile.type.Black)
                //hover.GetComponent<MeshRenderer>().material.color = Color.green;
                rend.sharedMaterial = materials[1];
            else if (WorldController.Instance.selectedtile == ActiveTile.type.White)
                rend.sharedMaterial = materials[2];
        }  
    }
}