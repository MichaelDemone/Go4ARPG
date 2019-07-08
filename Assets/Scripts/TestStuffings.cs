using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStuffings : MonoBehaviour {
    public int AmountToCopy;

    public GameObject Prefab;

    public Transform PrefabParent;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AmountToCopy; i++) {
            Instantiate(Prefab, PrefabParent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
