using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour {

    public HelpMenuItem[] HelpItems;
    public GameObject HelpItemPrefab;
    public Transform HelpItemParent;
    public ScrollRect MenuItems;
    public HelpMenuPopUp PopUp;

	// Use this for initialization
	void Start () {
	    foreach (var item in HelpItems) {
	        var go = Instantiate(HelpItemPrefab, HelpItemParent);
	        go.GetComponentInChildren<TextMeshProUGUI>().text = item.DisplayName;
            SetClickHandler(go.GetComponent<Button>(), item);
	    }
	}

    public void OnOpen() {
        //MenuItems.normalizedPosition = new Vector2(0, 1);
    }

    void SetClickHandler(Button b, HelpMenuItem item) {
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => {
            PopUp.ShowItem(item);
        });
    }

	// Update is called once per frame
	void Update () {
		
	}
}
