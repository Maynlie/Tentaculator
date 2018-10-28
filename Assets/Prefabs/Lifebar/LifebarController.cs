using System.Collections;
using System.Collections.Generic;
using AnimationHelper;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class LifebarController : MonoBehaviour {

	public Transform background;
	public Transform foreground;
	public SpriteRenderer sr = null;
	private bool animating = false;

	public float hp = 10;
	public float maxHP = 10;

	private void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		if (background != null) background.transform.localScale = new Vector3 (maxHP + 0.2f, 1, 1);
		if (foreground != null) foreground.transform.localScale = new Vector3 (Mathf.Clamp (hp, 0, maxHP), 0.8f, 1);
	}
	public void AddHp (float amount) {
		hp += amount;
		if (sr != null && amount < 0 && !animating) {
			animating = true;
			Color c = sr.color;
			this.AnimateOverTime01 (Mathf.Max(0.05f, amount * 0.01f), i => {
				sr.color = new Color (
					Mathf.Max(1, c.r + Mathf.Cos (i)*0.2f), 
					Mathf.Min(0, c.g - Mathf.Cos (i)*0.2f), 
					Mathf.Min(0, c.b - Mathf.Cos (i)*0.2f));
				if (i == 1) {
					sr.color = c;
					animating = false;
				}
			});
		}
	}
	public float GetHp () {
		return hp;
	}

	public void SetHp (float h) {
		hp = h;
	}

}