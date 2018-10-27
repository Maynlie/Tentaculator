using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationHelper;

public class MoveBehaviour : MonoBehaviour {

    // first is left 2nd is right
    private TentacleAnimation[] tentacleAnims;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private int direction = 1;
    public float vitesse;
    public float jumpHeight;
    public float attackDistance;
    public GameObject shot;
    public GameObject shield;


    public float anticipationDuration = 0.3f;
    public float actionDuration = 0.1f;
    private bool canJump = true;
    private bool hasTentacle = true;
    private bool hasShield = true;

    private bool shieldSpawned = false;
    private bool paused = false;
    
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

        paused = false;

        tentacleAnims = gameObject.GetComponentsInChildren<TentacleAnimation>();

	}

    // Update is called once per frame
    void Update () {
        Debug.Log(paused);
        if (!paused)
        {
            float translateX = Input.GetAxis("Horizontal") * vitesse;


            if (translateX > 0)
            {
                direction = 1;
                if (!facingRight) Flip();
            }
            else if (translateX < 0)
            {
                direction = -1;
                if (facingRight) Flip();
            }


        if(Input.GetMouseButtonDown(0) && leftHand != EquipHand.NONE)
        {
            HandleClick(leftHand, tentacleAnims.Length > 0 ? tentacleAnims[0] : null);
        }

        if(Input.GetMouseButtonDown(1) && rightHand != EquipHand.NONE)
        {
            HandleClick(rightHand, tentacleAnims.Length > 0 ? tentacleAnims[1] : null);
        }


            if (translateX == 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(translateX, rb.velocity.y);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                die();
            }

            rb.freezeRotation = true;

        }
    }

    private void HandleClick(EquipHand hand, TentacleAnimation tentacleAnim) {
        switch (hand) {
            case EquipHand.GUN:
                handleGun(tentacleAnim);
                break;
            case EquipHand.DASH:
                handleDash(tentacleAnim);
                break;
            case EquipHand.TENTACLE:
                handleTentacle(tentacleAnim);
                break;
            case EquipHand.SHIELD:
                handleShield(tentacleAnim);
                break;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        GetComponent<SpriteRenderer>().flipX = facingRight;
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
        
        GameObject.Find("LevelManager").GetComponent<LevelSPawner>().mutate();

        paused = true;
    }

    public void resume()
    {
        Debug.Log("resume");
        paused = false;
    }

    public void handleGun(TentacleAnimation tentacleAnim)
    {
        Vector2 mouse = Input.mousePosition;
        mouse = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
        Vector3 orientation = (mouse - (Vector2) this.transform.position).normalized;
        GameObject shoot = (GameObject)GameObject.Instantiate(shot, this.transform.position + orientation, Quaternion.identity);
        //Si jamais on veut tirer en direction de la souris plutot que tout droit

        shoot.GetComponent<Rigidbody2D>().AddForce(orientation * 1000);

        //shoot.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000 * direction, 0));
    }

    public void handleDash(TentacleAnimation tentacleAnim)
    {
        if (!canJump) return;
        Vector2 mouse = Input.mousePosition;
        mouse = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
        RaycastHit2D ray = Physics2D.Linecast(transform.position, mouse, 1 << LayerMask.NameToLayer("Default"));

        Collider2D coll = ray.collider;

        Transform ikTarget = tentacleAnim.IKTipOfTentacle;        
        this.AnimateOverTime01(actionDuration, j => {
            Vector2 destination = (Vector2)transform.position + (mouse - (Vector2) this.transform.position).normalized * attackDistance;
            ikTarget.position = Vector3.Lerp(
                ikTarget.position, 
                destination,
                j
            );
        });

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

    public void handleTentacle(TentacleAnimation tentacleAnim)
    {
        if (!hasTentacle) return;

        Transform ikTarget = tentacleAnim.IKTipOfTentacle;

        this.AnimateOverTime01(anticipationDuration, i => {
            ikTarget.position = Vector3.Lerp(
                ikTarget.position, 
                transform.position,
                i
            );
            if(i == 1) {
                this.AnimateOverTime01(actionDuration, j => {
                    Vector2 mouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 destination = (Vector2)transform.position + (mouse - (Vector2) this.transform.position).normalized * attackDistance;
                    ikTarget.position = Vector3.Lerp(
                        ikTarget.position, 
                        destination,
                        j
                    );
                    if (j == 1) {
                        Debug.DrawLine(transform.position, destination);
                        RaycastHit2D ray = Physics2D.Linecast(transform.position, destination, 1<<LayerMask.NameToLayer("Default"));
                        Collider2D coll = ray.collider;
                        if (coll != null)
                        {
                            Debug.Log(coll.gameObject.tag);
                            if (coll.gameObject.tag == "alien")
                            {
                                coll.gameObject.GetComponent<AlienBehavior>().Die();
                            }
                        }
                    }
                });
            }
        });
    }

    public void handleShield(TentacleAnimation tentacleAnim)
    {
        if (!hasShield) return;
        if (shieldSpawned) return;
        Vector2 mouse = Input.mousePosition;
        mouse = (Vector2)Camera.main.ScreenToWorldPoint(mouse);


        Transform ikTarget = tentacleAnim.IKTipOfTentacle;
        Transform ikbaseTarget = tentacleAnim.IKBase;

        this.AnimateOverTime01(anticipationDuration, i => {
            ikTarget.position = Vector3.Lerp(
                ikTarget.position, 
                transform.position + new Vector3(0, 0.2f, 0),
                i
            );
            ikbaseTarget.position = Vector3.Lerp(
                ikbaseTarget.position, 
                transform.position,
                i
            );
            if(i == 1) {
                Vector2 v = (mouse - (Vector2)transform.position).normalized;
                GameObject shieldSpawn = (GameObject)GameObject.Instantiate(shield, transform.position + (Vector3)v * 2, Quaternion.Euler(0, 0, v.y > 0 ? Vector2.Angle(Vector2.right, v) : Vector2.Angle(Vector2.left, v)));
                shieldSpawn.transform.parent = this.transform;
                shieldSpawn.transform.localScale = Vector3.one * 4;
                shieldSpawned = true;
                this.AnimateOverTime01(0.4f, j => {
                    ikTarget.position = transform.position + new Vector3(0, 0.2f, 0);
                    ikbaseTarget.position = Vector3.Lerp(
                        ikbaseTarget.position, 
                        shieldSpawn.transform.position,
                        i * 2
                    );
                });
            }
        });
    }

    public void clearShield()
    {
        shieldSpawned = false;
    }
}
