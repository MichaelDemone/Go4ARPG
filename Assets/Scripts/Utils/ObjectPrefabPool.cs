using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPrefabPool {

    public List<GameObject> Available = new List<GameObject>();
    public List<GameObject> InUse = new List<GameObject>();

    private GameObject prefab;
    private Transform parent;

    public ObjectPrefabPool(GameObject prefab, Transform parent, int initialValue = 10) {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < initialValue; i++) {
            var go = Object.Instantiate(prefab, parent);
            go.SetActive(false);
            Available.Add(go);
        }
    }

    public GameObject GetObject() {
        if (Available.Count == 0) {
            Available.Add(Object.Instantiate(prefab, parent));
        }

        GameObject go = Available[0];
        go.SetActive(true);
        Available.RemoveAt(0);
        InUse.Add(go);
        return go;
    }

    public void Return(GameObject go) {
        go.SetActive(false);
        if (!InUse.Remove(go)) {
            throw new Exception("Tried to return object to pool that wasn't in use.");
        }
        Available.Add(go);
    }

    public void Reset() {
        foreach (var go in InUse) {
            go.SetActive(false);
            Available.Add(go);
        }
        InUse.Clear();
    }
}
