using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationHelper {
	public static class Animations {

		public static Coroutine AnimateOverTime01 (this MonoBehaviour go, float durationInSec, Action<float> onTick) {
			return go.StartCoroutine (exectuteAnimateOverTime01 (durationInSec, onTick));
		}
		public static IEnumerator exectuteAnimateOverTime01 (float durationInSec, Action<float> onTick) {
			for (float i = 0; i <= durationInSec; i += Time.deltaTime) {
				onTick (i / durationInSec);
				yield return null;
			}
			onTick (1);
		}

		public static void setTimeout (this MonoBehaviour go, Action callback, float ms) {
			go.StartCoroutine (executeAfter (callback, ms));
		}
		private static IEnumerator executeAfter (Action callback, float ms) {
			yield return new WaitForSeconds (ms / 1000);
			callback ();
		}

	}
}