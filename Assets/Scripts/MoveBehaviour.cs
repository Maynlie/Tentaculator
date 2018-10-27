using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour {

    private Rigidbody2D rb;
    private bool facingRight = true;
    private int direction;
    public float vitesse;
    public float jumpHeight;
    public GameObject shot;
    private bool canJump = true;
    public bool hasTentacle = false;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
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

        /*if(Input.GetKeyDown("space") && canJump)
        {
            rb.AddForce(new Vector2(0f, jumpHeight));
        }*/

        if(Input.GetMouseButtonDown(0) && canJump)
        {
            Vector2 mouse = Input.mousePosition;
            mouse = (Vector2)Camera.main.ScreenToWorldPoint(mouse);

            Debug.DrawLine(transform.position, mouse, Color.red);

            ContactFilter2D filter;
            filter.layerMask = LayerMask.NameToLayer("Ground");
            
            RaycastHit2D[] rays = Physics2D.LinecastAll(transform.position, mouse);

            for(int i = 0; i < rays.Length; i++)
            {
                Collider2D coll = rays[i].collider;
                if(coll != null)
                {
                    if(coll.gameObject.layer == 8)
                    {
                        if(rays[i].distance < 3)
                        {
                            Vector2 v = (Vector2)transform.position - mouse;
                            if (v.x > 0) v.x = 1;
                            if (v.x < 0) v.x = -1;
                            if (v.y > 0) v.y = 1;
                            if (v.y < 0) v.y = -1;

                            rb.AddForce(v * jumpHeight);
                        }
                    }
                    if(coll.gameObject.tag == "alien")
                    {
                        if (rays[i].distance < 3)
                        {
                            //repousse l'alien
                        }
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameObject gun = GameObject.Find("Gun");
            GameObject shoot = (GameObject)GameObject.Instantiate(shot, gun.transform.position, Quaternion.identity);
            shoot.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000*direction, 0));
        }

        if(translateX == 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        else
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
        if (type == 1)
            canJump = true;
        else if (type == 2)
            hasTentacle = true;

    }

    public void die()
    {
        Debug.Log("I'm Dead. Let's Respawn and Mutate");
    }
}
