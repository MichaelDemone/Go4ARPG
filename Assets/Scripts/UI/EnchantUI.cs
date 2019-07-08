using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.UI;

public class EnchantUI : MonoBehaviour {

    public Inventory Inventory;
    public WeaponReference PlayerWeapon;

    public IconWithTextController Weapon;
    public IconWithTextController Enchanter;
    public IconWithTextController Result;

    public ItemViewer ItemViewer;

    public Button EnchantButton;

    public IntVariable NumberOfEnchantedWeapons;

    // Use this for initialization
    void Start () {
		Weapon.SetData(null, 0, "Put weapon to enchant here.", WeaponViewerClicked);
		Enchanter.SetData(null, 0, "Put item to enchant weapon with here.", EnchantViewerClicked);
		Result.SetData(null, 0, "Result of enchantment goes here.", () => {});
        EnchantButton.interactable = false;
    }

    void WeaponViewerClicked() {
        ItemViewer.ShowItemsFromInventoryWhere<Weapon>(ie => !((Weapon)ie.Item).IsEnchanted && !((Weapon) ie.Item).IsTrash(), false, WeaponClicked);
        if(!PlayerWeapon.Value.IsEnchanted)
            ItemViewer.Add<Weapon>(PlayerWeapon, 0, WeaponClicked);
    }

    void WeaponClicked(Weapon w) {
        EnchantButton.interactable = Enchanter.Item.Item != null;
        Weapon.SetData(w, 1, $"{w.GetName()}\nDAM: {w.DamageAtLevel0}", WeaponViewerClicked);
		Result.SetData(null, 0, "Result of enchantment goes here.", () => {});
    }

    void EnchantViewerClicked() {
        ItemViewer.ShowItemsFromInventory<Enchanter>(false, EnchantClicked);
    }

    void EnchantClicked(Enchanter e) {
        EnchantButton.interactable = Weapon.Item.Item != null;
        ItemViewer.Close();
        Enchanter.SetData(e, 1, $"{e.GetName()}\n{e.GetDescription()}", EnchantViewerClicked);
		Result.SetData(null, 0, "Result of enchantment goes here.", () => {});
    }

    public WeaponUI WeaponUI;

    public void Enchant() {
        Weapon w = Weapon.Item.Item as Weapon;
        Enchanter e = Enchanter.Item.Item as Enchanter;

        if (w == null || e == null) return;

        NumberOfEnchantedWeapons.Value++;

        Inventory.Remove(e);
        w.Enchant(e);

        Start();

        Result.SetData(w, 1, "Enchant Damage: " + w.GetEnchantDamage(), () => { WeaponUI.SetWeaponWithDefaults(w); });
    }
}
