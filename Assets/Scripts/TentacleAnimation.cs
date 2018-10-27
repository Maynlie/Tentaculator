using System.Collections;
using System.Collections.Generic;
using Anima2D;
using UnityEngine;

public class TentacleAnimation : MonoBehaviour {

	public int tentacleLength = 10;
	public float tentacleScaleRatio = 0.95f;
	public float tentacleInitialScaleRatio = 0.5f;
	public float tentaclePositionRatio = 0.8f;

	public GameObject tentaclePrefab;
	public GameObject ikCCDAllPrefab;
	public GameObject ikCCDBasePrefab;

	public Transform IKBase;
	public Transform IKTipOfTentacle;

	// Use this for initialization
	void Start () {

	}

	public void BuildTentacle () {
		Transform parentTentacle = this.transform;
		Transform parentBone = this.transform;
		Bone2D parentBoneScript = null;
		while (parentTentacle.childCount > 0) GameObject.DestroyImmediate (parentTentacle.GetChild (0).gameObject);
		float scaleRatio = 1;
		for (var iTentacleUnit = 0; iTentacleUnit < tentacleLength; iTentacleUnit++) {
			scaleRatio = 1 - ((float) iTentacleUnit / (float) tentacleLength) * 0.5f;
			// create tentacle
			GameObject tentacleUnit = GameObject.Instantiate (
				tentaclePrefab,
				parentTentacle.position,
				this.transform.rotation,
				parentTentacle
			);
			if (iTentacleUnit <= 2) {
				tentacleUnit.GetComponent<SpriteMeshInstance>().sortingOrder = 2;
			}

			// create bone
			GameObject bone = new GameObject ("bone");
			Bone2D boneScript = bone.AddComponent<Bone2D> ();
			bone.transform.SetParent (parentBone);
			bone.transform.position = parentBone.transform.position + new Vector3 (0, scaleRatio * scaleRatio * tentaclePositionRatio, 0);
			if (iTentacleUnit == 0) {
				bone.transform.localRotation = Quaternion.Euler (0, 0, 90);
			} else {
				bone.transform.localRotation = Quaternion.identity;
			}
			bone.transform.localScale = Vector3.one * tentacleInitialScaleRatio;

			if (parentBoneScript != null) {
				// bone.transform.position = parentBoneScript.endPosition;
				boneScript.localLength = scaleRatio;
				boneScript.transform.localScale = new Vector3 (tentacleScaleRatio, tentacleScaleRatio, 1);
				parentBoneScript.child = boneScript;
			}

			tentacleUnit.GetComponent<SpriteMeshInstance> ().bones = new List<Bone2D> () { boneScript };
			//tentacleUnit.transform.localScale = new Vector3 (scaleRatio, scaleRatio, 1);

			// recusiveParent
			parentBone = bone.transform;
			parentBoneScript = boneScript;
			parentTentacle = tentacleUnit.transform;
		}

		GameObject ikCCD = GameObject.Instantiate (ikCCDAllPrefab);
		IkCCD2D ikCCDComponent = ikCCD.GetComponent<IkCCD2D> ();
		ikCCD.transform.SetParent (this.transform);
		ikCCDComponent.target = parentBoneScript;
		ikCCDComponent.numBones = parentBoneScript.chainLength - 2;

		GameObject ikCCD2 = GameObject.Instantiate (ikCCDBasePrefab);
		IkCCD2D ikCCDComponent2 = ikCCD2.GetComponent<IkCCD2D> ();
		ikCCD2.transform.SetParent (this.transform);
		Bone2D baseBone = this.gameObject.GetComponentInChildren<Bone2D> ().child;
		ikCCDComponent2.target = baseBone;
		ikCCDComponent2.numBones = baseBone.chainLength;

		TentacleMouseControl mouseControle = GetComponent<TentacleMouseControl> ();
		if (mouseControle != null) {
			IKBase = ikCCD2.transform;
			IKTipOfTentacle = ikCCD.transform;
			GetComponent<TentacleMouseControl> ().center = baseBone.root.transform;
		}
	}

	public void Attack () {

	}

	// Update is called once per frame
	void Update () {

	}
}