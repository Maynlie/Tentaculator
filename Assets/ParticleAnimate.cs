using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnimate : MonoBehaviour {

	public float speed = 4f;
	public float maxGrow = 10f;
	public float minSize = 40f;

	// Update is called once per frame
	void Update () {
		this.transform.localScale = Vector2.one * minSize + Vector2.one * maxGrow * Mathf.Sin(Time.time * speed);
	}
}
