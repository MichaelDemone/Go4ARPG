using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using TMPro;
using UnityEngine;

public class DebugMenu : MonoBehaviour {

    #region Add Enemy

    public TMP_InputField EnemyLevel;
    public TMP_Dropdown EnemyDropdown;
    public PersistentSetFollowerData AllFollowers;
    public RuntimeSetFollowerData CurrentFollowers;

    public void PopulateDropdown() {
        List<TMP_Dropdown.OptionData> dropdownItems = new List<TMP_Dropdown.OptionData>();
        foreach (var follower in AllFollowers) {
            dropdownItems.Add(new TMP_Dropdown.OptionData(follower.DisplayName, follower.Portrait));
        }

        EnemyDropdown.ClearOptions();
        EnemyDropdown.AddOptions(dropdownItems);
    }

    public void DropEnemy() {

        FollowerData data = Instantiate(AllFollowers.ElementAt(EnemyDropdown.value));
        if (data is EnemyData) {
            ((EnemyData) data).Level = int.Parse(EnemyLevel.text);
        }
        data.AfterCreated();

        CurrentFollowers.Add(data);
    }

    #endregion

    // Use this for initialization
    void Start () {
		PopulateDropdown();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
