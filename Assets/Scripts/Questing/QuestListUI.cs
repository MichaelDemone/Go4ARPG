using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour {

	public UnityEventQuest QuestClicked; 
	public GameObject ListItemPrefab;
	public GameObject ListParent;

	private List<Quest> quests = new List<Quest>();
	private List<GameObject> questItems = new List<GameObject>();

    public RuntimeSetQuest CurrentQuests;

    public void Awake() {
        CurrentQuests.OnChange.AddListener(q => RefreshList());
    }

    public void RefreshList() {
        Clear();
        CurrentQuests.Value.ForEach(AddItem);
    }

	public void AddItem(Quest info) {
		quests.Add(info);
		var gameobject = GameObject.Instantiate(ListItemPrefab);
		gameobject.transform.SetParent(ListParent.transform, false);

		questItems.Add(gameobject);
		gameobject.GetComponent<Button>().onClick.RemoveAllListeners();
		gameobject.GetComponent<Button>().onClick.AddListener(() => QuestClicked.Invoke(info));

		SetContent(info, gameobject);
	}

	private void SetContent(Quest info, GameObject go) {
		go.GetComponentInChildren<TextMeshProUGUI>().text = info.DisplayName;
	}

	public void Clear() {
		quests.Clear();
		questItems.ForEach(Destroy);
		questItems.Clear();
	}

}
