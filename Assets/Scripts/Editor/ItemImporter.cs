using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemImporter : EditorWindow {

    [MenuItem("Tools/Item Importer")]
    static void Init() {
        EditorWindow window = GetWindowWithRect(typeof(ItemImporter), new Rect(0, 0, 400, 400));
        window.Show();
    }

    void OnEnable() {
        
    }

    private string itemName;
    private Texture2D itemSprite = null;

    void OnGUI() {
        itemName = EditorGUILayout.TextField("Item Name", itemName);

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);

        SerializedProperty textureProp = so.FindProperty(nameof(itemSprite));
        EditorGUILayout.PropertyField(textureProp, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties

        so.Update();
        so.ApplyModifiedProperties(); // Remember to apply modified properties


    }
}
