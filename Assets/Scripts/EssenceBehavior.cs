using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceBehavior : MonoBehaviour {

    public int mode;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //unlock power
            collision.gameObject.GetComponent<MoveBehaviour>().getEssence(mode);

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
