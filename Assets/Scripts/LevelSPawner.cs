using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSPawner : MonoBehaviour {
    public GameObject[] levels;
    public Vector3 playerPos;
    public GameObject player;
    public Canvas canvas;
    public Button[] btns;

	// Use this for initialization
	void Start () {
        canvas.gameObject.SetActive(false);
        btns[6].onClick.AddListener(validMutation);
    }
    
    // Update is called once per frame
    void Update () {
		
	}

    void validMutation()
    {
        player.GetComponent<MoveBehaviour>().resume();
        Reset();
        canvas.gameObject.SetActive(false);
    }



    public void Reset()
    {
        GameObject lvl = GameObject.Find("Level");
        Transform t = lvl.transform;
        Destroy(GameObject.Find("Level"));
        
        GameObject o = (GameObject)GameObject.Instantiate(levels[0], t.position, Quaternion.identity);
        o.name = "Level";

        player.transform.position = playerPos;
    }

    public void mutate()
    {
        canvas.gameObject.SetActive(true);
        if (player.GetComponent<MoveBehaviour>().CanJump)
        {
            if (btns[0].GetComponent<ButtonHandler>().disabled)
                btns[0].GetComponent<ButtonHandler>().enable();
            if (btns[3].GetComponent<ButtonHandler>().disabled)
                btns[3].GetComponent<ButtonHandler>().enable();
        }
        if (player.GetComponent<MoveBehaviour>().HasTentacle)
        {
            if (btns[1].GetComponent<ButtonHandler>().disabled)
                btns[1].GetComponent<ButtonHandler>().enable();
            if (btns[4].GetComponent<ButtonHandler>().disabled)
                btns[4].GetComponent<ButtonHandler>().enable();
        }
        if (player.GetComponent<MoveBehaviour>().HasShield)
        {
            if (btns[2].GetComponent<ButtonHandler>().disabled)
                btns[2].GetComponent<ButtonHandler>().enable();
            if (btns[5].GetComponent<ButtonHandler>().disabled)
                btns[5].GetComponent<ButtonHandler>().enable();
        }
    }
}
