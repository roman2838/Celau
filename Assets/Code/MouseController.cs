using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour
{
    public GameObject hover;
    //    public Object marker;

    // The world-position of the mouse last frame.
    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    // Use this for initialization

    void Start()
    {
        hover = (GameObject)Instantiate(hover);
        Debug.Log("Mouseposition: " + Input.mousePosition);
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        //       currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        currFramePosition.z = .5f;
        // Do a raycast to find position on field
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        currFramePosition = ray.origin - (ray.origin.z / ray.direction.z) * ray.direction;
        currFramePosition.z = .5f;

        UpdateCursor();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = .5f;

        if (Input.GetMouseButtonDown(0))
        {
            if (WorldController.Instance.locked == false)
            {
                WorldController.Instance.locked = true;
                Vector3 Absoluteposition = new Vector3(Mathf.Round(currFramePosition.x), Mathf.Round(currFramePosition.y), .5f);
                BackgroundTile tile = WorldController.Instance.GetTileAt((int)Absoluteposition.x, (int)Absoluteposition.y);
                if (tile != null)
                {
                    tile.CreateChild();
                    StartCoroutine(WorldController.Instance.UpdateTiles());
                }
                else Debug.Log("Out of boundaries");
            }
        }
    }

    void UpdateCursor()
    {
         hover.transform.position = new Vector3(Mathf.Round(currFramePosition.x), Mathf.Round(currFramePosition.y), .5f);
        if (WorldController.Instance.locked)
            hover.GetComponent<MeshRenderer>().material.color = Color.red;
        else
            hover.GetComponent<MeshRenderer>().material.color = Color.green;
           
    }
}