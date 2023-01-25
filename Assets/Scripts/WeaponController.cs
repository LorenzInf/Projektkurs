using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController{
    
    private string name;
    private bool usesAmmo;
    private int ammo;
    private int durability;
    private double damage;
    private int _currentDurability;

    public WeaponController(string name,bool usesAmmo,int durability,double damage) {
        this.name = name;
        this.usesAmmo = usesAmmo;
        this.durability = durability;
        this.damage = damage;
        ammo = 0;
        _currentDurability = durability;
    }

    public string GetName(){
        return name;
    }

    public double Use(){
        if (CanBeUsed()) {
            if (usesAmmo)
                ammo--;
            _currentDurability--;
            return damage;
        }
        return 0;
    }

    public bool CanBeUsed(){
        if (_currentDurability < 1)
            return false;
        if (usesAmmo)
            return ammo > 0;
        return true;
    }

    public void Repair(){
        _currentDurability = durability;
    }

    public void Refill(){
        ammo = 10;
    }

    public void ReduceDurabilityBy(int factor){
        damage = damage / (factor+1);
    }

    public static WeaponController CreateNewWeapon(string name,string input)
    {
        WeaponController wc = null;
        switch (name)
        {
            case "Branch":
                wc = new WeaponController(input,false,100,10);
                break;
            case "Knife":
                wc = new WeaponController(input,false,30,20);
                break;
            case "Dagger":
                wc = new WeaponController(input,false,20,30);
                break;
            case "Spear":
                wc = new WeaponController(input,false,5,40);
                break;
            case "Scythe":
                wc = new WeaponController(input,false,5,50);
                break;
            case "Pistol":
                wc = new WeaponController(input,true,20,70);
                break;
            case "Bow":
                wc = new WeaponController(input,true,30,60);
                break;
        }
        wc.ReduceDurabilityBy(TypingController.Levenshtein(name,input));
        return wc;
    }
}
