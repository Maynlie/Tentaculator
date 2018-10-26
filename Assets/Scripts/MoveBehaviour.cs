using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour {

    public Rigidbody2D rb;
    public float vitesse;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("left"))
        {
            rb.AddForce(-transform.right * vitesse);
        }
        else if(Input.GetKey("right"))
        {
            rb.AddForce(transform.right * vitesse);
        }
        else if(Input.GetKeyUp("left"))
        {
            rb.AddForce(transform.right * vitesse);
        }
        else if (Input.GetKeyUp("right"))
        {
            rb.AddForce(-transform.right * vitesse);
        }
    }
}
