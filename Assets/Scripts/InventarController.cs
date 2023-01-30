using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarController : MonoBehaviour{

    public GameObject[] items;
    public GameObject[] weapons;

	private GameObject[] itemObjects=new GameObject[3];

    public void MakeAvailabel(Config.Item item) {
        switch (item) {
            case Config.Item.AmmoBox:
                itemObjects[0]=Instantiate(items[0]);
                break;
            case Config.Item.HealingPotion:
                itemObjects[1]=Instantiate(items[1]);
                break;
            case Config.Item.RepairKit:
                itemObjects[2]=Instantiate(items[2]);
                break;
        }
    }

	public void Remove(string item){
		switch (item) {
            case "ammobox":
				Destroy(itemObjects[0]);
                itemObjects[0]=null;
                break;
            case "healingpotion":
				Destroy(itemObjects[1]);
                itemObjects[1]=null;
                break;
            case "repairkit":
				Destroy(itemObjects[2]);
                itemObjects[2]=null;
                break;
        }
	}
    
    public void MakeAvailabel(Config.Weapon weapon) {
        switch (weapon) {
            case Config.Weapon.Bat:
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
