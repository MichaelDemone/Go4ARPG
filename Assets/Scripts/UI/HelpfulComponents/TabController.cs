using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour {

    [Tooltip("First tab is the default tab")]
    public TabAndPanel[] Tabs;

    public GameObject BG;

    private TabAndPanel lastSelectedButton;

	// Use this for initialization
	void Start () {

        if(Tabs.Length == 0)
        {
            throw new System.Exception("No tabs in tab controller");
        }

        
        Transform parent = Tabs[0].Tab.transform.parent;

		foreach(var tab in Tabs)
        {
            if(tab.Tab.transform.parent != parent)
            {
                throw new System.Exception("Tabs must have the same parent");
            }
            SetTab(tab);
            tab.Tab.transform.SetAsFirstSibling();
            tab.Panel.SetActive(false);
        }

        if(BG.transform.parent != parent)
        {
            throw new System.Exception("Background must have same parent as tabs");
        }

        OnClick(Tabs[0]);
	}
	
    private void SetTab(TabAndPanel tab)
    {
        tab.Tab.onClick.AddListener(() => OnClick(tab));
    }

    void OnClick(TabAndPanel tab)
    {
        lastSelectedButton?.Tab.transform.SetAsFirstSibling();
        lastSelectedButton?.Panel.SetActive(false);

        tab.Tab.transform.SetAsLastSibling();
        tab.Panel.SetActive(true);

        lastSelectedButton = tab;
    }

    [System.Serializable]
    public class TabAndPanel
    {
        public Button Tab;
        public GameObject Panel;
    }
}
