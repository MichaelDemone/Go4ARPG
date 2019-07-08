using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShopUI : MonoBehaviour {

    public RuntimeSetFollowerData Followers;
    public Inventory Inventory;
    public IntReference GoldAmount;

    public GameObject IconWithTextPrefab;
    public GameObject BuyingScrollPanelContent;
    public GameObject SellingScrollPanelContent;

    public TextMeshProUGUI TrashButtonText;

    public UnityEvent OnFinish;

    private ShopFollower shopKeep;

    private List<GameObject> buyingItems = new List<GameObject>();
    private List<GameObject> sellingItems = new List<GameObject>();

    private Action actionOnSendAway;

    public void OpenShop(ShopFollower shopKeep, Action actionOnSendAway) {
        this.actionOnSendAway = actionOnSendAway;
        this.shopKeep = shopKeep;
        gameObject.SetActive(true);
        GetComponent<RobustLerper>().StartLerping();

        RefreshBuyingList();
        SetSellingTab();
    }

    public void Finish() {
        actionOnSendAway?.Invoke();
        Followers.Remove(shopKeep);
        GetComponent<RobustLerper>().StartReverseLerp();
        OnFinish.Invoke();
    }

    public void SetSellingTab() {
        RefreshSellingList();
        UpdateTrashButtonText();
    }

    public void UpdateTrashButtonText() {
        TrashButtonText.text = $"Sell Trash + Shards ({GetTrashSum()} gold)";
    }

    private int GetTrashSum() {
        int sum = 0;
        foreach(var i in Inventory) {

            if(i.Item is ITrashable && ((ITrashable) i.Item).IsTrash()) {
                sum += Mathf.RoundToInt(i.Item.GetValue() * i.Amount * (shopKeep.SellingPriceMultiplier));
            }

            if(i.Item.SellWithTrash) {
                sum += Mathf.RoundToInt(i.Item.GetValue() * i.Amount * (shopKeep.SellingPriceMultiplier));
            }
        }
        return sum;
    }

    public void SellTrash() {
        List<InventoryEntry> toRemove = new List<InventoryEntry>();
        foreach(var i in Inventory) {
            if(i.Item is ITrashable && ((ITrashable) i.Item).IsTrash()) {
                toRemove.Add(i);
            }
            if(i.Item.SellWithTrash) {
                toRemove.Add(i);
            }
        }

        GoldAmount.Value += GetTrashSum();
        toRemove.ForEach(i => Inventory.Remove(i));

        SetSellingTab();
    }

    #region Buying
    public void RefreshBuyingList() {
        buyingItems.ForEach(Destroy);
        buyingItems.Clear();

        foreach(var item in shopKeep.Items) {
            var go = Instantiate(IconWithTextPrefab, BuyingScrollPanelContent.transform);
            var itemDisplay = go.GetComponent<IconWithTextController>();
            SetDataBuying(itemDisplay, item);
            buyingItems.Add(go);
        }
    }

    private int GetBuyingPrice(Item i) {
        return Mathf.Max(Mathf.RoundToInt(i.GetValue() * shopKeep.BuyingPriceMultiplier), 1);
    }

    private void SetDataBuying(IconWithTextController itc, ShopFollower.SellableItem iid) {
        int price = GetBuyingPrice(iid.Item);

        string text = $"{iid.Item.GetName()}\n{price} gold each";
        itc.SetData(iid.Item, iid.Amount, text, () => ItemClickedBuying(iid));
    }

    private void ItemClickedBuying(ShopFollower.SellableItem it) {
        int price = GetBuyingPrice(it.Item);

        if (GoldAmount.Value < price) {
            PopUp.SetPopUp("Not Enough Gold.", new[] {":(", "):"}, new Action[] {() => { }, () => { }});
            return;
        }

        string title =
            $"Would you like to buy a {it.Item.GetName()} for {price} gold?\nAmount Left: {it.Amount}\n\n{it.Item.GetDescription()}";

        PopUp.SetPopUp(title, new string[] { "Buy All", "Buy 1", "No" },
            new Action[] {
                () => {
                    int amt = 0;
                    while (it.Amount > 0 && GoldAmount >= price) {
                        GoldAmount.Value -= price;
                        Inventory.Add(it.Item, 1);
                        it.Amount -= 1;
                        if(it.Amount == 0) shopKeep.Items.Remove(it);
                        amt++;
                    }

                    RefreshBuyingList();
                },
                () => {
                    GoldAmount.Value -= price;
                    Inventory.Add(it.Item, 1);
                    it.Amount -= 1;
                    if(it.Amount == 0) shopKeep.Items.Remove(it);

                    if (it.Amount > 0) ItemClickedBuying(it);
                    RefreshBuyingList();
                },
                () => {
                    RefreshBuyingList();
                }
            });
    }


    #endregion

    #region Selling
    public void RefreshSellingList() {
        sellingItems.ForEach(Destroy);
        sellingItems.Clear();

        foreach(var item in Inventory) {
            var go = Instantiate(IconWithTextPrefab, SellingScrollPanelContent.transform);
            var itemDisplay = go.GetComponent<IconWithTextController>();
            SetDataSelling(itemDisplay, item);
            sellingItems.Add(go);
        }
    }

    private int GetSellingPrice(Item i) {
        return Mathf.Max(Mathf.RoundToInt(i.GetValue() * shopKeep.SellingPriceMultiplier), 1);
    }

    private void SetDataSelling(IconWithTextController itc, InventoryEntry iid) {
        string text = iid.Item.GetName() + "\n" + GetSellingPrice(iid.Item) + " gold each";
        itc.SetData(iid.Item, iid.Amount, text, () => ItemClickedSelling(iid));
    }

    private void ItemClickedSelling(InventoryEntry it) {

        int price = GetSellingPrice(it.Item);

        string amountLeft = it.Amount > 1 ? $"Amount Left: {it.Amount}\n" : "";

        string title = string.Format($"Would you like to sell a {it.Item.GetName()} for {price} gold?\n{amountLeft}\n{it.Item.GetDescription()}");

        PopUp.SetPopUp(title, new string[] { "Sell All", "Sell 1", "No" },
            new Action[] {
                () => {
                    while (it.Amount > 0) {
                        GoldAmount.Value += price;
                        Inventory.Remove(it.Item, 1);
                    }
                    RefreshSellingList();
                },
                () => {
                    GoldAmount.Value += price;
                    Inventory.Remove(it.Item, 1);
                    if (it.Amount != 0) {
                        ItemClickedSelling(it);
                    }
                    RefreshSellingList();
                },
                () => {
                    RefreshSellingList();
                }
            });
    }

    #endregion
}
