using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfAnimated : MonoBehaviour {

	[Serializable]
	public struct Animation {
		public string name;
		public Sprite[] sprites;
	}

	public Animation[] animations;

	public float animationDurationInSec = 0.5f;
	public bool animate = true;
	public bool loop = true;
	private SpriteRenderer srenderer;
	private float acc = 0;
	private int spriteIndex = 0;
	private int animationIndex = 0;

	public string currentAnimation {
		get {
			if (animationIndex < 0) return null;
			return animations[animationIndex].name;
		}
	}

	void Awake () {
		srenderer = GetComponent<SpriteRenderer> ();
	}

	public void Play (string name, float duration = -1) {
		if (duration > 0) animationDurationInSec = duration;
		ChangeAnimation (name);
		loop = true;
		animate = true;
	}

	public void PlayOnce (string name, float duration = -1) {
		if (duration > 0) animationDurationInSec = duration;
		ChangeAnimation (name);
		loop = false;
		animate = true;
	}

	public void Pause () {
		animate = false;
	}
	public void Resume () {
		animate = true;
	}

	public void ChangeAnimation (string name) {
		animationIndex = Array.FindIndex (animations, a => a.name == name);
		acc = 0;
		UpdateSprite (acc);
	}

	void Update () {
		if (!animate ||
			animationIndex == -1 ||
			animations.Length == 0 ||
			animations[animationIndex].sprites == null ||
			animations[animationIndex].sprites.Length == 0 ||
			srenderer == null) {
			return;
		}

		if (loop) {
			acc = (acc + Time.deltaTime);
			acc = acc % animationDurationInSec;
		} else if (acc < animationDurationInSec) {
			acc = (acc + Time.deltaTime);
			if (acc >= animationDurationInSec) {
				acc = animationDurationInSec;
			}
		}

		UpdateSprite (acc);
	}

	void UpdateSprite (float acc) {
		if (animationIndex == -1 ||
			animations.Length == 0 ||
			animations[animationIndex].sprites == null ||
			animations[animationIndex].sprites.Length == 0 ||
			srenderer == null) {
			return;
		}
		spriteIndex = Mathf.FloorToInt (
			Mathf.Max (0, Mathf.Min (animations[animationIndex].sprites.Length - 1,
				(acc * animations[animationIndex].sprites.Length) / animationDurationInSec)));
		srenderer.sprite = animations[animationIndex].sprites[spriteIndex];
	}

}