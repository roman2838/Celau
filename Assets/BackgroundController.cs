using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {
    public GameObject Planet;
    public float SpeedX, SpeedY, SpeedZ;
    public float RotX, RotY, RotZ;
    


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Rigidbody rb = Planet.GetComponent<Rigidbody>;
        Vector3 old = Planet.transform.position;
        Planet.transform.position = old + new Vector3(SpeedX, SpeedY, SpeedZ);
        Planet.transform.Rotate(new Vector3(RotX, RotY, RotZ));

	}
}
