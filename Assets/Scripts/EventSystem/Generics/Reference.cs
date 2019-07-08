using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
    [Serializable]
	public abstract class Reference<T, TVar, TEvent> where TEvent : UnityEvent<T>,new() where TVar : Variable<T, TEvent> {

		public bool UseConstant = true;
		public T ConstantValue;
		public TVar Variable;

		public Reference() { }

		public Reference( T value ) {
			UseConstant = true;
			ConstantValue = value;
		}

		public T Value {
			get { return UseConstant ? ConstantValue : (Variable == null ? default(T) : Variable.Value); }
			set {
				if (UseConstant) {
					ConstantValue = value;
				}
				else {
					Variable.Value = value;
				}
			}
		}

	    public static implicit operator T(Reference<T, TVar, TEvent> val) {
		    return val.Value;
	    }
	}


}

