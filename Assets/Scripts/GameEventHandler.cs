using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using UnityEngine;

public class GameEventHandler : MonoBehaviour {

    public static GameEventHandler Singleton;

    public static Action<ActiveQuestBase> QuestChanged;
    public static Action<EnemyData> EnemyKilled;
    public static Action<Item> LootObtained;

    void Awake() {
        Singleton = this;
    }

    public void OnQuestChanged(ActiveQuestBase quest) {
        QuestChanged?.Invoke(quest);
    }

    public void OnEnemyKilled(EnemyData ed) {
        EnemyKilled?.Invoke(ed);
    }

    public void OnLootObtained(IEnumerable<Item> its) {
        foreach(var it in its) OnLootObtained(it);
    }

    public void OnLootObtained(Item it) {
        LootObtained?.Invoke(it);
    }

    public void OnLootObtained(Item it, int amount) {
        for (int i = 0; i < amount; i++) LootObtained?.Invoke(it);
    }
}
