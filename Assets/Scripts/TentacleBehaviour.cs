using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBehaviour : MonoBehaviour {
    private float time = 0;
	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //KO Player
            collision.gameObject.GetComponent<MoveBehaviour>().die();
        }
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        if (time > 1.5)
            gameObject.SetActive(false);
	}
}
