using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationHelper;

public class MoveBehaviour : MonoBehaviour {

    // first is left 2nd is right
    private TentacleAnimation[] tentacleAnims;
    private SelfAnimated anim;
    private Rigidbody2D rb;
    public bool facingRight = true;
    private int direction = 1;
    public float vitesse;
    public float jumpHeight;
    public float dashKnockBackForce;
    public float attackDistance;
    public GameObject shot;
    public GameObject shieldSpawn;

    public GameObject gun;
    public GameObject dasher;
    public GameObject attacker;
    public GameObject shield;

    private AudioSource audioSource;
	public AudioClip[] footstepSound;
    private float footstepDelay = 0.3f;
    private float footstepSoundacc = 0.5f;


    public float anticipationDuration = 0.3f;
    public float actionDuration = 0.1f;
    private bool canJump;
    public bool CanJump
    {
        get
        {
            return canJump;
        }
        set
        {
            canJump = value;
        }

    }
    private bool hasTentacle;
    public bool HasTentacle
    {
        get
        {
            return hasTentacle;
        }
        set
        {
            hasTentacle = value;
        }
    }
    private bool hasShield = true;
    public bool HasShield
    {
        get
        {
            return hasShield;
        }
        set
        {
            hasShield = value;
        }
    }

    public float dashDamage = 2;
    public float attackDamage = 25;
    public float shieldDamage = 15;

    private bool grounded = false;

    private bool shieldSpawned = false;
    private bool paused = false;
    
    public enum EquipHand
    {
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
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<SelfAnimated>();
        paused = false;
        canJump = false;
        hasShield = false;
        hasTentacle = false;

        tentacleAnims = gameObject.GetComponentsInChildren<TentacleAnimation>();

        foreach (TentacleAnimation anim in tentacleAnims)
        {
            Debug.Log(anim.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
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


        if(Input.GetMouseButtonDown(0))
        {
            HandleClick(leftHand, tentacleAnims.Length > 0 ? tentacleAnims[0] : null);
        }

        if(Input.GetMouseButtonDown(1))
        {
            HandleClick(rightHand, tentacleAnims.Length > 1 ? tentacleAnims[1] : null);
        }


            if (translateX == 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                anim.Play("Iddle");
            }
            else
            {
                footstepSoundacc += Time.deltaTime;
                rb.velocity = new Vector2(translateX, rb.velocity.y);
                if (anim.currentAnimation != "Run" && grounded) anim.Play("Run");
                if (anim.currentAnimation != "DashFwd" && !grounded 
                    && (rb.velocity.x > 0 && facingRight
                    || (rb.velocity.x < 0 && !facingRight))) {
                    anim.Play("DashFwd");
                }
                if (anim.currentAnimation != "DashBack" && !grounded 
                    && (rb.velocity.x > 0 && !facingRight
                    || (rb.velocity.x < 0 && facingRight))) {
                    anim.Play("DashBack");
                }
            }
            if(footstepSoundacc > footstepDelay && grounded) {
                footstepSoundacc = 0;
                playFootstep(0.3f);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                die();
            }

            rb.freezeRotation = true;

        }
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (collision.collider.gameObject.tag == "ground") {
            grounded = true;
        }
    }

    private void OnCollisionExit2D (Collision2D collision) {
        if (collision.collider.gameObject.tag == "ground") {
            grounded = false;
        }
    }

    private void playFootstep (float volume) {
		if (footstepSound.Length <= 0) return;
		audioSource.clip = footstepSound[Random.Range (0, footstepSound.Length - 1)];
		audioSource.volume = volume;
		audioSource.Play();
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

    public void equipAll()
    {
        equip(leftHand);
        Debug.Log(leftHand != EquipHand.GUN || rightHand != leftHand);
        if(leftHand != EquipHand.GUN || rightHand != leftHand)
            equip(rightHand);

        

        tentacleAnims = gameObject.GetComponentsInChildren<TentacleAnimation>();

        foreach(TentacleAnimation anim in tentacleAnims)
        {
            Debug.Log(anim.gameObject);
        }


    }

    public void equip(EquipHand hand)
    {
        GameObject toEquip;
        switch(hand)
        {
            case EquipHand.GUN:
                {
                    toEquip = (GameObject)GameObject.Instantiate(gun, new Vector3(0, 0, 1), Quaternion.identity, transform);
                    toEquip.transform.localPosition = new Vector3(0, 0, 0);
                    break;
                }
            case EquipHand.DASH:
                {
                    toEquip = (GameObject)GameObject.Instantiate(dasher, new Vector3(0, 0, 1), Quaternion.identity, transform);
                    toEquip.transform.localPosition = new Vector3(0, 0, 0);
                    break;
                }
            case EquipHand.TENTACLE:
                {
                    toEquip = (GameObject)GameObject.Instantiate(attacker, new Vector3(0, 0, 1), Quaternion.identity, transform);
                    toEquip.transform.localPosition = new Vector3(0, 0, 0);
                    break;
                }
            case EquipHand.SHIELD:
                {
                    toEquip = (GameObject)GameObject.Instantiate(shield, new Vector3(0, 0, 1), Quaternion.identity, transform);
                    toEquip.transform.localPosition = new Vector3(0, 0, 0);
                    break;
                }
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
        if(leftHand == EquipHand.GUN && rightHand == EquipHand.GUN) {
            anim.PlayOnce("DieArm");
        } else {
            anim.PlayOnce("Die");
        }
        this.setTimeout(() => {
            GameObject.Find("LevelManager").GetComponent<LevelSPawner>().mutate();
        }, 1000);

        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        paused = true;
    }

    public void resume()
    {
        paused = false;
    }

    public void handleGun(TentacleAnimation tentacleAnim)
    {
        Vector2 mouse = Input.mousePosition;
        mouse = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
        Vector3 orientation = (mouse - (Vector2) this.transform.position).normalized;
        GameObject shoot = (GameObject)GameObject.Instantiate(shot, this.transform.position + orientation, Quaternion.identity);
        
        shoot.GetComponent<Rigidbody2D>().AddForce(orientation * 1000);
        tentacleAnim.PlayAnimSound(0.1f);

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
        this.AnimateOverTime01(0.3f, j => {
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
                    tentacleAnim.PlayHitSound(0.2f);
                    Vector2 v = ((Vector2)transform.position - mouse).normalized;
                                        rb.AddForce(v * jumpHeight);
                }
            } else {
                 tentacleAnim.PlayAnimSound(0.1f);
            }
            if (coll.gameObject.tag == "alien")
            {
                if (ray.distance < 3)
                {
                     //repousse l'alien et damage
                     Vector2 v = (mouse - (Vector2)transform.position).normalized;
                     coll.gameObject.GetComponent<Rigidbody2D>().AddForce(v * dashKnockBackForce);
                     coll.gameObject.GetComponent<LifebarController>().AddHp(-dashDamage);
                }
            }
        } else {
            tentacleAnim.PlayAnimSound(0.1f);
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
                tentacleAnim.PlayAnimSound(0.2f);
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
                            tentacleAnim.PlayHitSound(0.4f);
                            Debug.Log(coll.gameObject.tag);
                            if (coll.gameObject.tag == "alien")
                            {
                                Vector2 v = (mouse - (Vector2)transform.position).normalized;
                                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(v * dashKnockBackForce);
                                coll.gameObject.GetComponent<LifebarController>().AddHp(-attackDamage);
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
        tentacleAnim.PlayAnimSound(0.5f);
        
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
                GameObject shieldSpawner = (GameObject)GameObject.Instantiate(shieldSpawn, transform.position + (Vector3)v * 2, Quaternion.Euler(0, 0, v.y > 0 ? Vector2.Angle(Vector2.right, v) : Vector2.Angle(Vector2.left, v)));
                shieldSpawner.transform.parent = this.transform;
                shieldSpawner.transform.localScale = Vector3.one * 4;
                shieldSpawned = true;
                tentacleAnim.PlayHitSound(0.5f);
                this.AnimateOverTime01(0.4f, j => {
                    ikTarget.position = transform.position + new Vector3(0, 0.2f, 0);
                    ikbaseTarget.position = Vector3.Lerp(
                        ikbaseTarget.position,
                        shieldSpawner.transform.position,
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
