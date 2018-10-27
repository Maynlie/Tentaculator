using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBehavior : MonoBehaviour {
    private BoxCollider2D murD, murG;
    private BoxCollider2D player;
    private bool onLeft = true;
    private bool grounded = false;
    private Rigidbody2D rb;
    private int vitesse = 5;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Perso").GetComponent<BoxCollider2D>();
        murD = GameObject.Find("Droite").GetComponent<BoxCollider2D>();
        murG = GameObject.Find("Gauche").GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "ground")
        {
            grounded = false;
        }
    }

    // Update is called once per frame
    void Update () {
        BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
        float d1, d2, d3;
        d1 = bc.Distance(murG).distance;
        d2 = bc.Distance(murD).distance;
        d3 = bc.Distance(player).distance;

        if(onLeft)
        {
            if (d1 > 2)
                rb.velocity = new Vector2(-1 * vitesse, rb.velocity.y);
            else
            {
                onLeft = false;
                rb.velocity = new Vector2(1 * vitesse, rb.velocity.y);
            }
        }
        else
        {
            if (d2 > 2)
                rb.velocity = new Vector2(1 * vitesse, rb.velocity.y);
            else
            {
                onLeft = true;
                rb.velocity = new Vector2(-1 *vitesse, rb.velocity.y);
            }
        }

        if(d3 < 3)
        {
            Debug.Log("trigger");
            if(grounded)
                rb.AddForce(new Vector2(0, 250));
        }

        rb.freezeRotation = true;
	}
}
