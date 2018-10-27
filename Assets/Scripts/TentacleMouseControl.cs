using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleMouseControl : MonoBehaviour {

	public Rigidbody2D baseIk;
	public Rigidbody2D tentacleIk;
	public float targetDistance = 1;
	public float mouseDepth = 13;
	private Camera c;

	// Use this for initialization
	void Start () {
		c = Camera.main;
	}

	// Update is called once per frame
	void Update () {
		Vector3 _mousePos = c.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, mouseDepth));
		Vector3 orientation = ((Vector2) _mousePos - (Vector2) this.transform.position).normalized * targetDistance;
		baseIk.position = this.transform.position + orientation;
		Vector2 left = Vector2Extension.Rotate (orientation, 90);
		Vector2 right = Vector2Extension.Rotate (orientation, -90);
		Vector2 pos = left.y > right.y ? left : right;
		tentacleIk.GetComponent<TargetJoint2D> ().target = baseIk.position + pos;
	}
}