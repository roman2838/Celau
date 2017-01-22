using UnityEngine;
using System.Collections;
using System;

/// <summary>
///  MouseController, should interact only with the WorldController
/// </summary>

public class MouseController : MonoBehaviour
{
    public GameObject hover;
    private LevelController lvl;
    public Material[] materials;
    private Renderer rend;
    private bool hasMoved = false;
    
    //    public Object marker;

    // The world-position of the mouse last frame.
    Vector3 lastFrameWorldPosition;
    Vector3 currFrameWorldPosition;

    Vector3 currFrameScreenPosition;
    Vector3 lastFrameScreenPosition;


    // Use this for initialization

    void Start()
    {
        
        hover = (GameObject)Instantiate(hover);
        rend = hover.GetComponent<Renderer>();
        if (WorldController.Instance != null)
            if(WorldController.Instance.lvl != null)
                lvl = WorldController.Instance.lvl;
        currFrameWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFrameWorldPosition.z = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (lvl == null)
            lvl = WorldController.Instance.lvl;
        //       currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        currFramePosition.z = .5f;

        currFrameScreenPosition = Input.mousePosition;

        // Do a raycast to find position on field
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        currFrameWorldPosition = ray.origin - (ray.origin.z / ray.direction.z) * ray.direction;
        currFrameWorldPosition.z = .5f;

        UpdateCursor();
        UpdateMovement();

        lastFrameWorldPosition = Input.mousePosition;
        lastFrameWorldPosition.z = .5f;

        lastFrameScreenPosition = currFrameScreenPosition;
        // Press left mouse button
        // TODO: Probably move this code to another controller to enable different interfaces
        if (Input.GetMouseButtonUp(0))
        {
            if (WorldController.Instance.locked == false && hasMoved == false)
            {
                Vector3 Absoluteposition = new Vector3(Mathf.Round(currFrameWorldPosition.x), Mathf.Round(currFrameWorldPosition.y), .5f);
                WorldController.Instance.OnAction((int)Absoluteposition.x, (int)Absoluteposition.y);
            }
            hasMoved = false;
        }
        if (Input.GetMouseButtonDown(1))
            WorldController.Instance.SwitchSelectedTile();
        if (Input.GetMouseButtonDown(2))
            WorldController.Instance.Restart();
    }

    private void UpdateMovement()
    {
        if (Input.GetMouseButton(0))
        {  
            
            Vector3 diff = lastFrameScreenPosition - currFrameScreenPosition;
            Camera.main.transform.Translate(diff/50);
            if(diff != Vector3.zero)
            {
                hasMoved = true;
            }
        }
    }

    void UpdateCursor()
    {
         hover.transform.position = new Vector3(Mathf.Round(currFrameWorldPosition.x), Mathf.Round(currFrameWorldPosition.y), .5f);
        if (WorldController.Instance.locked)
        {
            //            hover.GetComponent<MeshRenderer>().material.color = Color.red;
            rend.sharedMaterial = materials[0];
        }
        else
        {
            //if (WorldController.Instance.selectedtile == ActiveTile.type.Black)
            //    //hover.GetComponent<MeshRenderer>().material.color = Color.green;
            //    rend.sharedMaterial = materials[1];
            //else if (WorldController.Instance.selectedtile == ActiveTile.type.White)
            //    rend.sharedMaterial = materials[2];
            rend.sharedMaterial = materials[(int)WorldController.crrLevel.selectedtile +1];
        }  
    }
}