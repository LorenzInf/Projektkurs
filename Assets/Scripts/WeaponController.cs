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
    private double dist;

    public WeaponController(string name,bool usesAmmo,int durability,double damage,double dist) {
        this.name = name;
        this.usesAmmo = usesAmmo;
        this.durability = durability;
        this.damage = damage;
        this.dist = dist;
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

    public double GetRange(){
        return dist;
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

    public static WeaponController CreateWeapon(Config.Weapon weapon)
    {
        WeaponController wc = null;
        switch (weapon)
        {
            case Config.Weapon.Basballbat:
                wc = new WeaponController(weapon.ToString(),false,100,10,0.5);
                break;
            case Config.Weapon.Bow:
                wc = new WeaponController(weapon.ToString(),true,20,15,10);
                break;
            case Config.Weapon.Dagger:
                wc = new WeaponController(weapon.ToString(),false,20,15,0.5);
                break;
            case Config.Weapon.Sword:
                wc = new WeaponController(weapon.ToString(),false,20,20,1);
                break;
            case Config.Weapon.Machete:
                wc = new WeaponController(weapon.ToString(),false,25,20,1);
                break;
            case Config.Weapon.Pistol:
                wc = new WeaponController(weapon.ToString(),false,30,30,10);
                break;
            case Config.Weapon.Spear:
                wc = new WeaponController(weapon.ToString(),false,5,30,2);
                break;
            case Config.Weapon.Scythe:
                wc = new WeaponController(weapon.ToString(),false,10,40,2);
                break;
        }
        //wc.ReduceDurabilityBy(TypingController.Levenshtein(name,input));
        return wc;
    }
}
