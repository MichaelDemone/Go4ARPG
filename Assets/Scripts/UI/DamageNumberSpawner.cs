using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour {

    public GameObject DamageNumberPrefab;
    public Transform damageNumberParent;

    private ObjectPrefabPool pool;
    private GenericPool<UpdateTimer> timers;
    
    void Awake() {
        pool = new ObjectPrefabPool(DamageNumberPrefab, damageNumberParent, 5);
        timers = new GenericPool<UpdateTimer>(() => new UpdateTimer());
    }

    void OnEnable() {
        pool.Reset();
        timers.Reset();
    }

    void Update() {

        // Have to make a copy of the list because it changes in the update function
        foreach(UpdateTimer t in timers.InUse.ToList()) { 
            t.Update(Time.deltaTime);
        }
    }

    public void SpawnNumber(int number, Color c) {

        GameObject damageNumber = pool.GetObject();
        ((RectTransform) damageNumber.transform).anchoredPosition = new Vector2(0,0);
        TextMeshProUGUI tmpugui = damageNumber.GetComponent<TextMeshProUGUI>();

        tmpugui.SetText(number.ToString());
        tmpugui.faceColor = c;

        Color outline = Color.black;

        UpdateTimer ut = timers.Get();
        ut.Start(2, 
        () => { // finish
            pool.Return(damageNumber);
            timers.Return(ut);
        }, 
        (percentComplete) => { // update
            c.a = 1 - percentComplete;
            tmpugui.faceColor = c;
            outline.a = c.a;
            tmpugui.outlineColor = outline;
        });
    }

#if UNITY_EDITOR
    [ContextMenu("Spawn Test")]
    public void SpawnNumberTest() {
        SpawnNumber(Random.Range(1,1000), Color.black);
    }
#endif
}
