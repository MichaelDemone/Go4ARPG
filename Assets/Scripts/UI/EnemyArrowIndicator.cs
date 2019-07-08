using CustomEvents;
using G4AW2.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyArrowIndicator : MonoBehaviour {

    public RuntimeSetFollowerData Followers;
    public Image Arrow;
    public TextMeshProUGUI NumberofFollowersText;

    public bool OnMainScreen = true;
    public bool HasFollowers = true;

    private void Awake() {
        Followers.OnChange.AddListener(FollowersChanged);
    }

    public void FollowersChanged(FollowerData data) {
        HasFollowers = Followers.Value.Count > 0;

        Arrow.enabled = OnMainScreen && HasFollowers;
        NumberofFollowersText.gameObject.SetActive(OnMainScreen && HasFollowers);
       
        NumberofFollowersText.text = "x" + Followers.Value.Count;
    }

    public void SetOnMainScreen(bool val) {
        OnMainScreen = val;

        Arrow.enabled = OnMainScreen && HasFollowers;
        NumberofFollowersText.gameObject.SetActive(OnMainScreen && HasFollowers);

    }
}
