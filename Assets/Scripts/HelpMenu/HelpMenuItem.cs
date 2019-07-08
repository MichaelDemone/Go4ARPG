using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Misc/HelpMenuItems")]
public class HelpMenuItem : ScriptableObject {
    public string DisplayName;
    [TextArea(30, 40)]
    public string Description;
    public List<HelpMenuItem> SeeAlso;
}
