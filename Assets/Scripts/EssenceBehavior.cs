using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceBehavior : MonoBehaviour {

    private int _mode;
    public int mode {
        get {
            return _mode;
        }
        set {
            _mode = value;
            Color c = Color.yellow; // jump
            if (_mode == 2) c = Color.red; // attack
            if (_mode == 3) c = Color.green; // shield
            foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
                sr.color = c;
            }
        }
    }

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
