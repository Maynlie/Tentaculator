using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour {

    private Rigidbody2D rb;
    private bool facingRight = true;
    private int direction = 1;
    public float vitesse;
    public float jumpHeight;
    public GameObject shot;
    public GameObject shield;
    private bool canJump = true;
    private bool hasTentacle = true;
    private bool hasShield = true;
    private bool shieldSpawned = false;
    
    public enum EquipHand
    {
        NONE,
        GUN,
        DASH,
        TENTACLE,
        SHIELD
    };

    public EquipHand leftHand;
    public EquipHand rightHand;

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

        if(Input.GetMouseButtonDown(0) && leftHand != EquipHand.NONE)
        {
            switch(leftHand)
            {
                case EquipHand.GUN:
                    handleGun();
                    break;
                case EquipHand.DASH:
                    handleDash();
                    break;
                case EquipHand.TENTACLE:
                    handleTentacle();
                    break;
                case EquipHand.SHIELD:
                    handleShield();
                    break;
            }
        }

        if(Input.GetMouseButtonDown(1) && rightHand != EquipHand.NONE)
        {
            switch (rightHand)
            {
                case EquipHand.GUN:
                    handleGun();
                    break;
                case EquipHand.DASH:
                    handleDash();
                    break;
                case EquipHand.TENTACLE:
                    handleTentacle();
                    break;
                case EquipHand.SHIELD:
                    handleShield();
                    break;
            }
        }

        if(translateX == 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(translateX, rb.velocity.y);
        }
            

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
        else if (type == 3)
            hasShield = true;

    }

    public void die()
    {
        Debug.Log("I'm Dead. Let's Respawn and Mutate");
    }

    public void handleGun()
    {
        GameObject gun = GameObject.Find("Gun");
        Vector2 mouse = Input.mousePosition;
        mouse = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
        GameObject shoot = (GameObject)GameObject.Instantiate(shot, gun.transform.position, Quaternion.identity);
        //Si jamais on veut tirer en direction de la souris plutot que tout droit
        shoot.GetComponent<Rigidbody2D>().AddForce(((Vector2)transform.position - mouse).normalized * 1000);
        //shoot.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000 * direction, 0));
    }

    public void handleDash()
    {
        if (!canJump) return;
        Vector2 mouse = Input.mousePosition;
        mouse = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
        RaycastHit2D ray = Physics2D.Linecast(transform.position, mouse, 1 << LayerMask.NameToLayer("Default"));

        Collider2D coll = ray.collider;

        if (coll != null)
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                if (ray.distance < 3)
                {
                    Vector2 v = ((Vector2)transform.position - mouse).normalized;
                                        rb.AddForce(v * jumpHeight);
                }
            }
            if (coll.gameObject.tag == "alien")
            {
                if (ray.distance < 3)
                {
                     //repousse l'alien et damage
                }
            }
        }
    }

    public void handleTentacle()
    {
        if (!hasTentacle) return;
        Vector2 mouse = Input.mousePosition;
        mouse = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
        RaycastHit2D ray = Physics2D.Linecast(transform.position, mouse, 1<<LayerMask.NameToLayer("Default"));

        Debug.DrawLine(transform.position, mouse);

        

        Collider2D coll = ray.collider;
        Debug.Log(coll);
        if (coll != null)
        {
            Debug.Log(coll.gameObject.tag);
            if (coll.gameObject.tag == "alien")
            {
                if (ray.distance < 3)
                {
                    //more damage
                    coll.gameObject.GetComponent<AlienBehavior>().Die();
                } 
            }
        }
        
    }

    public void handleShield()
    {
        if (!hasShield) return;
        if (shieldSpawned) return;
        Vector2 mouse = Input.mousePosition;
        mouse = (Vector2)Camera.main.ScreenToWorldPoint(mouse);

        Vector2 v = (mouse - (Vector2)transform.position).normalized;

        GameObject shieldSpawn = (GameObject)GameObject.Instantiate(shield, transform.position + (Vector3)v * 2, Quaternion.Euler(0, 0, Vector2.Angle(Vector2.right, v)));
        shieldSpawn.transform.parent = transform;

        shieldSpawned = true;
    }

    public void clearShield()
    {
        shieldSpawned = false;
    }
}
