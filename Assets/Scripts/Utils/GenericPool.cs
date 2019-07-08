using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPool<T> {

    public List<T> Available = new List<T>();
    public List<T> InUse = new List<T>();

    public Func<T> ObjectConstructor;

    public GenericPool(Func<T> objectConstructor) {
        ObjectConstructor = objectConstructor;
    }

    public T Get() {
        if(Available.Count == 0) {
            Available.Add(ObjectConstructor());
        }

        T obj = Available[0];
        Available.RemoveAt(0);
        InUse.Add(obj);
        return obj;
    }

    public void Return(T obj) {
        if(!InUse.Remove(obj)) {
            throw new Exception("Tried to return object to pool that wasn't in use.");
        }
        Available.Add(obj);
    }

    public void Reset() {
        foreach(var obj in InUse) {
            Available.Add(obj);
        }
        InUse.Clear();
    }
}
