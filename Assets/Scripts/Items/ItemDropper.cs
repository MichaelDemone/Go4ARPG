using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Data.DropSystem {
    [System.Serializable]
    public class ItemDropper {
        public List<ItemAndRarity> Items;

        public List<Item> GetItems(bool addGlobalItems) {
            List<Item> droppedItems = new List<Item>();

            foreach(ItemAndRarity item in Items) {
                float value = Random.value;
                if(item.dropChance > value) {
                    Item itemThatGoesToInventory = item.item;
                    if(item.item.ShouldCreateNewInstanceWhenPlayerObtained()) {
                        itemThatGoesToInventory = Object.Instantiate(item.item);
                        itemThatGoesToInventory.CreatedFromOriginal = true;
                        itemThatGoesToInventory.OnAfterObtained();
                    }
                    droppedItems.Add(itemThatGoesToInventory);
                }
            }

            if(addGlobalItems) droppedItems.AddRange(ItemDropManager.GetRandomDrop());

            return droppedItems;
        }

#if UNITY_EDITOR
        private void DropItem(int times) {
            int drop_total = 0;
            Dictionary<Item, int> counts = new Dictionary<Item, int>();
            for(int i = 0; i < times; i++) {
                List<Item> items = GetItems(false);
                foreach(Item item in items) {
                    int count = 0;
                    if(counts.TryGetValue(item, out count)) {
                        counts.Remove(item);
                    }
                    counts.Add(item, count + 1);
                    drop_total++;
                }
            }

            foreach(KeyValuePair<Item, int> kvp in counts) {
                Debug.Log(string.Format("Key: {0}, Value: {1}", kvp.Key.name, kvp.Value / (float) times));
            }
            Debug.Log("Number of drops: " + drop_total);
        }

        [ContextMenu("Drop 1")]
        private void DropOne() {
            Debug.Log("Drop one");

            List<Item> items = GetItems(false);
            foreach(Item item in items) {
                Debug.Log("Dropped: " + item.name);
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
