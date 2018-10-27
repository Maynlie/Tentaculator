using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleMouseControl : MonoBehaviour {

	public Transform center;
	public float targetDistance = 1;
	public float mouseDepth = 13;
	private Camera c;

	// Use this for initialization
	void Start () {
		c = Camera.main;
	}

	// Update is called once per frame
	void Update () {
		Vector3 mousePos = c.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 13));
		Vector3 orientation = ((Vector2) mousePos - (Vector2) this.transform.position).normalized;
		Vector3 targetDistanceVect = center.position + orientation * targetDistance;
		GetComponent<TentacleAnimation>().IKBase.position = center.position + orientation * 3;
		Vector2 left = Vector2Extension.Rotate (orientation * targetDistance, 90);
		Vector2 right = Vector2Extension.Rotate (orientation * targetDistance, -90);
		Vector2 pos = left.y > right.y ? left : right;
		GetComponent<TentacleAnimation>().IKTipOfTentacle.GetComponent<TargetJoint2D> ().target = (Vector2)targetDistanceVect + pos;
	}
}