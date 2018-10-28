using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour {

    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update () {
        this.transform.rotation = Quaternion.Euler (0, 0, Vector2.SignedAngle (Vector2.right, rb.velocity));
    }

    void OnTriggerEnter2D (Collider2D collider) {
        Destroy (gameObject);
        if (collider.gameObject.tag == "shield") {
            Debug.Log ("shield");
        }

        if (collider.gameObject.tag == "alien") {
            collider.gameObject.GetComponent<LifebarController> ().AddHp(-1);
        }
    }
}