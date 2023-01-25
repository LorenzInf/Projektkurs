using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController{
    
    private string name;
    private bool usesAmmo;
    private int ammo;
    private int durability;
    private int _currentDurability;

    public WeaponController(string name,bool usesAmmo,int durability)
    {
        this.name = name;
        this.usesAmmo = usesAmmo;
        this.durability = durability;
        ammo = 5;
        _currentDurability = durability;
    }

    public void Use(){
        if (CanBeUsed()) {
            if (usesAmmo)
                ammo--;
            _currentDurability--;
        }
    }

    public bool CanBeUsed(){
        if (_currentDurability < 1)
            return false;
        if (usesAmmo)
            return ammo > 0;
        return true;
    }

    public void Repair(int ammount){
        _currentDurability += ammount;
        if (_currentDurability > durability)
            _currentDurability = durability;
    }

    public void ReduceDurabilityBy(int factor){
        durability = durability / (factor+1);
    }

    public static WeaponController CreateNewWeapon(string name,string input)
    {
        WeaponController wc = null;
        switch (name)
        {
            case "Branch":
                wc = new WeaponController(input,false,10);
                break;
            case "Knife":
                wc = new WeaponController(input,false,10);
                break;
            case "Dagger":
                wc = new WeaponController(input,false,10);
                break;
            case "Spear":
                wc = new WeaponController(input,false,10);
                break;
            case "Scythe":
                wc = new WeaponController(input,false,10);
                break;
            case "Pistol":
                wc = new WeaponController(input,true,10);
                break;
            case "Bow":
                wc = new WeaponController(input,true,10);
                break;
        }
        wc.ReduceDurabilityBy(TypingController.Levenshtein(name,input));
        return wc;
    }
}
