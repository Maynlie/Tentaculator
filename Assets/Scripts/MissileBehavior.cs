using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        if (collision.collider.gameObject.tag == "shield")
        {
            Debug.Log("shield");
        }

        if(collision.collider.gameObject.tag == "alien")
        {
            collision.collider.gameObject.GetComponent<AlienBehavior>().Die();
        }
    }
}
