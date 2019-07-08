using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using G4AW2.Questing;
using UnityEngine;

namespace G4AW2.Followers {
	[CreateAssetMenu(menuName = "Data/Follower/QuestGiver")]
	public class QuestGiver : FollowerData {

		public Quest QuestToGive;
		public AnimationClip GivingQuest;
	    public AnimationClip WalkingAnimation;
	}
}

