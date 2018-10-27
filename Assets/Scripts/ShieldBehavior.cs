using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour {

    // Use this for initialization
    float time;
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time > 0.75)
        {
            GameObject.Find("Perso").GetComponent<MoveBehaviour>().clearShield();
            Destroy(gameObject);
        }
	}
}
