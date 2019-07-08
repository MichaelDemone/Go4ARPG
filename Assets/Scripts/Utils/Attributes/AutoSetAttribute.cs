using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class AutoSet : Attribute {

    public bool CheckChildrenForComponents;
    public bool SetByNameInChildren;
    public string PropertyName; // Could use some kind of reflection to make this work...

    public AutoSet(bool setByNameInChildren = false, bool checkChildrenForComponents = false, [CallerMemberName] string propertyName = null) {
        SetByNameInChildren = setByNameInChildren;
        CheckChildrenForComponents = checkChildrenForComponents;
        PropertyName = propertyName;
    }

    public static void Init(MonoBehaviour parent) {

        Type t = parent.GetType();

        foreach (FieldInfo field in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            AutoSet attr = (AutoSet) GetCustomAttribute(field, typeof(AutoSet));
            if (attr == null) continue;
            if(attr.SetByNameInChildren) {
                foreach(Transform child in parent.transform) {
                    if(child.name == field.Name) {
                        field.SetValue(parent, child.GetComponent(field.FieldType));
                        break;
                    }
                }
            } else if(!attr.CheckChildrenForComponents)
                field.SetValue(parent, parent.GetComponent(field.FieldType));
            else
                field.SetValue(parent, parent.GetComponentInChildren(field.FieldType));

        }
    }
}