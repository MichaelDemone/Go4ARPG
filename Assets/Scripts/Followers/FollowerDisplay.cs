using System;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace G4AW2.Followers {

	[RequireComponent(typeof(Animator))]
	public class FollowerDisplay : MonoBehaviour, IPointerClickHandler {
		[NonSerialized] public FollowerData Data;

		public AnimatorOverrideController BaseController;
        public TextMeshProUGUI followerID;

		public Action<FollowerDisplay> FollowerClicked;
		private Animator Animator;

		private AnimatorOverrideController aoc;

		private float currentTime = 0;
		private float doRandomTime = 0;

		void Awake() {
			Animator = GetComponent<Animator>();
			aoc = new AnimatorOverrideController(BaseController);
			Animator.runtimeAnimatorController = aoc;

			if (Data != null) {
				aoc["Idle"] = Data.SideIdleAnimation;
				if (Data.HasRandomAnimation)
					aoc["Random"] = Data.RandomAnimation;
			}
			
		}
		void Update() {
			if (Data.HasRandomAnimation) {
				currentTime += Time.deltaTime;
				if (currentTime > doRandomTime) {
					currentTime = 0;
					Animator.SetTrigger("Random");
					doRandomTime = Random.Range(Data.MinTimeBetweenRandomAnims, Data.MaxTimeBetweenRandomAnims);
				}
			}
		}

		public void SetData(FollowerData data) {
			Data = data;

			aoc["Idle"] = Data.SideIdleAnimation;
			if (Data.HasRandomAnimation) {
				aoc["Random"] = Data.RandomAnimation;
				doRandomTime = Random.Range(Data.MinTimeBetweenRandomAnims, Data.MaxTimeBetweenRandomAnims);
		        currentTime = 0;
            }
            if (Data is EnemyData) {
                EnemyData e = (EnemyData) data;
                followerID.text = $"LVL {e.Level}\n{e.DisplayName}";
            } else if (Data is ShopFollower) {
                followerID.text = data.DisplayName;
            } else if (Data is QuestGiver) {
                followerID.text = data.DisplayName;
            } else {
                followerID.text = "";
            }
		}

		public void OnPointerClick(PointerEventData eventData) {
			FollowerClicked.Invoke(this);
		}
	}

}

