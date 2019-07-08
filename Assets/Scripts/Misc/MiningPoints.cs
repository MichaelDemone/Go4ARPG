using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.Area;
using G4AW2.Dialogue;
using G4AW2.Questing;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MiningPoints : MonoBehaviour {


    public int ReturnToPoolXValue;

    public GameObject PointPrefab;
    public Transform Parent;

    public FloatReference ScrollSpeed;
    public BoolReference IsScrolling;

    public Inventory PlayerInventory;

    private ObjectPrefabPool Pool;

    private List<MiningPoint> areaPoints = new List<MiningPoint>();
    private List<float> nextSpawnDistance = new List<float>();

    private Area currentArea;

    public void QuestChanged(ActiveQuestBase a) {
        if (currentArea != a.Area) {
            currentArea = a.Area;
            Pool.Reset();
        }

        areaPoints = a.MiningPoints;
        nextSpawnDistance.Clear();
        foreach(var point in areaPoints) {
            nextSpawnDistance.Add(Random.Range(point.MinDistanceBetween, point.MaxDistanceBetween));
        }
    }

    void Awake() {
        Pool = new ObjectPrefabPool(PointPrefab, Parent, 3);
    }

	// Update is called once per frame
	void Update () {
	    if (IsScrolling) {
	        float scrollSpeed = ScrollSpeed;

	        for (int i = 0; i < nextSpawnDistance.Count; i++) {
	            nextSpawnDistance[i] -= scrollSpeed * Time.deltaTime;
	            if (nextSpawnDistance[i] <= 0) {
	                MiningPoint point = areaPoints[i];
	                GameObject go = Pool.GetObject();
                    Image im = go.GetComponent<Image>();
                    im.sprite = point.Image;
                    im.SetNativeSize();
	                AddListener(go.GetComponent<ClickReceiver>(), point);

	                Vector3 pos = go.transform.localPosition;
	                pos.x = 79;
	                go.transform.localPosition = pos;

                    nextSpawnDistance[i] = Random.Range(point.MinDistanceBetween, point.MaxDistanceBetween);
	            }
	        }

	        foreach (var point in Pool.InUse.ToArray()) {
	            Vector3 pos = point.transform.localPosition;
	            pos.x -= scrollSpeed * Time.deltaTime;
	            if (pos.x <= ReturnToPoolXValue) {
	                Pool.Return(point);
	            }

	            point.transform.localPosition = pos;
	        }
        }
	}

    void AddListener(ClickReceiver cr, MiningPoint point) {
        cr.MouseClick2D.RemoveAllListeners();
        cr.MouseClick2D.AddListener((v) => {
            var items = point.Drops.GetItems(false);
            string itemText = "";
            foreach (var item in items) {
                itemText += $"A {item.GetName()}\n";
                PlayerInventory.Add(item);
            }
            if (items.Count == 0) {
                PopUp.SetPopUp("The mining point breaks apart and you get nothing :(", new[] {"Shucks!"}, new Action[] {() => { }});
            } else {
                PopUp.SetPopUp("You got: " + itemText, new[] {"Awesome!"}, new Action[] {() => { }});
            }
            Pool.Return(cr.gameObject);
        });
    }
}
