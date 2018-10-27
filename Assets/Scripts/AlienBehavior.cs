using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBehavior : MonoBehaviour {
    private SelfAnimated animation;
    private bool onLeft = false;
    private bool grounded = false;
    private Rigidbody2D rb;
    private int vitesse = 5;
    public GameObject essence;
    public GameObject tentacle;
    public int mode;
    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D> ();
        animation = GetComponent<SelfAnimated> ();
        tentacle.SetActive (false);
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

        // if (onLeft) {
        //     rb.velocity = new Vector2 (-1 * vitesse, rb.velocity.y);
        // } else {
        //     rb.velocity = new Vector2 (1 * vitesse, rb.velocity.y);
        // }

        if (grounded && mode == 1) rb.AddForce (new Vector2 (0, 250));

        rb.freezeRotation = true;
        if (rb.velocity.magnitude < 0.1f) {
            if (animation.currentAnimation != "Iddle") animation.Play ("Iddle");
        } else {
            if (animation.currentAnimation != "Run") animation.Play ("Run");
        }
    }

    public void Die () {
        //Leave Essence
        GameObject ess = (GameObject) GameObject.Instantiate (essence, transform.position, Quaternion.identity);
        ess.GetComponent<EssenceBehavior> ().mode = mode;
        //getDestroyed
        Destroy (gameObject);
    }
}