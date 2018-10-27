using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour {

    private Rigidbody2D rb;
    private bool grounded = false;
    public float vitesse;
    public float jumpHeight;
    public Transform groundPos;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        
        for (int i = 0; i < coll.contacts.Length; i++)
        {
            if (coll.contacts[i].normal.y > 0)
            {
                grounded = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if(coll.collider.gameObject.tag == "ground")
        {
            grounded = false;
        }
    }

    // Update is called once per frame
    void Update () {
        float translateX = Input.GetAxis("Horizontal") * vitesse;

        if(Input.GetKeyDown("space") && grounded)
        {
            rb.AddForce(new Vector2(0f, jumpHeight));
        }

        rb.velocity = new Vector2(translateX, rb.velocity.y);
    }
}
