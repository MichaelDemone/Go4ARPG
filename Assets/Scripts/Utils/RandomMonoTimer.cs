using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Utils {

    public class RandomMonoTimer : MonoBehaviour {

        public float MinTime = 0;
        public float MaxTime = 1;
        public UnityEvent OnFinish;

        // TODO: Make this actually read only through an editor script
        [ReadOnly] public float ThrowTime;
		[ReadOnly] public float CurrentTime;

        // Use this for initialization
        void Start() {
            CurrentTime = 0;
	        ThrowTime = Random.Range(MinTime, MaxTime);
        }

        // Update is called once per frame
        void Update() {
            CurrentTime += Time.deltaTime;
            if (CurrentTime > ThrowTime) {
                OnFinish.Invoke();
                CurrentTime = 0;
                ThrowTime = Random.Range(MinTime, MaxTime);
            }
        }
    }
}

