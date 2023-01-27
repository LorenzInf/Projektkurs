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
    private Config.Weapon type;

    public WeaponController(string name,bool usesAmmo,int durability,double damage,double dist,Config.Weapon type) {
        this.name = name;
        this.usesAmmo = usesAmmo;
        this.durability = durability;
        this.damage = damage;
        this.dist = dist;
        this.type = type;
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

    public Config.Weapon GetType(){
        return type;
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

    public static WeaponController CreateWeapon(Config.Weapon weapon){
        WeaponController wc = null;
        switch (weapon) {
            case Config.Weapon.Baseballbat:
                wc = new WeaponController(weapon.ToString().ToLower(),false,100,10,0.5,Config.Weapon.Baseballbat);
                break;
            case Config.Weapon.Bow:
                wc = new WeaponController(weapon.ToString().ToLower(),true,20,15,10,Config.Weapon.Bow);
                break;
            case Config.Weapon.Dagger:
                wc = new WeaponController(weapon.ToString().ToLower(),false,20,15,0.5,Config.Weapon.Dagger);
                break;
            case Config.Weapon.Sword:
                wc = new WeaponController(weapon.ToString().ToLower(),false,20,20,1,Config.Weapon.Sword);
                break;
            case Config.Weapon.Machete:
                wc = new WeaponController(weapon.ToString().ToLower(),false,25,20,1,Config.Weapon.Machete);
                break;
            case Config.Weapon.Pistol:
                wc = new WeaponController(weapon.ToString().ToLower(),false,30,30,10,Config.Weapon.Pistol);
                break;
            case Config.Weapon.Spear:
                wc = new WeaponController(weapon.ToString().ToLower(),false,5,30,2,Config.Weapon.Spear);
                break;
            case Config.Weapon.Scythe:
                wc = new WeaponController(weapon.ToString().ToLower(),false,10,40,2,Config.Weapon.Scythe);
                break;
        }
        //wc.ReduceDurabilityBy(TypingController.Levenshtein(name,input));
        return wc;
    }
}