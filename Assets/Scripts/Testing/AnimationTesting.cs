using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Testing {

	[ExecuteInEditMode] [RequireComponent(typeof(Animator))]
	public class AnimationTesting : MonoBehaviour {


		public AnimationClip clip;
		public float timeToPlay = 10f;
		public GameObject toPlayOn;

		public AnimatorOverrideController controller;

		private Animator a;

		// Use this for initialization
		void Start() {
			a = GetComponent<Animator>();
			a.runtimeAnimatorController = controller;

		}

		private float time = 0;

		// Update is called once per frame
		void OnRenderObject() {
			time += Time.deltaTime;
			if (time > a.runtimeAnimatorController.animationClips[0].length)
				time = 0;
			a.runtimeAnimatorController.animationClips[0].SampleAnimation(gameObject, time);
		}

		

		[ContextMenu("Play")]
		void Play() {
			controller["Random"] = clip;
			//a.StartPlayback();
			time = 0;
			a.runtimeAnimatorController.animationClips[0].SampleAnimation(gameObject, 0);
		}


		
	}
}

