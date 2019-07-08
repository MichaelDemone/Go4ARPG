using CustomEvents;
using G4AW2.Dialogue;
using G4AW2.Questing;
using System;
using System.Collections.Generic;
using G4AW2.Data.Area;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour {

    public ActiveQuestBaseVariable CurrentQuest;

    public Dialogue QuestDialogUI;

    public RuntimeSetQuest CurrentQuests;
    public BossQuestController BossController;
    public DragObject DraggableWorld;
    public GameObject ScrollArrow;

    [Header("Events")]
    public UnityEventActiveQuestBase AreaQuestChanged;
    public UnityEvent AreaChanged;

    private Area currentArea = null;

    public void Initialize() {
        currentArea = CurrentQuest.Value.Area;
        if(CurrentQuest.Value.ID == 1) {
            SetCurrentQuest(CurrentQuest.Value);
        } else {

            CurrentQuest.Value.ResumeQuest(FinishQuest);
            AreaQuestChanged.Invoke(CurrentQuest.Value);

            if(CurrentQuest.Value.IsFinished()) {
                CurrentQuest.Value.CleanUp();
                if (CurrentQuest.Value.NextQuest != null) {
                    Debug.LogWarning("Progressing quest?");
                    AdvanceQuestAfterConversation(CurrentQuest.Value);
                }
            } else if(CurrentQuest.Value is BossQuest) {
                BossController.ResumeBossQuest();
            }
        }
    }

    private void FinishQuest(ActiveQuestBase quest) {
        quest.CleanUp();
        if(quest.NextQuest != null) CurrentQuest.Value = quest.NextQuest;
        QuestDialogUI.SetConversation(quest.EndConversation, () => DropRewardAndAdvanceConversation(quest));
    }

    public ItemDropBubbleManager ItemDropManager;
    public Inventory Inventory;

    private void DropRewardAndAdvanceConversation(ActiveQuestBase q) {


        List<Item> todrops = new List<Item>();
        foreach (var reward in q.QuestRewards) {
            Item it = reward.it;
            if(it.ShouldCreateNewInstanceWhenPlayerObtained()) {
                if(it is Weapon) {
                    Weapon w = ScriptableObject.Instantiate(it) as Weapon;
                    w.OnAfterObtained();
                    w.Level = reward.Level;
                    if(reward.RandomRoll != -1) {
                        w.Random = reward.RandomRoll;
                        w.SetValuesBasedOnRandom();
                    }
                    it = w;
                } else if(it is Armor) {
                    Armor a = ScriptableObject.Instantiate(it) as Armor;
                    a.OnAfterObtained();
                    a.Level = reward.Level;
                    if(reward.RandomRoll != -1) {
                        a.Random = reward.RandomRoll;
                        a.SetValuesBasedOnRandom();
                    }
                    it = a;
                } else if(it is Headgear) {
                    Headgear a = ScriptableObject.Instantiate(it) as Headgear;
                    a.OnAfterObtained();
                    a.Level = reward.Level;
                    if(reward.RandomRoll != -1) {
                        a.RandomRoll = reward.RandomRoll;
                        a.SetValuesBasedOnRandom();
                    }
                    it = a;
                }
            }
            todrops.Add(it);
        }

        if (todrops.Count == 0) {
            AdvanceQuestAfterConversation(q);
        }
        else {
            ItemDropManager.Clear();

            DraggableWorld.Disable2();
            ScrollArrow.SetActive(false);

            Inventory.AddItems(todrops);
            int amount = 0;
            ItemDropManager.AddItems(todrops, () => {
                amount++;
                if (amount >= todrops.Count) {
                    ScrollArrow.SetActive(true);
                    DraggableWorld.Enable2();
                    AdvanceQuestAfterConversation(q);
                }
            });
        }
    }

    private void AdvanceQuestAfterConversation(ActiveQuestBase q) {

        q.CleanUp();

        if(q.NextQuest == null) {
            PopUp.SetPopUp(
                "You finished the quest! You may either continue in this area or switch quests using the quest book on your screen.",
                new[] { "ok" }, new Action[] {
                    () => { }
                });
            return;
        }

        SetCurrentQuest(q.NextQuest);
    }

    public RobustLerperSerialized AreaChangeInterpolater;

    void Update() {
        AreaChangeInterpolater.Update(Time.deltaTime);
    }


    public void SetCurrentQuest(ActiveQuestBase quest) {

        ItemDropManager.Clear();
        quest.CleanUp();

        if (quest.Area != currentArea) {
            AreaChangeInterpolater.StartLerping(() => {

                AreaChanged.Invoke();

                CurrentQuest.Value = quest;
                AreaQuestChanged.Invoke(quest);
                quest.StartQuest(FinishQuest);
                
                AreaChangeInterpolater.StartReverseLerp(() => {
                    QuestDialogUI.SetConversation(quest.StartConversation, () => {

                        if(quest is BossQuest) {
                            BossController.StartBossQuest();
                        }

                        // Check that the quest isn't finished (for reach quests)
                        if(quest.IsFinished()) FinishQuest(quest);
                    });
                });
            });
        }
        else {
            CurrentQuest.Value = quest;
            AreaQuestChanged.Invoke(quest);
            quest.StartQuest(FinishQuest);
            QuestDialogUI.SetConversation(quest.StartConversation, () => {

                if(quest is BossQuest) {
                    BossController.StartBossQuest();
                }

                if(quest.IsFinished())
                    FinishQuest(quest);
            });
        }

        currentArea = quest.Area;
    }

    public GameObject Journal;

    public void QuestClicked(Quest q) {
        //TODO: Show some sort of info on the quest.
        if(!(q is ActiveQuestBase)) {
            return;
        }

        PopUp.SetPopUp($"{q.DisplayName}\nWhat would you like to do?", new[] {"Set Active", "Remove", "Cancel"},
            new Action[] {
                () => {
                    // Set Active
                    if (!CurrentQuest.Value.IsFinished()) {
                        if (!(CurrentQuest.Value is ReachValueQuest)) {
                            PopUp.SetPopUp(
                                "Are you sure you want to switch quests? You will lose all progress in this one.",
                                new[] {"Yep", "Nope"}, new Action[] {
                                    () => {
                                        CurrentQuests.Add(CurrentQuest);
                                        CurrentQuest.Value.CleanUp();
                                        CurrentQuests.Remove(q);

                                        SetCurrentQuest((ActiveQuestBase) q);
                                    },
                                    () => { }
                                });
                        }
                        else {
                            CurrentQuests.Add(CurrentQuest);
                            CurrentQuest.Value.CleanUp();
                            CurrentQuests.Remove(q);
                            SetCurrentQuest((ActiveQuestBase) q);
                        }
                    }
                    else {
                        // You've already completed the quest
                        CurrentQuests.Remove(q);
                        SetCurrentQuest((ActiveQuestBase) q);
                    }
                    Journal.SetActive(false);

                },
                () => {
                    // Remove
                    CurrentQuests.Remove(q);
                },
                () => {
                    // Cancel

                }
            });
    }

    public ActiveQuestBase TestQuest;

    [ContextMenu("Set Quest from test quest")]
    public void SetQuestFromTestQuest() {
        CurrentQuest.Value.CleanUp();
        SetCurrentQuest(TestQuest);
    }
}
