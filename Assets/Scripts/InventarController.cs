using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarController : MonoBehaviour
{
    public GameObject[] items;
    public GameObject[] weapons;

    public void MakeAvailabel(Config.Item item)
    {
        switch (item)
        {
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
    
    public void MakeAvailabel(Config.Weapon weapon)
    {
        switch (weapon)
        {
            case Config.Weapon.Baseballbat:
                Instantiate(items[0]);
                break;
            case Config.Weapon.Bow:
                Instantiate(items[1]);
                break;
            case Config.Weapon.Dagger:
                Instantiate(items[2]);
                break;
            case Config.Weapon.Sword:
                Instantiate(items[7]);
                break;
            case Config.Weapon.Machete:
                Instantiate(items[3]);
                break;
            case Config.Weapon.Pistol:
                Instantiate(items[4]);
                break;
            case Config.Weapon.Spear:
                Instantiate(items[6]);
                break;
            case Config.Weapon.Scythe:
                Instantiate(items[5]);
                break;
        }
    }
}
