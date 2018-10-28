using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonHandler : MonoBehaviour {
    public Sprite[] states;

    public GameObject Player;

    public bool left;

    public bool disabled;

    public Button[] others;

    public MoveBehaviour.EquipHand equip;
    //0: Normal
    //1: Selected
    //2: Disabled
    // Use this for initialization

	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(handleClick);

        if(left)
        {
            if (Player.GetComponent<MoveBehaviour>().leftHand == equip)
                select();
        }
        else
        {
            if (Player.GetComponent<MoveBehaviour>().rightHand == equip)
                select();
        }

        if (disabled) disable();
    }

    void handleClick()
    {
        if (disabled) return;
        equipHand();
        gameObject.GetComponent<Image>().sprite = states[1];
        foreach(Button b in others)
        {
            if(!b.GetComponent<ButtonHandler>().disabled)
               b.GetComponent<ButtonHandler>().enable();
        }
    }

    public void disable()
    {
        disabled = true;
        gameObject.GetComponent<Image>().sprite = states[2];
    }

    public void enable()
    {
        gameObject.GetComponent<Image>().sprite = states[0];
        disabled = false;
    }

    public void select()
    {
        equipHand();
        gameObject.GetComponent<Image>().sprite = states[1];
    }

    public void equipHand()
    {
        if (left)
            Player.GetComponent<MoveBehaviour>().leftHand = equip;
        else
            Player.GetComponent<MoveBehaviour>().rightHand = equip;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
