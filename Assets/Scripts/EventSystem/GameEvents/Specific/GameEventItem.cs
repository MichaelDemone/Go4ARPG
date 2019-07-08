using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using G4AW2.Data.DropSystem;


namespace CustomEvents {
    [Serializable][CreateAssetMenu(menuName = "SO Architecture/Events/Specific/Item")]
    public class GameEventItem : GameEventGeneric<Item, GameEventItem, UnityEventItem> { }
}
