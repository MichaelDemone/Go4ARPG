using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.Combat;
using UnityEngine;
using UnityEngine.UI;

public class DeadEnemyController : MonoBehaviour {

    public BoolReference Scrolling;
    public float ScrollSpeed = 5;
    public int EndingPosition = -527;

    public GameObject DeadEnemyPrefab;
    public Transform DeadEnemyParent;

    private ObjectPrefabPool DeadEnemies;

    void Awake() {
        DeadEnemies = new ObjectPrefabPool(DeadEnemyPrefab, DeadEnemyParent);
    }

    public void ClearEnemies() {
        StopAllCoroutines();
        DeadEnemies.Reset();
    }

    public void AddDeadEnemy(float x, float y, EnemyData s) {

        GameObject go = DeadEnemies.GetObject();

        go.GetComponent<Image>().sprite = s.DeadSprite;
        RectTransform rt = ((RectTransform) go.transform);
        Vector2 r = rt.sizeDelta;
        r.x = s.SizeOfSprite.x;
        r.y = s.SizeOfSprite.y;
        rt.sizeDelta = r;

        Vector3 pos = rt.anchoredPosition;
        pos.x = x;
        pos.y = y;
        rt.anchoredPosition = pos;

        StartCoroutine(ScrollOff(go));
    }

    IEnumerator ScrollOff(GameObject go) {

        RectTransform rt = go.GetComponent<RectTransform>();

        while(rt.anchoredPosition.x > EndingPosition) {
            if(Scrolling) {
                Vector3 pos = rt.anchoredPosition;
                pos.x -= ScrollSpeed * Time.deltaTime;
                rt.anchoredPosition = pos;
            }
            yield return null;
        }

        DeadEnemies.Return(go);
    }
}
