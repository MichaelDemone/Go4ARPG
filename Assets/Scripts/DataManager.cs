using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public StatTracker StatTracker;
    public PersistentSetCraftingRecipe AllRecipes;
    public PersistentSetItem AllItems;

#if UNITY_EDITOR
    public void UpdateAll() {
        AllRecipes.AddAllOfType();
        AllItems.AddAllOfType();

        while(AllRecipes.Contains(null))
            AllRecipes.Remove(null);
        while(AllItems.Contains(null))
            AllItems.Remove(null);

        foreach (var v in StatTracker.EnemyKillCount.Where(ekc => ekc.Enemy == null)) {
            StatTracker.EnemyKillCount.Remove(v);
        }
        foreach(var v in StatTracker.ItemObtainedCount.Where(ekc => ekc.Item == null)) {
            StatTracker.ItemObtainedCount.Remove(v);
        }

        UnityEditor.AssetDatabase.SaveAssets();
    }

    [ContextMenu("Ensure No ID Dups")]
    public void EnsureNoIDDups() {
        HashSet<int> ids = new HashSet<int>();

        ids.Clear();
        foreach(var thing in AllItems) {
            if(ids.Contains(thing.ID)) {
                Debug.LogWarning("Item has same id as another: " + thing.name);
            }
            ids.Add(thing.ID);
        }

        ids.Clear();
        foreach(var thing in AllRecipes) {
            if(ids.Contains(thing.ID)) {
                Debug.LogWarning("Recipe has same id as another: " + thing.name);
            }
            ids.Add(thing.ID);
        }
    }
#else
     public void UpdateAll() {}
    public void EnsureNoIDDups() {}
#endif

    // Use this for initialization
    void Awake () {
		UpdateAll();
        EnsureNoIDDups();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
