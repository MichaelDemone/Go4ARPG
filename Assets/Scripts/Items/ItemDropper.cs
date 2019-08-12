using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Data.DropSystem {
    [System.Serializable]
    public class ItemDropper {
        public List<ItemAndRarity> Items;

        public List<IItem> GetItems(bool addGlobalItems) {
            List<IItem> droppedItems = new List<IItem>();

            foreach(ItemAndRarity item in Items) {
                float value = Random.value;
                if(item.dropChance > value) {
                    Item itemThatGoesToInventory = item.item;
                    if (itemThatGoesToInventory is Weapon) {
                        WeaponInstance wi = new WeaponInstance((Weapon) itemThatGoesToInventory, 1, null);
                        droppedItems.Add(wi);
                    }
                    else if (itemThatGoesToInventory is Armor) {
                        ArmorInstance wi = new ArmorInstance((Armor) itemThatGoesToInventory, 1);
                        droppedItems.Add(wi);
                    }
                    else if (itemThatGoesToInventory is Headgear) {
                        HeadgearInstance wi = new HeadgearInstance((Headgear) itemThatGoesToInventory, 1);
                        droppedItems.Add(wi);
                    }
                    else {
                        ItemInstance ii = new ItemInstance(itemThatGoesToInventory, 1);
                        droppedItems.Add(ii);
                    }
                }
            }

            return droppedItems;
        }

#if UNITY_EDITOR
        private void DropItem(int times) {
            int drop_total = 0;
            Dictionary<IItem, int> counts = new Dictionary<IItem, int>();
            for(int i = 0; i < times; i++) {
                List<IItem> items = GetItems(false);
                foreach(IItem item in items) {
                    int count = 0;
                    if(counts.TryGetValue(item, out count)) {
                        counts.Remove(item);
                    }
                    counts.Add(item, count + 1);
                    drop_total++;
                }
            }

            foreach(KeyValuePair<IItem, int> kvp in counts) {
                Debug.Log(string.Format("Key: {0}, Value: {1}", kvp.Key.GetName(), kvp.Value / (float) times));
            }
            Debug.Log("Number of drops: " + drop_total);
        }

        [ContextMenu("Drop 1")]
        private void DropOne() {
            Debug.Log("Drop one");

            List<IItem> items = GetItems(false);
            foreach(IItem item in items) {
                Debug.Log("Dropped: " + item.GetName());
            }
        }

        [ContextMenu("Drop 10")]
        private void Drop10() {
            Debug.Log("Drop 10");
            DropItem(10);
        }

        [ContextMenu("Drop 1000")]
        private void Drop1000() {
            Debug.Log("Drop 1000");
            DropItem(1000);
        }

        [ContextMenu("Drop 1000000")]
        private void Drop1000000() {
            Debug.Log("Drop 1000000");
            DropItem(1000000);
        }
#endif

    }


}
