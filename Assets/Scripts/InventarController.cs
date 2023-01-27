using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarController : MonoBehaviour{
    public GameObject[] items;
    public GameObject[] weapons;

    public void MakeAvailabel(Config.Item item) {
        switch (item) {
            case Config.Item.AmmoBox:
                Instantiate(items[0]);
                break;
            case Config.Item.HealingPotion:
                Instantiate(items[1]);
                break;
            case Config.Item.RepairKit:
                Instantiate(items[2]);
                break;
        }
    }
    
    public void MakeAvailabel(Config.Weapon weapon) {
        switch (weapon) {
            case Config.Weapon.Baseballbat:
                Instantiate(weapons[0]);
                break;
            case Config.Weapon.Bow:
                Instantiate(weapons[1]);
                break;
            case Config.Weapon.Dagger:
                Instantiate(weapons[2]);
                break;
            case Config.Weapon.Sword:
                Instantiate(weapons[7]);
                break;
            case Config.Weapon.Machete:
                Instantiate(weapons[3]);
                break;
            case Config.Weapon.Pistol:
                Instantiate(weapons[4]);
                break;
            case Config.Weapon.Spear:
                Instantiate(weapons[6]);
                break;
            case Config.Weapon.Scythe:
                Instantiate(weapons[5]);
                break;
        }
    }
}
