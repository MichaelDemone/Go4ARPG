using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {

	public class RuntimeSetGeneric<T, TEvent> : ScriptableObject, IEnumerable<T> where TEvent : UnityEvent<T> {
		public List<T> Value { get { return list; } }
		private List<T> list = new List<T>();

		public TEvent OnAdd, OnRemove, OnChange;

		public void AddRange(IEnumerable<T> items) {
		    foreach (var item in items) {
		        Add(item, true);
		        OnAdd.Invoke(item);
		    }
			OnChange.Invoke(default(T));
		}

		public void RemoveAt(int index) {
		    T item = list[index];
			list.RemoveAt(index);
            OnRemove.Invoke(item);
			OnChange.Invoke(default(T));
		}

		public void Insert(int index, T item ) {
			list.Insert(index, item);
		    OnAdd.Invoke(item);
			OnChange.Invoke(default(T));
		}

        public void Add(T item) {
            Add(item, false);
        }

		public void Add( T item, bool silently = false) {
			list.Add(item);
			if (!silently) {
				OnAdd.Invoke(item);
				OnChange.Invoke(item);
			}
		}

		public bool Contains(T it) => Value.Contains(it);

		public void Remove( T item ) {
			list.Remove(item);
			OnChange.Invoke(item);
			OnRemove.Invoke(item);
		}

		public void Clear() {
			list.Clear();
			OnChange.Invoke(default(T));
		}

		public static implicit operator List<T>(RuntimeSetGeneric<T, TEvent> val) {
			return val.list;
		}

		public T this[int i] {
			get { return list[i]; }
			set { list[i] = value; }
		}

		private void OnEnable() {
			Clear();
		}

        public IEnumerator<T> GetEnumerator() {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Value.GetEnumerator();
        }
    }
}


