using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Follower/Shop Follower")]
    public class ShopFollower : FollowerData, ISaveable {
        public AnimationClip WalkingAnimation;

        public PersistentSetItem AllItems;
        public List<SellableItem> Items;
        public float BuyingPriceMultiplier = 1.5f;
        public float SellingPriceMultiplier = 0.5f;

        [Serializable]
        public class SellableItem {
            public Item Item;
            public int Amount;
            public int Level;

            public SellableItem(Item it, int amount, int level) {
                Item = it;
                Amount = amount;
                Level = level;
            }

            public SellableItem(string data, PersistentSetItem allItems) {
                StringReader sr = new StringReader(data);
                int id = int.Parse(sr.ReadLine());
                int amount = int.Parse(sr.ReadLine());
                int level = int.Parse(sr.ReadLine());
                string additionalInfo = sr.ReadLine();
                Item original = allItems.First(it => it.ID == id);
                if (original is ISaveable) {
                    Item = Instantiate(original);
                    Item.CreatedFromOriginal = true;
                    ((ISaveable) Item).SetData(additionalInfo, original);
                }
                else {
                    Item = original;
                }
                Amount = amount;
                Level = level;
            }

            public string GetData() {
                string additionalInfo = "";
                if (Item is ISaveable) {
                    additionalInfo = ((ISaveable) Item).GetSaveString();
                }
                return $"{Item.ID}\n{Amount}\n{Level}\n{additionalInfo}";
            }
        }

        public override void AfterCreated() {
            foreach (var item in Items.ToList()) {
                if (item.Item.ShouldCreateNewInstanceWhenPlayerObtained()) {
                    Item it = Instantiate(item.Item);
                    it.CreatedFromOriginal = true;
                    it.OnAfterObtained();

                    if (it is Weapon) {
                        ((Weapon) it).Level = item.Level;
                    } else if (it is Armor) {
                        ((Armor) it).Level = item.Level;
                    } else if (it is Headgear) {
                        ((Headgear) it).Level = item.Level;
                    }

                    Items.Remove(item);
                    var sell = new SellableItem(it, 1, -1 /*level doesn't matter anymore*/);
                    Items.Add(sell);
                }
            }
        }

        private class SaveObject {
            public int ID;
            public List<string> Entries;
        }

        public override string GetSaveString() {

            return JsonUtility.ToJson(new SaveObject() {
                ID = ID,
                Entries = Items.Select(it => it.GetData()).ToList()
            });
        }

        public override void SetData(string saveString, params object[] otherData) {
            SaveObject ds = JsonUtility.FromJson<SaveObject>(saveString);

            ID = ds.ID;

            if (AllItems != null && ds.Entries != null) {
                Items.Clear();

                foreach(string data in ds.Entries) {
                    Items.Add(new SellableItem(data, AllItems));
                }
            }

            FollowerData original;

            if(otherData[0] is PersistentSetFollowerData) {
                PersistentSetFollowerData allFollowers = (PersistentSetFollowerData) otherData[0];
                original = allFollowers.First(it => it.ID == ID);
            } else {
                original = otherData[0] as FollowerData;
                if (SizeOfSprite == original.SizeOfSprite && SideIdleAnimation == original.SideIdleAnimation) {


                    return; // This object may have been create based on the original. In which case, we don't need to do any copying
                }
                    
            }

            ID = original.ID;
            SizeOfSprite = original.SizeOfSprite;
            SideIdleAnimation = original.SideIdleAnimation;
            RandomAnimation = original.RandomAnimation;
            MinTimeBetweenRandomAnims = original.MinTimeBetweenRandomAnims;
            MaxTimeBetweenRandomAnims = original.MaxTimeBetweenRandomAnims;
            AllItems = ((ShopFollower) original).AllItems;
            if (ds.Entries == null) {
                Items = ((ShopFollower) original).Items;
            }

            AfterCreated();
        }
    }
}


