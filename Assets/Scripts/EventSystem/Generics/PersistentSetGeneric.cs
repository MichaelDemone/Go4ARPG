using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PersistentSetGeneric<T, TEvent> : ScriptableObject, IEnumerable<T> where TEvent : UnityEvent<T> {
    [SerializeField] private List<T> Value;

    public TEvent OnAdd, OnRemove, OnChange;

	public int Count => Value.Count;

	public void Add(T item) {
		if (!Value.Contains(item)) {
			Value.Add(item);
			OnAdd.Invoke(item);
			OnChange.Invoke(item);
		}
	}

	public void Remove( T item ) {
		if (Value.Contains(item)) {
			Value.Remove(item);
			OnChange.Invoke(item);
			OnRemove.Invoke(item);
		}
	}

	public void ForEach(Action<T> func) {
		Value.ForEach(func);
	}

	public void AddRange(IEnumerable<T> range) {
		Value.AddRange(range);
	}

	public void Clear() {
		Value.Clear();
	}

    public IEnumerator<T> GetEnumerator() {
        return Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return Value.GetEnumerator();
    }
}
