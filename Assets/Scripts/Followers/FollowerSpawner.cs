using System;
using CustomEvents;
using G4AW2.Data;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using G4AW2.Saving;
using UnityEngine;
using Random = UnityEngine.Random;

namespace G4AW2.Followers {
	public class FollowerSpawner : MonoBehaviour {

        public RuntimeSetFollowerData CurrentFollowers;
	    public ActiveQuestBaseVariable CurrentQuest;

		private float currentTimeToReach;
		private float currentDistanceToReach;

		private float currentTime;
		private float currentDistance;

		void Awake() {
            CurrentQuest.OnChange.AddListener(QuestChanged);
		}

	    public void LoadFinished() {
	        DateTime lastTimePlayedUTC = SaveManager.LastTimePlayedUTC;
	        TimeSpan TimeSinceLastPlayed = DateTime.UtcNow - lastTimePlayedUTC;
	        double secondsSinceLastPlayed = TimeSinceLastPlayed.TotalSeconds;
            Debug.Log(secondsSinceLastPlayed);
	        while (true) {
	            secondsSinceLastPlayed -= Random.Range(CurrentQuest.Value.MinEnemyDropTime,
	                CurrentQuest.Value.MaxEnemyDropTime) * 10;
	            if (secondsSinceLastPlayed < 0) {
	                break;
	            }

                AddFollower();
	            if (CurrentFollowers.Value.Count == 10) break;
	        }
	    }

	    void QuestChanged(ActiveQuestBase quest) {
	        currentTime = 0;
	        currentTimeToReach = Random.Range(quest.MinEnemyDropTime, quest.MaxEnemyDropTime);
	        currentDistanceToReach = Random.Range(quest.MinEnemyDropDistance, quest.MaxEnemyDropDistance);
        }

		void Update() {
			currentTime += Time.deltaTime;
			CheckSpawns();
		}

		public void UpdateDistance(float travelled) {
			currentDistance += travelled;
			CheckSpawns();
		}

		private void CheckSpawns() {
			if (currentTime > currentTimeToReach) {
				currentTime -= currentTimeToReach;
				currentTimeToReach = Random.Range(CurrentQuest.Value.MinEnemyDropTime, CurrentQuest.Value.MaxEnemyDropTime);
                AddFollower();
				CheckSpawns();
			}
			if (currentDistance > currentDistanceToReach) {
				currentDistance -= currentDistanceToReach;
				currentDistanceToReach = Random.Range(CurrentQuest.Value.MinEnemyDropDistance, CurrentQuest.Value.MaxEnemyDropDistance);
                AddFollower();
				CheckSpawns();
			}
		}

	    [ContextMenu("Add Follower")]
		public void AddFollower() {
            // randomly choose a follower!
	        if (CurrentFollowers.Value.Count >= 10) return;

	        FollowerData data = CurrentQuest.Value.Enemies.GetRandomFollower(true);
	        if (data == null) return;

            CurrentFollowers.Add(data);
        }

#if UNITY_EDITOR
	    [ContextMenu("Clear Followers")]
	    void ClearFollowers() {
		    CurrentFollowers.Value.Clear();
	    }

	    
#endif
	}
}


