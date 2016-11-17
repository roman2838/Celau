using UnityEngine;
using System.Collections;

public class ActiveBlackTile : MonoBehaviour {

    Rigidbody rb;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Fling(Vector3 v)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(v);
    }

}
