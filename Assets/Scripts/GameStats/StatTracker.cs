using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StatTracker : MonoBehaviour {

    [Serializable]
    public class EnemyCounter {
        public EnemyData Enemy;
        public IntVariable Count;
    }

    public List<EnemyCounter> EnemyKillCount;
    private Dictionary<int, IntVariable> enemyDictionary;


    [Serializable]
    public class ItemCounter {
        public Item Item;
        public IntVariable Count;
    }

    public List<ItemCounter> ItemObtainedCount;
    private Dictionary<int, IntVariable> itemDictionary;

    void Awake() {
        GameEventHandler.EnemyKilled += EnemyKilled;
        GameEventHandler.LootObtained += LootObtained;

        itemDictionary = new Dictionary<int, IntVariable>();
        foreach(ItemCounter item in ItemObtainedCount) {
            itemDictionary.Add(item.Item.ID, item.Count);
        }

        enemyDictionary = new Dictionary<int, IntVariable>();
        foreach(EnemyCounter monster in EnemyKillCount) {
            enemyDictionary.Add(monster.Enemy.ID, monster.Count);
        }
    }

    private void LootObtained(Item item) {
        if(itemDictionary.ContainsKey(item.ID)) {
            itemDictionary[item.ID].Value++;
        }
    }

    private void EnemyKilled(EnemyData enemyData) {
        if(enemyDictionary.ContainsKey(enemyData.ID)) {
            enemyDictionary[enemyData.ID].Value++;
        }
    }


#if UNITY_EDITOR
    [ContextMenu("Create Variables For Enemies")]
    public void CreateVariablesToTrackEnemies() {
        string[] paths = AssetDatabase.FindAssets("t:" + typeof(EnemyData).Name);
        for(int i = 0; i < paths.Length; i++) {
            paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
        }

        List<EnemyData> enemies = paths.Select(AssetDatabase.LoadAssetAtPath<EnemyData>).ToList();

        foreach (var enemyData in enemies) {
            string variableTitle = $"{enemyData.DisplayName}sKilled";
            if (AssetDatabase.FindAssets(variableTitle).Length != 0) {
                continue;
            }
            IntVariable variable = ScriptableObject.CreateInstance<IntVariable>();
            variable.Value = 0;
            
            AssetDatabase.CreateAsset(variable, $"Assets/Data/StatVariables/Killed/{variableTitle}.asset");
            EnemyKillCount.Add(new EnemyCounter() {
                Enemy = enemyData,
                Count = variable
            });
        }
    }

    [ContextMenu("Create Variables For Items")]
    public void CreateVariablesToTrackItems() {
        string[] paths = AssetDatabase.FindAssets("t:" + typeof(Item).Name);
        for(int i = 0; i < paths.Length; i++) {
            paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
        }

        List<Item> items = paths.Select(AssetDatabase.LoadAssetAtPath<Item>).ToList();

        foreach(var item in items) {
            string variableTitle = $"{item.Name}sCollected";
            if(AssetDatabase.FindAssets(variableTitle).Length != 0) {
                continue;
            }
            IntVariable variable = ScriptableObject.CreateInstance<IntVariable>();
            variable.Value = 0;

            AssetDatabase.CreateAsset(variable, $"Assets/Data/StatVariables/Collected/{variableTitle}.asset");
            ItemObtainedCount.Add(new ItemCounter() {
                Item = item,
                Count = variable
            });
        }
    }
#endif

}
