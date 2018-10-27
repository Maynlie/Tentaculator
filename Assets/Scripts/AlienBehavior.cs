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
    public GameObject essence;
    public GameObject tentacle;
    public int mode;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Perso").GetComponent<BoxCollider2D>();
        murD = GameObject.Find("Droite").GetComponent<BoxCollider2D>();
        murG = GameObject.Find("Gauche").GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        tentacle.SetActive(false);
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
                rb.velocity = new Vector2(1 * vitesse, rb.velocity.y);
                Flip();
            }
        }
        else
        {
            if (d2 > 2)
                rb.velocity = new Vector2(1 * vitesse, rb.velocity.y);
            else
            {
                rb.velocity = new Vector2(-1 *vitesse, rb.velocity.y);
                Flip();
            }
        }

        if(d3 < 3)
        {
            if(grounded && mode == 1)
                rb.AddForce(new Vector2(0, 250));
            if(mode == 2)
            {
                //Strike
                tentacle.SetActive(true);
            }
        }

        if(mode == 3 && grounded)
        {
            rb.velocity = new Vector2(0, 0);
        }

        rb.freezeRotation = true;
	}

    void Flip()
    {
        onLeft = !onLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Die()
    {
        //Leave Essence
        GameObject ess = (GameObject)GameObject.Instantiate(essence, transform.position, Quaternion.identity);
        ess.GetComponent<EssenceBehavior>().mode = mode;
        //getDestroyed
        Destroy(gameObject);
    }
}
