using System;
using System.Collections;
using System.Collections.Generic;
using AnimationHelper;
using UnityEngine;

public class AlienBehavior : MonoBehaviour {
    private SelfAnimated animation;
    private LifebarController life;
    private bool onLeft = false;
    private bool grounded = false;
    private Rigidbody2D rb;
    public int vitesse = 3;
    public GameObject essence;
    public GameObject tentacle;
    private TentacleAnimation tentacleAnim;

    private float accumulator = 0;
    public float attackCooldown = 1;

    public GameObject attackTentaclePrefab;
    public GameObject dashTentaclePrefab;
    public GameObject shieldTentaclePrefab;

    // 1 -> saut, 2 -> attaque, 3 -> shield
    public int mode;
    // Use this for initialization
    void Start () {
        life = GetComponent<LifebarController> ();
        rb = gameObject.GetComponent<Rigidbody2D> ();
        animation = GetComponent<SelfAnimated> ();
        tentacle.SetActive (false);

        GameObject t = null;
        if (mode == 1) {
            t = GameObject.Instantiate (dashTentaclePrefab);
        }
        if (mode == 2) {
            t = GameObject.Instantiate (attackTentaclePrefab);
        }
        if (mode == 3) {
            t = GameObject.Instantiate (shieldTentaclePrefab);
        }
        if (t != null) {
            t.transform.parent = this.transform;
            t.GetComponent<TentacleMouseControl> ().enabled = false;
            tentacleAnim = t.GetComponent<TentacleAnimation> ();
            tentacleAnim.BuildTentacle();
            t.transform.localPosition = Vector3.zero;
            t.transform.localScale = Vector3.one;
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

    // Update is called once per frame
    void Update () {
        BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D> ();

        if (this.transform.position.x < -6 && onLeft) {
            GetComponent<SpriteRenderer> ().flipX = true;
            onLeft = false;

        } else if (this.transform.position.x > 6 && !onLeft) {
            GetComponent<SpriteRenderer> ().flipX = false;
            onLeft = true;
        }

        if (onLeft) {
            rb.velocity = new Vector2 (-1 * vitesse, rb.velocity.y);
        } else {
            rb.velocity = new Vector2 (1 * vitesse, rb.velocity.y);
        }

        if (tentacleAnim != null && tentacleAnim.IKBase != null) {
            tentacleAnim.IKBase.transform.position = this.transform.position + new Vector3 (Mathf.Cos (Time.time * 1.5f + mode) * 2, Mathf.Sin (Time.time + mode) * 2, 1);
            tentacleAnim.IKTipOfTentacle.transform.position = this.transform.position + new Vector3 (Mathf.Sin (Time.time * 1.5f + mode) * 2.5f, Mathf.Sin (Time.time + mode) * 2.5f, 1);

            accumulator -= Time.deltaTime;
            if (accumulator <= 0) {
                if (mode == 1 && grounded) {
                    Vector2 orientation = (tentacleAnim.IKBase.transform.position - this.transform.position).normalized;
                    rb.AddForce (new Vector2 (orientation.x * 200, orientation.y * 200));
                    Debug.Log("Going toward " + orientation);
                    this.AnimateOverTime01 (0.4f, j => {
                        Vector2 destination = (Vector2) transform.position - orientation;
                        tentacleAnim.IKTipOfTentacle.position = Vector3.Lerp (
                            tentacleAnim.IKTipOfTentacle.position,
                            destination,
                            j
                        );
                    });
                }
                if (mode == 2) {
                    //t = GameObject.Instantiate (attackTentaclePrefab);
                }
                if (mode == 3) {
                    // t = GameObject.Instantiate (shieldTentaclePrefab);
                }
                accumulator = attackCooldown + attackCooldown * UnityEngine.Random.Range (0, 1);
            }
        }
        if (grounded && mode == 1)

            rb.freezeRotation = true;
        if (rb.velocity.magnitude < 0.1f) {
            if (animation.currentAnimation != "Iddle") animation.Play ("Iddle");
        } else {
            if (animation.currentAnimation != "Run") animation.Play ("Run");
        }

        if (life.GetHp () <= 0) {
            life.SetHp (0);
            Die ();
        }
    }

    public void Die () {
        //Leave Essence
        GameObject ess = (GameObject) GameObject.Instantiate (essence, transform.position, Quaternion.identity);
        ess.GetComponent<EssenceBehavior> ().mode = mode;
        ess.transform.parent = GameObject.Find ("Level").transform;
        //getDestroyed
        Destroy (gameObject);
    }
}