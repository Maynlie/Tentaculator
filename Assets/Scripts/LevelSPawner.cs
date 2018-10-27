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
    public Button btn;

	// Use this for initialization
	void Start () {
        canvas.gameObject.SetActive(false);
        btn.onClick.AddListener(TaskOnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void TaskOnClick()
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
    }
}
