using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour {

    private Rigidbody2D rb;
    private bool grounded = false;
    private bool facingRight = true;
    private int direction;
    public float vitesse;
    public float jumpHeight;
    public GameObject shot;
    public bool canJump = false;

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

        if (translateX > 0)
        {
            direction = 1;
            if (!facingRight) Flip();
        }
        else if(translateX < 0)
        {
            direction = -1;
            if (facingRight) Flip();
        }

        if(Input.GetKeyDown("space") && grounded && canJump)
        {
            rb.AddForce(new Vector2(0f, jumpHeight));
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameObject gun = GameObject.Find("Gun");
            GameObject shoot = (GameObject)GameObject.Instantiate(shot, gun.transform.position, Quaternion.identity);
            shoot.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000*direction, 0));
        }

        rb.velocity = new Vector2(translateX, rb.velocity.y);

        rb.freezeRotation = true;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void getEssence(int type)
    {
        canJump = true;
    }
}
