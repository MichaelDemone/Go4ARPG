using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEditor;
using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    public enum Rarity
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        VeryRare = 3,
        Legendary = 4,
        Mythical = 5
    }

    [CreateAssetMenu(menuName = "Data/Items/Item")]
    public class Item : ScriptableObject, IID {

        [NonSerialized] public bool CreatedFromOriginal = false;

	    public int ID;
        public string Name = "";
        public Sprite Image;
        public int Value;
        public string Description;
        public Rarity Rarity;
        public bool SellWithTrash = false;

        public Action DataChanged;

        void OnEnable() {
            if (Name == "") Name = name;
        }

	    public int GetID() {
		    return ID;
	    }

        public virtual int GetValue() {
            return Value;
        }

        public virtual bool ShouldCreateNewInstanceWhenPlayerObtained() {
            return false;
        }

        public virtual void OnAfterObtained() {
        }

        public virtual void CopyValues(Item original) {
            ID = original.ID;
            Image = original.Image;
            Value = original.Value;
            Description = original.Description;
            Rarity = original.Rarity;
            Name = original.Name;
        }

        public virtual string GetName() {
            return Name;
        }

        public virtual string GetDescription() {
            return Description;
        }

#if UNITY_EDITOR
	    [ContextMenu("Pick ID")]
	    public void PickID() {
		    ID = IDUtils.PickID<Item>();
	    }
#endif
        
    }


}

